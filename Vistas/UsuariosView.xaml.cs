using SugahriStore.Datos;
using SugahriStore.Logica;
using SugahriStore.Modelos;

namespace SugahriStore;

public partial class UsuariosView : ContentPage
{
    PedidosRepositorio PedidosRepositorio = new PedidosRepositorio();
    public UsuariosView(Usuario usuario, MainPage mainPage)
    {
        InitializeComponent();
        NombreProeba.Text = usuario.Nombre;
        RolName.Text = usuario.Rol.Nombre;

    }
}