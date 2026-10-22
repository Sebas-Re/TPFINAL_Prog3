using System;
using System.Security.Cryptography;

namespace Dao
{
    /* Hash de contraseñas con PBKDF2-SHA256 y salt aleatorio. Se guarda como
       "pbkdf2$iteraciones$salt$hash" (salt y hash en Base64), así se puede subir
       la cantidad de iteraciones sin invalidar las contraseñas existentes. */
    public static class Contrasenas
    {
        const string Prefijo = "pbkdf2$";
        const int BytesSalt = 16;
        const int BytesHash = 32;

        /* Los tests la bajan para no tardar en cada base que crean. */
        public static int Iteraciones { get; set; } = 100000;

        public static string Hashear(string contraseña)
        {
            byte[] salt = new byte[BytesSalt];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            byte[] hash = Derivar(contraseña, salt, Iteraciones);
            return Prefijo + Iteraciones + "$" + Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(hash);
        }

        public static bool Verificar(string contraseña, string guardada)
        {
            if (contraseña == null || !EstaHasheada(guardada))
            {
                return false;
            }
            string[] partes = guardada.Split('$');
            int iteraciones;
            if (partes.Length != 4 || !int.TryParse(partes[1], out iteraciones))
            {
                return false;
            }
            byte[] salt = Convert.FromBase64String(partes[2]);
            byte[] esperado = Convert.FromBase64String(partes[3]);
            return IgualesEnTiempoConstante(Derivar(contraseña, salt, iteraciones), esperado);
        }

        /* Para que un usuario inexistente tarde lo mismo que uno con contraseña
           incorrecta (si no, el tiempo de respuesta revela qué usuarios existen). */
        public static void SimularVerificacion(string contraseña)
        {
            Derivar(contraseña ?? "", new byte[BytesSalt], Iteraciones);
        }

        public static bool EstaHasheada(string valor)
        {
            return valor != null && valor.StartsWith(Prefijo, StringComparison.Ordinal);
        }

        static byte[] Derivar(string contraseña, byte[] salt, int iteraciones)
        {
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(contraseña, salt, iteraciones, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(BytesHash);
            }
        }

        static bool IgualesEnTiempoConstante(byte[] a, byte[] b)
        {
            int diferencia = a.Length ^ b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diferencia |= a[i] ^ b[i];
            }
            return diferencia == 0;
        }
    }
}
