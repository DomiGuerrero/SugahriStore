using SugahriStore.Datos;
using SugahriStore.Modelos;

namespace SugahriStore;

public partial class ModificarUsuariosAdminView : ContentPage
{
    private UsuariosRepositorio UsuariosRepositorio = new();
    private Usuario UsuarioModificado;

    public ModificarUsuariosAdminView(Usuario Usuario)
    {
        UsuarioModificado = Usuario;
        InitializeComponent();

        // Establecer los valores predeterminados de los campos con los datos del usuario recibido
        NombreUsuario.Text = UsuarioModificado.Nombre;
        SwitchRol.IsToggled = UsuarioModificado.RolId == 1 ? true : false;
    }

    private void SwitchRol_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            textoRol.Text = "Admin";
        }
        else
        {
            textoRol.Text = "User";
        }
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
            // Obtener el rol seleccionado del Switch
            int rol = SwitchRol.IsToggled ? 1 : 2;

            // Actualizar los datos del usuario existente con los nuevos valores ingresados
            UsuarioModificado.Nombre = NombreUsuario.Text;
            UsuarioModificado.Contraseña = ContraseñaUsuario.Text;
            UsuarioModificado.RolId = rol;

            // Actualizar el usuario en la base de datos
            UsuariosRepositorio.ActualizarUsuario(UsuarioModificado,ContraseñaUsuario.Text);

            DisplayAlert("Actualización Correcta", "Se ha actualizado correctamente el usuario", "Aceptar");

            // Volver a la página anterior
            Navigation.PopAsync();
        }
    }
}
