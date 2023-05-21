namespace SugahriStore.Modelos
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<Usuario> Usuarios { get; set; } // navegación a la clase de usuarios
        public Rol()
        {
            // constructor vacío
            Usuarios = new List<Usuario>();
        }

        public Rol(string nombre) : this()
        {
            Nombre = nombre;
        }
    }
}
