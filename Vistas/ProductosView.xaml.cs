using SugahriStore.Datos;
using SugahriStore.Lógica.DatosCSV;
using SugahriStore.Modelos;
using SugahriStore.Vistas;


namespace SugahriStore;

public partial class ProductosView : ContentPage
{
    public MainPage MainPageView;
    private List<Producto> _productos;
    private List<Producto> _productosFiltrados;
    ProductosRepositorio ProductosRepositorio = new();
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
        _productos = ProductosRepositorio.ObtenerTodos();
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
            Navigation.PushAsync(new DetalleProducto(producto, this));
        }
    }
    private async void Regresar(object sender, EventArgs e)
    {
        await MainPageView.Navigation.PopAsync();
    }
    private async void Insertar(object sender, EventArgs e)
    {
        var fileResult = await FilePicker.PickAsync();
        string filePath = "";
        if (fileResult != null)
        {
            // Obtener la ruta del archivo seleccionado
            filePath = fileResult.FullPath;

            // Verificar la extensión del archivo seleccionado
            string fileExtension = Path.GetExtension(filePath);
            if (fileExtension != ".csv")
            {
                // Mostrar un mensaje de error si la extensión no es .csv
                await DisplayAlert("Error", "Seleccione un archivo con extensión .csv", "Aceptar");
                return;
            }
        }
        // Validar si se ha seleccionado un archivo para importar
        if (!string.IsNullOrEmpty(filePath))
        {
            // Obtener la ruta del archivo importado
            List<Producto> productos = CsvManagement.DeserializarProductos(filePath);

            if (productos != null && productos.Count > 0)
            {
                this.ProductosRepositorio.AgregarProductos(productos);
                _productos = ProductosRepositorio.ObtenerTodos(); // Actualizar la lista _productos
                _productosFiltrados = new List<Producto>(_productos); // Crear una nueva instancia de lista
                OnPropertyChanged(nameof(Productos)); // Notificar el cambio a la interfaz

                // Mostrar un mensaje de éxito
                await DisplayAlert("Éxito", "Archivo importado correctamente", "Aceptar");
            }
            else
            {
                // Mostrar un mensaje de error si la lista de pedidos está vacía
                await DisplayAlert("Error", "El archivo es incorrecto o está vacío", "Aceptar");
            }
        }
        else
        {
            // Mostrar un mensaje de error si no se ha seleccionado un archivo para importar
            await DisplayAlert("Error", "Por favor, selecciona un archivo para importar", "Aceptar");
        }
    }

}
