using SugahriStore.Datos;
using SugahriStore.Modelos;

namespace SugahriStore;

public partial class RegistroView : ContentPage
{
    private UsuariosRepositorio UsuariosRepositorio = new();
    private RolesRepositorio RolesRepositorio = new();
    private MainPage mainPage;
    public RegistroView()
    {
        InitializeComponent();
    }

    private void Registro(object sender, EventArgs e)
    {
        if (!Contrase�aUsuario.Text.Equals(ConfirmarContrase�a.Text))
        {
            DisplayAlert("Error", "Las contrase�as introducidas no coinciden", "Aceptar");
        }
        else if (NombreUsuario.Text.Length < 3)
        {
            DisplayAlert("Error", "El nombre de usuario no puede tener menos de 3 caracteres", "Aceptar");
        }
        else if (UsuariosRepositorio.UsuarioRegistrado(NombreUsuario.Text))
        {
            DisplayAlert("Error", "Ese usuario ya existe en la base de datos", "Aceptar");
        }
        else
        {
            Usuario usuario = new Usuario(NombreUsuario.Text, Contrase�aUsuario.Text, 1);

            // Crea la informaci�n adicional del usuario y as�gnala al usuario

            UsuariosRepositorio.AgregarUsuario(usuario);

            DisplayAlert("Registro Correcto", "Te registraste correctamente en la aplicaci�n", "Aceptar");
            mainPage = new MainPage(usuario);
            Navigation.PushAsync(mainPage);
        }
    }


}