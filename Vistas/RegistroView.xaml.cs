using SugahriStore.Datos;
using SugahriStore.Modelos;

namespace SugahriStore;

public partial class RegistroView : ContentPage
{
    private UsuariosRepositorio UsuariosRepositorio = new();
    private MainPage mainPage;

    public RegistroView()
    {
        InitializeComponent();
    }

    private void Registro(object sender, EventArgs e)
    {
        // Verificar si los campos están vacíos
        if (string.IsNullOrWhiteSpace(NombreUsuario.Text) || string.IsNullOrWhiteSpace(ContraseñaUsuario.Text) || string.IsNullOrWhiteSpace(ConfirmarContraseña.Text))
        {
            DisplayAlert("Error", "Todos los campos son obligatorios", "Aceptar");
        }
        // Verificar si las contraseñas coinciden
        else if (!ContraseñaUsuario.Text.Equals(ConfirmarContraseña.Text))
        {
            DisplayAlert("Error", "Las contraseñas introducidas no coinciden", "Aceptar");
        }
        // Verificar si el nombre de usuario tiene al menos 3 caracteres
        else if (NombreUsuario.Text.Length < 3)
        {
            DisplayAlert("Error", "El nombre de usuario no puede tener menos de 3 caracteres", "Aceptar");
        }
        // Verificar si la contraseña tiene al menos 5 caracteres y contiene letras y números
        else if (ContraseñaUsuario.Text.Length < 5 || !ContraseñaUsuario.Text.Any(char.IsLetter) || !ContraseñaUsuario.Text.Any(char.IsDigit))
        {
            DisplayAlert("Error", "La contraseña debe tener al menos 5 caracteres y contener letras y números", "Aceptar");
        }
        // Verificar si el usuario ya está registrado en la base de datos
        else if (UsuariosRepositorio.UsuarioRegistrado(NombreUsuario.Text))
        {
            DisplayAlert("Error", "Ese usuario ya existe en la base de datos", "Aceptar");
        }
        else
        {
            // Crear un nuevo objeto de Usuario con los datos ingresados
            Usuario usuario = new Usuario(NombreUsuario.Text, ContraseñaUsuario.Text, 2);

            // Agregar el usuario a la base de datos
            UsuariosRepositorio.AgregarUsuario(usuario);

            DisplayAlert("Registro Correcto", "Te registraste correctamente en la aplicación", "Aceptar");

            // Crear la página principal y pasar el usuario como parámetro
            mainPage = new MainPage(usuario);
            Navigation.PushAsync(mainPage);
        }
    }

}

