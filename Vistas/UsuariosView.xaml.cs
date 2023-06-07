using SugahriStore.Datos;
using SugahriStore.Modelos;
using SugahriStore.Vistas;

namespace SugahriStore;
    public partial class UsuariosView : ContentPage
    {
        public MainPage MainPageView;
        public RegistroUsuariosAdminView AdminView;
        public ModificarUsuariosAdminView ModificarUsuarioView;
        private List<Usuario> _usuarios;
        private List<Usuario> _usuariosFiltrados;
        private UsuariosRepositorio UsuariosRepositorio = new UsuariosRepositorio();
        private RolesRepositorio RolesRepositorio = new RolesRepositorio();
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

        private void FiltrarPorNombre(string filtro)
        {
            Usuarios = _usuarios.Where(u => u.Nombre.ToLower().Contains(filtro.ToLower())).ToList();

            if (!Usuarios.Any())
            {
                Usuarios = _usuarios;
            }
        }

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

        private async void Regresar(object sender, EventArgs e)
        {
            await MainPageView.Navigation.PopAsync();
        }
        private async void AdministrarUsuarios(object sender, EventArgs e)
        {
            // Obtener el usuario seleccionado
            var button = (Button)sender;
            var usuario = button?.BindingContext as Usuario;
               if (usuario != null)
               {
                 // Crear la p�gina de modificaci�n de usuario y pasar el usuario como par�metro
                 ModificarUsuarioView = new ModificarUsuariosAdminView(usuario);
                 await Navigation.PushAsync(ModificarUsuarioView);
               }
        }

        private async void A�adirUsuario(object sender, EventArgs e)
        {
            await Navigation.PushAsync(AdminView);

        }
}

