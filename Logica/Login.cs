using SugahriStore.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
      
        public bool LoginUser(string nombreUsuario, string contraseña, Usuario usuario)
        {
            string hashcontraseña = HashContraseña(contraseña);
            if (usuario.Nombre == nombreUsuario && usuario.Contraseña == hashcontraseña)
                return true;
            else
                return false;
        }
    }
}
