using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SugahriStore.Datos;
using SugahriStore.Modelos;
using SugahriStore.Repositorios;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SugahriStore
{
    public partial class DetallePedido : ContentPage
    {
        private readonly Pedido _pedido;

        public LineaPedidoRepositorio LineaPedidoRepositorio = new();

        private PedidosRepositorio PedidosRepositorio = new();

        private MainPage MainPage;

        // Propiedades del pedido
        public string NombrePedido => _pedido.Nombre;
        public string Estado => _pedido.Estado;
        public string Divisa => _pedido.Divisa;
        public decimal PrecioTotal => _pedido.Total;

        // Colección observable de líneas de pedido
        public ObservableCollection<LineaPedido> LineasPedido { get; set; } = new ObservableCollection<LineaPedido>();

        // Constructor de la clase
        public DetallePedido(MainPage mainPage, Pedido pedido)
        {
            MainPage = mainPage;
            InitializeComponent();

            _pedido = pedido;

            // Obtener las líneas de pedido del repositorio y asignarlas al pedido actual
            _pedido.LineasPedido = this.LineaPedidoRepositorio.BuscarLineasPedidoPorPedido(pedido.Id);

            BindingContext = this;

            // Agregar las líneas de pedido al ObservableCollection
            if (_pedido.LineasPedido != null)
            {
                foreach (var linea in _pedido.LineasPedido)
                {
                    LineasPedido.Add(linea);
                }
            }

            // Verificar si hay líneas de pedido y ajustar la visibilidad de la etiqueta y la lista
            if (LineasPedido.Count > 0)
            {
                LineasPedidoListView.IsVisible = true;
                NoLineasPedidoLabel.IsVisible = false;
            }
            else
            {
                LineasPedidoListView.IsVisible = false;
                NoLineasPedidoLabel.IsVisible = true;
            }
        }

        // Método para guardar los cambios realizados en el pedido
        private void GuardarPedidoCommand(object sender, EventArgs e)
        {
            // Obtener los nuevos valores de las entradas
            string nuevoNombrePedido = NombrePedidoEntry.Text;
            string nuevoEstado = EstadoEntry.Text;
            string nuevaDivisa = DivisaEntry.Text;
            decimal nuevoPrecioTotal;

            // Verificar que los campos no estén vacíos y que el precio total sea un decimal válido
            if (string.IsNullOrWhiteSpace(nuevoNombrePedido) ||
                string.IsNullOrWhiteSpace(nuevoEstado) ||
                string.IsNullOrWhiteSpace(nuevaDivisa) ||
                !decimal.TryParse(PrecioTotalEntry.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out nuevoPrecioTotal))
            {
                DisplayAlert("Error", "Por favor, complete todos los campos correctamente.", "Aceptar");
                return;
            }

            // Verificar que el nombre del pedido comience con '#'
            if (!nuevoNombrePedido.StartsWith("#"))
            {
                DisplayAlert("Error", "El nombre del pedido debe comenzar con '#'.", "Aceptar");
                return;
            }

            // Actualizar los campos del pedido con los nuevos valores
            _pedido.Nombre = nuevoNombrePedido;
            _pedido.Estado = nuevoEstado;
            _pedido.Divisa = nuevaDivisa;
            _pedido.Total = nuevoPrecioTotal;

            // Actualizar el pedido en la base de datos
            PedidosRepositorio repositorioPedidos = new PedidosRepositorio();
            repositorioPedidos.ActualizarPedido(_pedido);

            DisplayAlert("Éxito", "Pedido Actualizado Correctamente", "Aceptar");
        }

        // Método para volver a la vista de Pedidos
        private async void Volver(object sender, EventArgs e)
        {
            // Crear una instancia de la vista de Pedidos y volver a ella
            PedidosView pedidos = new PedidosView(MainPage);
            await Navigation.PushAsync(pedidos);
        }
    }
}

