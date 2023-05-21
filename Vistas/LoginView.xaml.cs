using Microsoft.EntityFrameworkCore;
using SugahriStore.ManejoDatos;
using SugahriStore.Modelos;
using SugahriStore.Lógica;
using SugahriStore.Lógica.DatosCSV;

namespace SugahriStore;

public partial class LoginView : ContentPage
{
    readonly Login login = new();
    readonly List<Usuario> usuarios;

    public LoginView()
    {
        InitializeComponent();
        usuarios = CsvManagement.DeserializarUsuarios();
        _ = new BaseDeDatosContext();
    }

    public async void Login(object sender, EventArgs e)
    {
        bool okLogin = false;
        for (int i = 0; i < usuarios.Count && !okLogin; i++)
        {
            if (login.LoginUser(NombreUsuario.Text, ContraseñaUsuario.Text, usuarios[i]))
            {
                await Navigation.PushAsync(new MainPage(usuarios[i]));
                okLogin = true;
            }
            else MensajeError.Text = " Inicio de sesión incorrecto";
        }
       
    }
}


