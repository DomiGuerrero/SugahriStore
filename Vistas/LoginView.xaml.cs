using Microsoft.EntityFrameworkCore;
using SugahriStore.Datos;
using SugahriStore.Modelos;
using SugahriStore.Lógica;
using SugahriStore.Lógica.DatosCSV;

namespace SugahriStore;

public partial class LoginView : ContentPage
{
    readonly Login login = new();
    readonly List<Usuario> usuarios;
    UsuariosRepositorio UsuariosRepositorio  = new();
    RolesRepositorio RolesRepositorio = new();

    public LoginView()
    {
        InitializeComponent();
        usuarios = UsuariosRepositorio.ObtenerUsuarios();
    }

    public async void Login(object sender, EventArgs e)
    {
        bool okLogin = false;
        for (int i = 0; i < usuarios.Count && !okLogin; i++)
        {
            if (login.LoginUser(NombreUsuario.Text, ContraseñaUsuario.Text, usuarios[i]))
            {
                Usuario usuarioRegistrado = usuarios[i];
                Rol rolUsuario = RolesRepositorio.ObtenerRolPorId(usuarios[i].RolId);
                if (rolUsuario != null)
                {
                    usuarioRegistrado.Rol = rolUsuario;
                    usuarioRegistrado.Rol.Nombre = rolUsuario.Nombre;
                }
                await Navigation.PushAsync(new MainPage(usuarioRegistrado));
                okLogin = true;
            }
            else MensajeError.Text = " Inicio de sesión incorrecto";
        }
       
    }
}


