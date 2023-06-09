using SugahriStore.Modelos;
using System.Security.Cryptography;
using System.Text;


namespace SugahriStore.Lógica
{
    public class Login
    {
        // Método privado para hashear la contraseña
        private static string HashContraseña(string password)
        {
            byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash;
        }
        // Método para hacer el login del uusario
        public bool LoginUser(string nombreUsuario, string contraseña, Usuario usuario)
        {
            // Generar el hash de la contraseña ingresada
            string hashcontraseña = HashContraseña(contraseña);

            // Verificar si el nombre de usuario y la contraseña coinciden con el usuario proporcionado
            if (usuario.Nombre == nombreUsuario && usuario.Contraseña == hashcontraseña)
                return true; // Si coinciden, se retorna verdadero
            else
                return false; // Si no coinciden, se retorna falso
        }

    }
}
