using SugahriStore.Modelos;
using Microsoft.Maui.Controls.Compatibility;
using System.Collections.Generic;
using System.Linq;
using SugahriStore.Datos;
using SugahriStore.Lógica.DatosCSV;
using SugahriStore.Vistas;
using Microsoft.Maui.Controls;
using SugahriStore.Lógica;

namespace SugahriStore;

public partial class MainPage : ContentPage
{
    private PedidosView pedidosView; // Vista de pedidos
    private ProductosView ProductosView; // Vista de productos
    private UsuariosView UsuariosView; // Vista de usuarios
    private ImportView ImportView; // Vista de importación
    private EtiquetasView EtiquetasView; // Vista de etiquetas
    private LoginView LoginView; // Vista de inicio de sesión

    private RolesRepositorio RolesRepositorio = new(); // Repositorio de roles
    public Usuario Usuario { get; set; } // Usuario actual

    private static string imagesDirectory = Path.Combine(AppContext.BaseDirectory, "Resources", "ImagesView"); // Directorio de imágenes
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
            _ = LogicaViews.StartTimer(carouselView, Images); // Iniciar el temporizador para el carrusel de imágenes
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en StartTimer: " + ex.Message); // Manejo de errores en caso de excepción
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
            // Navegar a la vista de Importación
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
        // Mostrar un mensaje de confirmación para cerrar sesión
        bool respuesta = await DisplayAlert("Confirmación", "¿Desea cerrar la sesión?", "Sí", "No");

        if (respuesta)
        {
            // Navegar a la vista de inicio de sesión si se confirma el cierre de sesión
            LoginView = new LoginView();
            await Navigation.PushAsync(LoginView);
        }
    }

    private async void Button6_Clicked(object sender, EventArgs e)
    {
        // Mostrar un mensaje de confirmación para cerrar la aplicación
        bool respuesta = await DisplayAlert("Confirmación", "¿Desea cerrar la aplicación?", "Sí", "No");

        if (respuesta)
        {
            // Cerrar la aplicación si se confirma
            App.Current.Quit();
        }
    }


    // Método invocado cuando se presiona el botón anterior
    private void PreviousButton_Clicked(object sender, EventArgs e)
    {
        // Verificar si el índice actual es el primero en la lista de imágenes
        if (carouselView.Position == 0)
        {
            carouselView.Position = Images.Count - 1; // Establecer la posición al último índice
        }
        else
        {
            carouselView.Position -= 1; // Restar 1 a la posición actual
        }
    }

    // Método invocado cuando se presiona el botón siguiente
    private void NextButton_Clicked(object sender, EventArgs e)
    {
        // Verificar si el índice actual es el último en la lista de imágenes
        if (carouselView.Position == Images.Count - 1)
        {
            carouselView.Position = 0; // Establecer la posición al primer índice
        }
        else
        {
            carouselView.Position += 1; // Sumar 1 a la posición actual
        }
    }



}