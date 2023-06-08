using SugahriStore.Datos;
using SugahriStore.Modelos;
using System.Collections.Generic;
using System.Linq;

namespace SugahriStore.Datos
{
    public class RolesRepositorio
    {
        private readonly BaseDeDatosContext _context;

        public RolesRepositorio()
        {
            _context = new BaseDeDatosContext();
        }

        // Obtener todos los roles de la base de datos
        public List<Rol> ObtenerTodosRoles()
        {
            return _context.Roles.ToList();
        }

        // Obtener un rol por su ID
        public Rol ObtenerRolPorId(int id)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == id);
        }

        // Rellenar los roles de una lista de usuarios
        public void RellenarRoles(List<Usuario> usuarios)
        {
            var roles = ObtenerTodosRoles();

            foreach (var usuario in usuarios)
            {
                usuario.Rol = roles.FirstOrDefault(r => r.Id == usuario.RolId);
            }
        }

        // Agregar un nuevo rol a la base de datos
        public void AgregarRol(Rol rol)
        {
            _context.Roles.Add(rol);
            _context.SaveChanges();
        }

        // Actualizar un rol en la base de datos
        public void ActualizarRol(Rol rol)
        {
            _context.Roles.Update(rol);
            _context.SaveChanges();
        }

        // Eliminar un rol de la base de datos por su ID
        public void EliminarRol(int id)
        {
            var rol = _context.Roles.FirstOrDefault(r => r.Id == id);
            if (rol != null)
            {
                _context.Roles.Remove(rol);
                _context.SaveChanges();
            }
        }
    }
}
