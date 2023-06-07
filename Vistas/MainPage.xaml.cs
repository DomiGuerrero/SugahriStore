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
    private PedidosView pedidosView;
    private ProductosView ProductosView;
    private UsuariosView UsuariosView;
    private ImportView ImportView;
    private EtiquetasView EtiquetasView;
    private LoginView LoginView;

    private RolesRepositorio RolesRepositorio = new();
    private Usuario Usuario { get; set; }

    private static string img1 = Path.Combine(Path.Combine(AppContext.BaseDirectory, "Resources", "ImagesView\\image1.jpeg"));
    private static string img2 = Path.Combine(Path.Combine(AppContext.BaseDirectory, "Resources", "ImagesView\\image2.jpeg"));
    private static string img3 = Path.Combine(Path.Combine(AppContext.BaseDirectory, "Resources", "ImagesView\\image3.jpeg"));
    private static string img4 = Path.Combine(Path.Combine(AppContext.BaseDirectory, "Resources", "ImagesView\\image4.jpeg"));
    private static string img5 = Path.Combine(Path.Combine(AppContext.BaseDirectory, "Resources", "ImagesView\\image5.jpeg"));

    private List<string> _images = new List<string>
        {
            img1,
            img2,
            img3,
            img4,
            img5
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
        NombreProeba.Text = "Nombre: " + usuario.Nombre;
        usuario.Rol = RolesRepositorio.ObtenerRolPorId(usuario.RolId);
        RolName.Text = "Rol: " + usuario.Rol.Nombre;
        try
        {
            _ = LogicaViews.StartTimer(carouselView, Images);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error en StartTimer: " + ex.Message);
        }
    }

    private void Button1_Clicked(object sender, EventArgs e)
    {
        pedidosView = new PedidosView(this);
        Navigation.PushAsync(pedidosView);
    }
    private void Button2_Clicked(object sender, EventArgs e)
    {
        ImportView = new ImportView();
        Navigation.PushAsync(ImportView);
    }
    private void Button3_Clicked(object sender, EventArgs e)
    {
        EtiquetasView = new EtiquetasView(this);
        Navigation.PushAsync(EtiquetasView);
    }
    private void Button4_Clicked(object sender, EventArgs e)
    {
        if (Usuario.Rol.Nombre.Equals("ADMIN")){
        UsuariosView = new UsuariosView(this.Usuario, this);
        Navigation.PushAsync(UsuariosView);
        }
        else DisplayAlert("Acceso Restringido", "No puede acceder a esta funcionalidad sin permisos de administrador", "Aceptar");


    }
    private void Button5_Clicked(object sender, EventArgs e)
    {
        ProductosView = new ProductosView(this);
        Navigation.PushAsync(ProductosView);
    }
    private async void Button7_Clicked(object sender, EventArgs e)
    {
        bool respuesta = await DisplayAlert("Confirmaci�n", "�Desea cerrar la sesi�n?", "S�", "No");

        if (respuesta)
        {
            LoginView = new LoginView();
            await Navigation.PushAsync(LoginView);
        }
    }

    private async  void Button6_Clicked(object sender, EventArgs e)
    {
        bool respuesta = await DisplayAlert("Confirmaci�n", "�Desea cerrar la aplicaci�n?", "S�", "No");

        if (respuesta)
        {
            App.Current.Quit();
        }
    }

    private void PreviousButton_Clicked(object sender, EventArgs e)
    {
        if (carouselView.Position == 0)
        {
            carouselView.Position = Images.Count - 1;
        }
        else
        {
            carouselView.Position -= 1;
        }
    }
    private void NextButton_Clicked(object sender, EventArgs e)
    {
        if (carouselView.Position == Images.Count - 1)
        {
            carouselView.Position = 0;
        }
        else
        {
            carouselView.Position += 1;
        }
    }


}