using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SugahriStore.Modelos
{
    public class Usuario
    {
        private readonly int id;
        private string nombre;
        private string contraseña;
        public int RolId { get; set; } // clave foránea

        public Rol Rol { get; set; } // navegación a la clase de roles

        public Usuario()
        {
        }
        public Usuario(string nombre, string contraseña, int rolId)
        {
            Nombre = nombre;
            Contraseña = contraseña;
            RolId = rolId;
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        public int Id
        {
            get { return id; }
            
        }

        public string Contraseña
        {
            get { return contraseña; }
            set { contraseña = value; }
        }
    }
}
