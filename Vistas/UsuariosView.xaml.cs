using SugahriStore.Modelos;

namespace SugahriStore;

public partial class UsuariosView : ContentPage
{
	public UsuariosView(Usuario usuario, MainPage mainPage)
	{
		InitializeComponent();
		NombreProeba.Text = usuario.Nombre;
		RolName.Text = usuario.Rol.Nombre;
	}
}