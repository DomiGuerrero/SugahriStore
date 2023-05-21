using SugahriStore.Datos;
using SugahriStore.Modelos;
using System.Collections.Generic;
using System.Linq;

namespace SugahriStore.Datos
{
    public class RolesRepositorio
    {
        private  readonly BaseDeDatosContext _context;

        public RolesRepositorio()
        {
            _context = new BaseDeDatosContext();
        }

        public List<Rol> ObtenerTodosRoles()
        {
            return _context.Roles.ToList();
        }

        public Rol ObtenerRolPorId(int id)
        {
            return _context.Roles.FirstOrDefault(r => r.Id == id);
        }

        public void AgregarRol(Rol rol)
        {
            _context.Roles.Add(rol);
            _context.SaveChanges();
        }

        public void ActualizarRol(Rol rol)
        {
            _context.Roles.Update(rol);
            _context.SaveChanges();
        }

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
