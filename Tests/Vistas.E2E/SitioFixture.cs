using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using Xunit;

namespace Vistas.E2E
{
    /* Levanta Vistas en IIS Express, en un puerto libre y con una base SQLite
       temporal (variable PCBOOSTER_DB), una sola vez para todos los tests. */
    public sealed class SitioFixture : IDisposable
    {
        readonly Process iisExpress;
        readonly string carpetaTemporal;

        public Uri Direccion { get; }

        public SitioFixture()
        {
            string raiz = BuscarRaizRepositorio();
            string sitio = Path.Combine(raiz, "Vistas");
            if (!File.Exists(Path.Combine(sitio, "bin", "Vistas.dll")))
            {
                throw new InvalidOperationException(
                    "No encontré Vistas\\bin\\Vistas.dll. Compilá la solución en Visual Studio (o con MSBuild) antes de correr los tests E2E.");
            }

            carpetaTemporal = Path.Combine(Path.GetTempPath(), "pcbooster-e2e-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(carpetaTemporal);
            int puerto = PuertoLibre();
            Direccion = new Uri("http://localhost:" + puerto + "/");

            var inicio = new ProcessStartInfo(BuscarIisExpress(), $"/path:\"{sitio}\" /port:{puerto} /clr:v4.0 /systray:false")
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            inicio.EnvironmentVariables["PCBOOSTER_DB"] = Path.Combine(carpetaTemporal, "PCbooster.db");
            iisExpress = Process.Start(inicio);
            iisExpress.OutputDataReceived += (s, e) => { };
            iisExpress.BeginOutputReadLine();
            iisExpress.BeginErrorReadLine();

            EsperarQueResponda();
        }

        private void EsperarQueResponda()
        {
            using (var cliente = new HttpClient { Timeout = TimeSpan.FromSeconds(10) })
            {
                DateTime limite = DateTime.Now.AddSeconds(60);
                while (DateTime.Now < limite)
                {
                    if (iisExpress.HasExited)
                    {
                        throw new InvalidOperationException("IIS Express terminó con código " + iisExpress.ExitCode + ".");
                    }
                    try
                    {
                        if (cliente.GetAsync(new Uri(Direccion, "Inicio.aspx")).Result.StatusCode == HttpStatusCode.OK)
                        {
                            return;
                        }
                    }
                    catch (AggregateException)
                    {
                        // Todavía no escucha.
                    }
                    Thread.Sleep(500);
                }
            }
            throw new TimeoutException("El sitio no respondió en 60 segundos en " + Direccion);
        }

        public void Dispose()
        {
            try
            {
                if (!iisExpress.HasExited)
                {
                    iisExpress.Kill();
                    iisExpress.WaitForExit(10000);
                }
            }
            catch (InvalidOperationException)
            {
            }
            try { Directory.Delete(carpetaTemporal, true); } catch (IOException) { } catch (UnauthorizedAccessException) { }
        }

        private static string BuscarRaizRepositorio()
        {
            DirectoryInfo dir = new(AppDomain.CurrentDomain.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "TP_Integrador.sln")))
            {
                dir = dir.Parent;
            }
            if (dir == null)
            {
                throw new InvalidOperationException("No encontré TP_Integrador.sln subiendo desde " + AppDomain.CurrentDomain.BaseDirectory);
            }
            return dir.FullName;
        }

        private static string BuscarIisExpress()
        {
            foreach (string programas in new[] { Environment.GetEnvironmentVariable("ProgramFiles"), Environment.GetEnvironmentVariable("ProgramFiles(x86)") })
            {
                if (programas == null) continue;
                string ruta = Path.Combine(programas, "IIS Express", "iisexpress.exe");
                if (File.Exists(ruta)) return ruta;
            }
            throw new InvalidOperationException("No encontré IIS Express (se instala con Visual Studio).");
        }

        private static int PuertoLibre()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int puerto = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return puerto;
        }
    }

    [CollectionDefinition("Sitio")]
    public class SitioCollection : ICollectionFixture<SitioFixture>
    {
    }
}
