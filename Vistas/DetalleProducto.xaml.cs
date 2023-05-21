using SugahriStore.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugahriStore.Vistas
{
    public partial class DetalleProducto : ContentPage
    {
        private readonly Producto _producto;
        public string Nombre=> _producto.Nombre;
        public int Inventario => _producto.Inventario;
        public decimal Coste => _producto.Coste;

        public ProductosView ProductosView;

        public DetalleProducto(Producto producto,ProductosView productosView)
        {
            InitializeComponent();
            imagenProducto.Source = producto.ImageUrl;
            ProductosView = productosView;
            _producto = producto;
            BindingContext = this;
        }

        private async void Volver(object sender, EventArgs e)
        {
            await ProductosView.Navigation.PopAsync();
        }
    }
}
