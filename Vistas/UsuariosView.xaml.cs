using SugahriStore.Datos;
using SugahriStore.Modelos;
using SugahriStore.Vistas;

namespace SugahriStore;
public partial class UsuariosView : ContentPage
{
    // Variables de referencia a otras vistas y listas de usuarios
    public MainPage MainPageView; // Referencia a la vista MainPage
    public RegistroUsuariosAdminView AdminView; // Referencia a la vista RegistroUsuariosAdminView
    public ModificarUsuariosAdminView ModificarUsuarioView; // Referencia a la vista ModificarUsuariosAdminView
    private List<Usuario> _usuarios; // Lista de usuarios
    private List<Usuario> _usuariosFiltrados; // Lista de usuarios filtrados
    private UsuariosRepositorio UsuariosRepositorio = new UsuariosRepositorio(); // Repositorio de usuarios
    private RolesRepositorio RolesRepositorio = new RolesRepositorio(); // Repositorio de roles

    // Propiedad para obtener y establecer la lista de usuarios filtrados
    public List<Usuario> Usuarios
    {
        get => _usuariosFiltrados;
        set
        {
            _usuariosFiltrados = value;
            OnPropertyChanged(nameof(Usuarios));
        }
    }

    public UsuariosView(Usuario usuario, MainPage mainPage)
    {
        InitializeComponent();
        _usuarios = UsuariosRepositorio.ObtenerUsuarios();
        RolesRepositorio.RellenarRoles(_usuarios);
        _usuariosFiltrados = _usuarios;
        MainPageView = mainPage;
        AdminView = new();
        BindingContext = this;
    }

    // Método para filtrar la lista de usuarios por nombre
    private void FiltrarPorNombre(string filtro)
    {
        Usuarios = _usuarios.Where(u => u.Nombre.ToLower().Contains(filtro.ToLower())).ToList();

        if (!Usuarios.Any())
        {
            Usuarios = _usuarios;
        }
    }

    // Método de cambio de texto en el campo de búsqueda
    private void CambioDeTexto(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            Usuarios = _usuarios;
        }
        else
        {
            FiltrarPorNombre(e.NewTextValue);
        }
    }

    //Método para regresar a la página anterior
    private async void Regresar(object sender, EventArgs e)
    {
        await MainPageView.Navigation.PopAsync();
    }

    // Método para administrar usuarios
    private async void AdministrarUsuarios(object sender, EventArgs e)
    {
        // Obtener el usuario seleccionado
        var button = (Button)sender;
        var usuario = button?.BindingContext as Usuario;

        if (usuario != null)
        {
            // Verificar si el nombre de usuario es "ADMIN"
            if (usuario.Nombre.Equals("ADMIN"))
            {
                await DisplayAlert("Acceso denegado", "No se puede modificar el usuario ADMIN", "Aceptar");
            }
            else
            {
                // Crear la página de modificación de usuario y pasar el usuario como parámetro
                ModificarUsuarioView = new ModificarUsuariosAdminView(usuario);
                await Navigation.PushAsync(ModificarUsuarioView);
            }
        }
    }

    // Método para insertar un nuevo usuario
    private async void InsertarUsuario(object sender, EventArgs e)
    {
        await Navigation.PushAsync(AdminView);
    }
}



