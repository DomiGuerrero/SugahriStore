using SugahriStore.Datos;
using SugahriStore.Modelos;
using SugahriStore.Repositorios;
using System.Collections.ObjectModel;

namespace SugahriStore
{
    public partial class DetallePedido : ContentPage
    {
        private readonly Pedido _pedido;

        public LineaPedidoRepositorio LineaPedidoRepositorio = new();

        private PedidosRepositorio PedidosRepositorio = new();

        private MainPage MainPage;
        public string NombrePedido => _pedido.Nombre;
        public string Estado => _pedido.Estado;
        public string Divisa => _pedido.Divisa;
        public decimal PrecioTotal => _pedido.Total;

        public ObservableCollection<LineaPedido> LineasPedido { get; set; } = new ObservableCollection<LineaPedido>();

        public DetallePedido(MainPage mainPage, Pedido pedido)
        {
            MainPage = mainPage;
            InitializeComponent();

            _pedido = pedido;
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


        private void GuardarPedidoCommand(object sender, EventArgs e)
        {
            // Obtener los nuevos valores de las entradas
            string nuevoNombrePedido = NombrePedidoEntry.Text;
            string nuevoEstado = EstadoEntry.Text;
            string nuevaDivisa = DivisaEntry.Text;
            decimal nuevoPrecioTotal = decimal.Parse(PrecioTotalEntry.Text);

            // Actualizar los campos del pedido
            _pedido.Nombre = nuevoNombrePedido;
            _pedido.Estado = nuevoEstado;
            _pedido.Divisa = nuevaDivisa;
            _pedido.Total = nuevoPrecioTotal;

            // Actualizar el pedido en la base de datos
            PedidosRepositorio repositorioPedidos = new PedidosRepositorio();
            repositorioPedidos.ActualizarPedido(_pedido);

            DisplayAlert("Éxito", "Pedido Actualizado Correctamente", "Aceptar");
        }



        private async void Volver(object sender, EventArgs e)
        {
            PedidosView pedidos = new PedidosView(MainPage);
            await Navigation.PushAsync(pedidos);
        }
    }
}
