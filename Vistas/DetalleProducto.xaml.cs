using SugahriStore.Modelos;

namespace SugahriStore.Vistas
{
    public partial class DetalleProducto : ContentPage
    {
        private readonly Producto _producto;
        public string Nombre => _producto.Nombre;
        public int Inventario => _producto.Inventario;
        public decimal Coste => _producto.Coste;

        public ProductosView ProductosView;

        public DetalleProducto(Producto producto, ProductosView productosView)
        {
            InitializeComponent();

            // Establecer la imagen del producto
            imagenProducto.Source = producto.ImageUrl;

            // Asignar la referencia de la vista principal de productos
            ProductosView = productosView;

            // Almacenar el producto actual
            _producto = producto;

            // Establecer el contexto de enlace de datos
            BindingContext = this;
        }

        // Manejar el evento de volver
        private async void Volver(object sender, EventArgs e)
        {
            await ProductosView.Navigation.PopAsync();
        }
    }

}
