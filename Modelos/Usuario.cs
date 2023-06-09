
namespace SugahriStore.Modelos
{
    public class Usuario
    {
        private int id; // Identificador del usuario
        private string nombre; // Nombre del usuario
        private string contraseña; // Contraseña del usuario
        public int RolId { get; set; } // Identificador del rol asociado al usuario
        public Rol Rol { get; set; } // Objeto Rol asociado al usuario

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
            set { nombre = value; } // Establece el nombre del usuario
        }

        public int Id
        {
            get { return id; }
            set { id = value; } // Establece el identificador del usuario
        }

        public string Contraseña
        {
            get { return contraseña; }
            set { contraseña = value; } // Establece la contraseña del usuario
        }
    }
}
