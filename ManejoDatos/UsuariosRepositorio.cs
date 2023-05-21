using Microsoft.EntityFrameworkCore;
using Prototipo_1_SugahriStore.Modelos;
using System.Security.Cryptography;
using System.Text;

namespace Prototipo_1_SugahriStore.ManejoDatos
{
    public class UsuariosRepositorio
    {
        private readonly BaseDeDatosContext context;

        // Constructor que inicializa el contexto de la base de datos
        public UsuariosRepositorio()
        {
            context = new BaseDeDatosContext();
        }

        // Método para agregar un usuario a la tabla Usuarios
        public void AgregarUsuario(Usuario usuario, string password)
        {
            // Se hashea la contraseña antes de guardarla en la base de datos
            usuario.Contraseña = UsuariosRepositorio.HashContraseña(password);
            context.Usuarios.Add(usuario);
            context.SaveChanges();
        }

        // Método para obtener un usuario por su id
        public Usuario ObtenerUsuarioPorId(int id)
        {
            return context.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        // Método para obtener un usuario por su nombre
        public Usuario ObtenerUsuarioPorNombre(string nombre)
        {
            return context.Usuarios.FirstOrDefault(u => u.Nombre == nombre);
        }

        // Método para actualizar la información de un usuario
        public void ActualizarUsuario(Usuario usuario, string password)
        {
            // Se hashea la contraseña antes de guardarla en la base de datos
            usuario.Contraseña = UsuariosRepositorio.HashContraseña(password);
            context.Entry(usuario).State = EntityState.Modified;
            context.SaveChanges();
        }

        // Método para eliminar un usuario de la tabla Usuarios
        public void EliminarUsuario(int id)
        {
            Usuario usuario = context.Usuarios.Find(id);
            context.Usuarios.Remove(usuario);
            context.SaveChanges();
        }

        // Método privado para hashear la contraseña
        private static string HashContraseña(string password)
        {
            byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            string hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash;
        }

        // Método para comparar el hash de una contraseña con la almacenada en la base de datos
        public static bool CompararHash(string password, string hash)
        {
            StringBuilder hashedPassword = new();
            foreach (byte b in SHA256.HashData(Encoding.UTF8.GetBytes(password)))
            {
                hashedPassword.Append(b.ToString("x2"));
            }

            return hashedPassword.ToString() == hash;
        }
    }
}

