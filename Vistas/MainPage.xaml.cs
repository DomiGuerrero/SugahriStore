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
    private PedidosView pedidosView;
    private ProductosView ProductosView;
    private UsuariosView UsuariosView;
    private ImportView ImportView;
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
    private int currentIndex = 0;


    public MainPage(Usuario usuario)
    {
        InitializeComponent();
        BindingContext = this;
        Usuario = usuario;
        NombreProeba.Text = "Nombre: " + usuario.Nombre;
        RolName.Text = "Rol: " + usuario.Rol.Nombre;
        LogicaViews.StartTimer(carouselView, Images);
    }

    private void Button1_Clicked(object sender, EventArgs e)
    {
        pedidosView = new PedidosView(this.Usuario, this);
        Navigation.PushAsync(pedidosView);
    }
    private void Button2_Clicked(object sender, EventArgs e)
    {
        ImportView = new ImportView();
        Navigation.PushAsync(ImportView);
    }
    private void Button3_Clicked(object sender, EventArgs e)
    {

    }
    private void Button4_Clicked(object sender, EventArgs e)
    {
        UsuariosView = new UsuariosView(this.Usuario, this);
        Navigation.PushAsync(UsuariosView);

    }
    private void Button5_Clicked(object sender, EventArgs e)
    {
        ProductosView = new ProductosView(this);
        Navigation.PushAsync(ProductosView);
    }
    private void Button6_Clicked(object sender, EventArgs e)
    {
        App.Current.Quit();
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