using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace Vistas.E2E
{
    /* Un "navegador" mínimo para WebForms: mantiene cookies (la sesión) y arma
       los postbacks como lo haría el navegador (campos del formulario,
       __VIEWSTATE, __EVENTTARGET para los LinkButton). No ejecuta JavaScript,
       así que también prueba la validación del lado del servidor. */
    public sealed class Navegador : IDisposable
    {
        // __doPostBack('target','arg') o, si el link valida, WebForm_PostBackOptions("target", "arg", ...).
        static readonly Regex DoPostBack = new(@"__doPostBack\('(?<target>[^']*)','(?<arg>[^']*)'\)|WebForm_PostBackOptions\(""(?<target>[^""]*)"", ""(?<arg>[^""]*)""");

        readonly HttpClient cliente;
        readonly Uri direccion;
        readonly CookieContainer cookies = new();

        public Navegador(Uri direccion)
        {
            this.direccion = direccion;
            cliente = new HttpClient(new HttpClientHandler { CookieContainer = cookies, AllowAutoRedirect = true })
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public Cookie Cookie(string nombre)
        {
            return cookies.GetCookies(direccion)[nombre];
        }

        /* La respuesta sin procesar, para revisar headers. */
        public Task<HttpResponseMessage> Pedir(string ruta)
        {
            return cliente.GetAsync(new Uri(direccion, ruta));
        }

        public void PonerCookie(string nombre, string valor)
        {
            cookies.Add(direccion, new Cookie(nombre, valor));
        }

        public async Task<Pagina> Abrir(string ruta)
        {
            return await Leer(await cliente.GetAsync(new Uri(direccion, ruta)));
        }

        /* Hace click en un botón (input submit) identificado por su ID de servidor. */
        public Task<Pagina> Enviar(Pagina pagina, string idBoton, IDictionary<string, string> campos = null)
        {
            IElement boton = pagina.Control(idBoton);
            var valores = ValoresDelFormulario(pagina, campos);
            valores[boton.GetAttribute("name")] = boton.GetAttribute("value");
            return Postear(pagina, valores);
        }

        /* Hace click en un LinkButton (o cualquier link "javascript:__doPostBack"). */
        public Task<Pagina> Click(Pagina pagina, IElement link, IDictionary<string, string> campos = null)
        {
            Match postback = DoPostBack.Match(WebUtility.HtmlDecode(link.GetAttribute("href") ?? ""));
            if (!postback.Success)
            {
                throw new InvalidOperationException("El link no hace postback: " + link.OuterHtml);
            }
            var valores = ValoresDelFormulario(pagina, campos);
            valores["__EVENTTARGET"] = postback.Groups["target"].Value;
            valores["__EVENTARGUMENT"] = postback.Groups["arg"].Value;
            return Postear(pagina, valores);
        }

        private async Task<Pagina> Postear(Pagina pagina, Dictionary<string, string> valores)
        {
            IHtmlFormElement form = pagina.Documento.Forms[0];
            Uri accion = new Uri(pagina.Url, WebUtility.HtmlDecode(form.GetAttribute("action")));
            return await Leer(await cliente.PostAsync(accion, new FormUrlEncodedContent(valores)));
        }

        /* Lo que el navegador mandaría: campos de texto, ocultos, selects y los
           radios/checkboxes marcados. "campos" pisa valores por ID de servidor. */
        private static Dictionary<string, string> ValoresDelFormulario(Pagina pagina, IDictionary<string, string> campos)
        {
            var valores = new Dictionary<string, string>();
            foreach (IElement input in pagina.Documento.QuerySelectorAll("form input[name]"))
            {
                string tipo = (input.GetAttribute("type") ?? "text").ToLowerInvariant();
                if (tipo == "submit" || tipo == "button" || tipo == "image") continue;
                if ((tipo == "radio" || tipo == "checkbox") && !input.HasAttribute("checked")) continue;
                valores[input.GetAttribute("name")] = input.GetAttribute("value") ?? "";
            }
            foreach (IHtmlSelectElement select in pagina.Documento.QuerySelectorAll("form select[name]").OfType<IHtmlSelectElement>())
            {
                IHtmlOptionElement elegida = select.Options.FirstOrDefault(o => o.IsSelected) ?? select.Options.FirstOrDefault();
                valores[select.Name] = elegida == null ? "" : elegida.Value;
            }
            foreach (IElement area in pagina.Documento.QuerySelectorAll("form textarea[name]"))
            {
                valores[area.GetAttribute("name")] = area.TextContent;
            }
            if (campos != null)
            {
                foreach (var campo in campos)
                {
                    valores[pagina.Control(campo.Key).GetAttribute("name")] = campo.Value;
                }
            }
            return valores;
        }

        private static async Task<Pagina> Leer(HttpResponseMessage respuesta)
        {
            string html = await respuesta.Content.ReadAsStringAsync();
            if (respuesta.StatusCode != HttpStatusCode.OK)
            {
                throw new InvalidOperationException($"{respuesta.RequestMessage.RequestUri} devolvió {(int)respuesta.StatusCode}:\n{ErrorDelServidor(html)}");
            }
            return new Pagina(respuesta.RequestMessage.RequestUri, html);
        }

        private static string ErrorDelServidor(string html)
        {
            Match error = Regex.Match(html, @"<title>(.*?)</title>", RegexOptions.Singleline);
            return error.Success ? WebUtility.HtmlDecode(error.Groups[1].Value.Trim()) : html.Substring(0, Math.Min(html.Length, 500));
        }

        public void Dispose()
        {
            cliente.Dispose();
        }
    }

    public sealed class Pagina
    {
        public Pagina(Uri url, string html)
        {
            Url = url;
            Html = html;
            Documento = new HtmlParser().ParseDocument(html);
        }

        public Uri Url { get; }
        public string Html { get; }
        public IHtmlDocument Documento { get; }

        public string Ruta
        {
            get { return Url.AbsolutePath; }
        }

        public IEnumerable<string> Alertas
        {
            get { return Documento.QuerySelectorAll("[role=alert]").Select(a => a.TextContent.Trim()); }
        }

        public string Texto
        {
            get { return Documento.Body.TextContent; }
        }

        /* Elemento de un control de servidor por su ID (WebForms le agrega prefijos al name). */
        public IElement Control(string id)
        {
            IElement elemento = Documento.QuerySelectorAll("[name]")
                .FirstOrDefault(e => e.GetAttribute("name").EndsWith("$" + id, StringComparison.Ordinal));
            if (elemento == null)
            {
                throw new InvalidOperationException($"No encontré el control '{id}' en {Ruta}.");
            }
            return elemento;
        }

        /* Elemento por el ID de servidor, para controles sin "name" (LinkButton). */
        public IElement PorId(string id)
        {
            IElement elemento = Documento.QuerySelectorAll("[id]")
                .FirstOrDefault(e => e.Id == id || e.Id.EndsWith("_" + id, StringComparison.Ordinal));
            if (elemento == null)
            {
                throw new InvalidOperationException($"No encontré el elemento '{id}' en {Ruta}.");
            }
            return elemento;
        }

        public IElement[] Links(string selector)
        {
            return Documento.QuerySelectorAll(selector).ToArray();
        }
    }
}
