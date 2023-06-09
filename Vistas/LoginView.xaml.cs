using SugahriStore.Datos;
using SugahriStore.Modelos;
using SugahriStore.Lógica;

namespace SugahriStore;

public partial class LoginView : ContentPage
{
    readonly Login login = new(); // Instancia de la clase Login para gestionar el inicio de sesión
    readonly Usuario usuario = new(); // Instancia de la clase Usuario para almacenar los datos del usuario
    UsuariosRepositorio UsuariosRepositorio = new(); // Instancia del repositorio de usuarios para acceder a los datos de los usuarios

    public LoginView()
    {
        InitializeComponent();
    }

    // Método para manejar el evento de inicio de sesión
    public async void Login(object sender, EventArgs e)
    {
        usuario.Nombre = NombreUsuario.Text; // Obtener el nombre de usuario ingresado
        bool existe = UsuariosRepositorio.UsuarioRegistrado(usuario.Nombre); // Verificar si el usuario está registrado en el repositorio
        if (existe)
        {
            Usuario usuarioRegistrado = UsuariosRepositorio.ObtenerUsuarioPorNombre(usuario.Nombre); // Obtener el objeto Usuario correspondiente al nombre de usuario

            string password = ContraseñaUsuario.Text; // Obtener la contraseña ingresada
            if (string.IsNullOrEmpty(password))
            {
                MensajeError.Text = "Debe ingresar una contraseña"; // Mostrar mensaje de error si no se ingresa una contraseña
            }
            else if (login.LoginUser(NombreUsuario.Text, password, usuarioRegistrado))
            {
                await Navigation.PushAsync(new MainPage(usuarioRegistrado)); // Navegar a la página principal si el inicio de sesión es exitoso
            }
            else
            {
                MensajeError.Text = "Contraseña Incorrecta"; // Mostrar mensaje de error si la contraseña es incorrecta
            }
        }
        else
        {
            MensajeError.Text = "Este usuario no está registrado"; // Mostrar mensaje de error si el usuario no está registrado
        }
    }

    // Método para manejar el evento de registro
    public async void Registro(object sender, EventArgs e)
    {
        RegistroView RegistroView = new RegistroView(); // Crear una instancia de la vista de registro
        await Navigation.PushAsync(RegistroView); // Navegar a la vista de registro
    }
}




