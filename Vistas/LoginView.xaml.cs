using Microsoft.EntityFrameworkCore;
using SugahriStore.Datos;
using SugahriStore.Modelos;
using SugahriStore.Lógica;
using SugahriStore.Lógica.DatosCSV;

namespace SugahriStore;

public partial class LoginView : ContentPage
{
    readonly Login login = new();
    readonly Usuario usuario = new();
    UsuariosRepositorio UsuariosRepositorio = new();
    public LoginView()
    {
        InitializeComponent();
    }

    public async void Login(object sender, EventArgs e)
    {
        usuario.Nombre = NombreUsuario.Text;
        bool existe = UsuariosRepositorio.UsuarioRegistrado(usuario.Nombre);
        if (existe)
        {
            Usuario usuarioRegistrado = UsuariosRepositorio.ObtenerUsuarioPorNombre(usuario.Nombre);
            if (login.LoginUser(NombreUsuario.Text, ContraseñaUsuario.Text, usuarioRegistrado))
            {
                await Navigation.PushAsync(new MainPage(usuarioRegistrado));
            }
            else MensajeError.Text = "Contraseña Incorrecta";
        }
        else MensajeError.Text = "Este usuario no está registrado";
    }
    public async void Registro(object sender, EventArgs e)
    {
        RegistroView RegistroView = new RegistroView();
        await Navigation.PushAsync(RegistroView);
    }
}


