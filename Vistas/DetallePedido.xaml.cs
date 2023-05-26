using SugahriStore.Modelos;
using SugahriStore.Repositorios;
using System.Collections.ObjectModel;

namespace SugahriStore
{
    public partial class DetallePedido : ContentPage
    {
        private readonly Pedido _pedido;

        public LineaPedidoRepositorio LineaPedidoRepositorio = new();
        public string NombrePedido => _pedido.Nombre;
        public string Estado => _pedido.Estado;
        public string Divisa => _pedido.Divisa;
        public decimal PrecioTotal => _pedido.Total;

        public ObservableCollection<LineaPedido> LineasPedido { get; set; } = new ObservableCollection<LineaPedido>();

        public DetallePedido(Pedido pedido)
        {
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
            // Código para guardar el pedido
        }

        private void VerPedidoCommand(object sender, EventArgs e)
        {
            // Código para ver los detalles del pedido
        }
    }
}
