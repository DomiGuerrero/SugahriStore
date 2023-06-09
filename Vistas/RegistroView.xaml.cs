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
        // Verificar si los campos est�n vac�os
        if (string.IsNullOrWhiteSpace(NombreUsuario.Text) || string.IsNullOrWhiteSpace(Contrase�aUsuario.Text) || string.IsNullOrWhiteSpace(ConfirmarContrase�a.Text))
        {
            DisplayAlert("Error", "Todos los campos son obligatorios", "Aceptar");
        }
        // Verificar si las contrase�as coinciden
        else if (!Contrase�aUsuario.Text.Equals(ConfirmarContrase�a.Text))
        {
            DisplayAlert("Error", "Las contrase�as introducidas no coinciden", "Aceptar");
        }
        // Verificar si el nombre de usuario tiene al menos 3 caracteres
        else if (NombreUsuario.Text.Length < 3)
        {
            DisplayAlert("Error", "El nombre de usuario no puede tener menos de 3 caracteres", "Aceptar");
        }
        // Verificar si la contrase�a tiene al menos 5 caracteres y contiene letras y n�meros
        else if (Contrase�aUsuario.Text.Length < 5 || !Contrase�aUsuario.Text.Any(char.IsLetter) || !Contrase�aUsuario.Text.Any(char.IsDigit))
        {
            DisplayAlert("Error", "La contrase�a debe tener al menos 5 caracteres y contener letras y n�meros", "Aceptar");
        }
        // Verificar si el usuario ya est� registrado en la base de datos
        else if (UsuariosRepositorio.UsuarioRegistrado(NombreUsuario.Text))
        {
            DisplayAlert("Error", "Ese usuario ya existe en la base de datos", "Aceptar");
        }
        else
        {
            // Crear un nuevo objeto de Usuario con los datos ingresados
            Usuario usuario = new Usuario(NombreUsuario.Text, Contrase�aUsuario.Text, 2);

            // Agregar el usuario a la base de datos
            UsuariosRepositorio.AgregarUsuario(usuario);

            DisplayAlert("Registro Correcto", "Te registraste correctamente en la aplicaci�n", "Aceptar");

            // Crear la p�gina principal y pasar el usuario como par�metro
            mainPage = new MainPage(usuario);
            Navigation.PushAsync(mainPage);
        }
    }

}

