namespace SugahriStore.Modelos
{
    public class Rol
    {
        public int Id { get; set; } // Identificador del rol
        public string Nombre { get; set; } // Nombre del rol
        public List<Usuario> Usuarios { get; set; } // Lista de usuarios asociados al rol (relación de navegación)
        public Rol()
        {
            // Constructor vacío utilizado para inicializar la lista de usuarios
            Usuarios = new List<Usuario>();
        }

        public Rol(string nombre) : this()
        {
            Nombre = nombre; // Constructor que acepta el nombre del rol y lo asigna
        }
    }

}
