using SugahriStore.Lógica.DatosCSV;
using SugahriStore.Modelos;
using SugahriStore.Vistas;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SugahriStore;

public partial class ProductosView : ContentPage
{
    public MainPage MainPageView;
    private List<Producto> _productos;
    private List<Producto> _productosFiltrados;
    public List<Producto> Productos
    {
        get => _productosFiltrados;
        set
        {
            _productosFiltrados = value;
            OnPropertyChanged(nameof(Productos));
        }
    }
    public ProductosView(MainPage mainPage)
    {
        InitializeComponent();
        _productos = CsvManagement.DeserializarProductos();
        _productosFiltrados = _productos;
        MainPageView = mainPage;
        BindingContext = this;
    }
    private void FiltrarPorNombreProducto(string filtro)
    {
        Productos = _productos.Where(p => p.Nombre.ToLower().Contains(filtro.ToLower())).ToList();

        if (!Productos.Any())
        {
            Productos = _productos;
        }
    }

    private void CambioDeTexto(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            Productos = _productos;
        }
        else
        {
            FiltrarPorNombreProducto(e.NewTextValue);
        }

    }
    private void Detalles_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var producto = button?.BindingContext as Producto;
        if (producto != null)
        {
            Navigation.PushAsync(new DetalleProducto(producto,this));
        }
    }
    private async void Regresar(object sender, EventArgs e)
    {
        await MainPageView.Navigation.PopAsync();
    }
}
