using SugahriStore.Modelos;
using Microsoft.Maui.Controls.Compatibility;
using System.Collections.Generic;
using System.Linq;
using SugahriStore.Datos;
using SugahriStore.L�gica.DatosCSV;
using SugahriStore.Vistas;
using Microsoft.Maui.Controls;
using SugahriStore.L�gica;

namespace SugahriStore;

public partial class MainPage : ContentPage
{
    private PedidosView pedidosView; // Vista de pedidos
    private ProductosView ProductosView; // Vista de productos
    private UsuariosView UsuariosView; // Vista de usuarios
    private ImportView ImportView; // Vista de importaci�n
    private EtiquetasView EtiquetasView; // Vista de etiquetas
    private LoginView LoginView; // Vista de inicio de sesi�n

    private RolesRepositorio RolesRepositorio = new(); // Repositorio de roles
    public Usuario Usuario { get; set; } // Usuario actual

    private static string imagesDirectory = Path.Combine(AppContext.BaseDirectory, "Resources", "ImagesView"); // Directorio de im�genes
    private List<string> _images = new List<string>
{
    Path.Combine(imagesDirectory, "image1.jpeg"),
    Path.Combine(imagesDirectory, "image2.jpeg"), 
    Path.Combine(imagesDirectory, "image3.jpeg"), 
    Path.Combine(imagesDirectory, "image4.jpeg"), 
    Path.Combine(imagesDirectory, "image5.jpeg") 
};

    public List<string> Images
    {
        get { return _images; }
        set { _images = value; }
    }

    public MainPage(Usuario usuario)
    {
        InitializeComponent();
        BindingContext = this;
        Usuario = usuario;
        NombreProeba.Text = "Nombre: " + usuario.Nombre; // Mostrar el nombre del usuario
        usuario.Rol = RolesRepositorio.ObtenerRolPorId(usuario.RolId); // Obtener el rol del usuario mediante el Repositorio de roles
        RolName.Text = "Rol: " + usuario.Rol.Nombre; // Mostrar el nombre del rol del usuario

        try
        {
            _ = LogicaViews.StartTimer(carouselView, Images); // Iniciar el temporizador para el carrusel de im�genes
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en StartTimer: " + ex.Message); // Manejo de errores en caso de excepci�n
        }
    }


    private void Button1_Clicked(object sender, EventArgs e)
    {
        // Navegar a la vista de Pedidos
        pedidosView = new PedidosView(this);
        Navigation.PushAsync(pedidosView);
    }

    private void Button2_Clicked(object sender, EventArgs e)
    {
        if (Usuario.Rol.Nombre.Equals("ADMIN"))
        {
            // Navegar a la vista de Importaci�n
            ImportView = new ImportView();
            Navigation.PushAsync(ImportView);
        }
        else
        {
            // Mostrar mensaje de acceso restringido si el usuario no es administrador
            DisplayAlert("Acceso Restringido", "No puede acceder a esta funcionalidad sin permisos de administrador", "Aceptar");
        }
    }

    private void Button3_Clicked(object sender, EventArgs e)
    {
        // Verificar el rol del usuario y permitir acceso a UsuariosView solo si es administrador
        EtiquetasView = new EtiquetasView(this);
        Navigation.PushAsync(EtiquetasView);
    }

    private void Button4_Clicked(object sender, EventArgs e)
    {
        // Verificar el rol del usuario y permitir acceso a UsuariosView solo si es administrador
        if (Usuario.Rol.Nombre.Equals("ADMIN"))
        {
            UsuariosView = new UsuariosView(this.Usuario, this);
            Navigation.PushAsync(UsuariosView);
        }
        else
        {
            // Mostrar mensaje de acceso restringido si el usuario no es administrador
            DisplayAlert("Acceso Restringido", "No puede acceder a esta funcionalidad sin permisos de administrador", "Aceptar");
        }
    }

    private void Button5_Clicked(object sender, EventArgs e)
    {
        // Navegar a la vista de Productos
        ProductosView = new ProductosView(this);
        Navigation.PushAsync(ProductosView);
    }

    private async void Button7_Clicked(object sender, EventArgs e)
    {
        // Mostrar un mensaje de confirmaci�n para cerrar sesi�n
        bool respuesta = await DisplayAlert("Confirmaci�n", "�Desea cerrar la sesi�n?", "S�", "No");

        if (respuesta)
        {
            // Navegar a la vista de inicio de sesi�n si se confirma el cierre de sesi�n
            LoginView = new LoginView();
            await Navigation.PushAsync(LoginView);
        }
    }

    private async void Button6_Clicked(object sender, EventArgs e)
    {
        // Mostrar un mensaje de confirmaci�n para cerrar la aplicaci�n
        bool respuesta = await DisplayAlert("Confirmaci�n", "�Desea cerrar la aplicaci�n?", "S�", "No");

        if (respuesta)
        {
            // Cerrar la aplicaci�n si se confirma
            App.Current.Quit();
        }
    }


    // M�todo invocado cuando se presiona el bot�n anterior
    private void PreviousButton_Clicked(object sender, EventArgs e)
    {
        // Verificar si el �ndice actual es el primero en la lista de im�genes
        if (carouselView.Position == 0)
        {
            carouselView.Position = Images.Count - 1; // Establecer la posici�n al �ltimo �ndice
        }
        else
        {
            carouselView.Position -= 1; // Restar 1 a la posici�n actual
        }
    }

    // M�todo invocado cuando se presiona el bot�n siguiente
    private void NextButton_Clicked(object sender, EventArgs e)
    {
        // Verificar si el �ndice actual es el �ltimo en la lista de im�genes
        if (carouselView.Position == Images.Count - 1)
        {
            carouselView.Position = 0; // Establecer la posici�n al primer �ndice
        }
        else
        {
            carouselView.Position += 1; // Sumar 1 a la posici�n actual
        }
    }



}