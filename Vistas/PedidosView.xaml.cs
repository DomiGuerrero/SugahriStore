using SugahriStore.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using SugahriStore.Lógica.DatosCSV;

namespace SugahriStore
{
    public partial class PedidosView : ContentPage
    {
        public MainPage MainPageView;
        private List<Pedido> _pedidos;
        private List<Pedido> _pedidosFiltrados;
        DetallePedido detallePage;
        public List<Pedido> Pedidos
        {
            get => _pedidosFiltrados;
            set
            {
                _pedidosFiltrados = value;
                OnPropertyChanged(nameof(Pedidos));
            }
        }
        public PedidosView(Usuario usuario, MainPage mainPage )
        {
            InitializeComponent();
            _pedidos = CsvManagement.DeserializarPedidos();
            _pedidosFiltrados = _pedidos;
            MainPageView = mainPage;
            BindingContext = this;
        }
        private void FiltrarPorNombrePedido(string filtro)
        {
            Pedidos = _pedidos.Where(p => p.Nombre.ToLower().Contains(filtro.ToLower())).ToList();

            if (!Pedidos.Any())
            {
                Pedidos = _pedidos;
            }
        }

        private void CambioDeTexto(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                Pedidos = _pedidos;
            }
            else
            {
                FiltrarPorNombrePedido(e.NewTextValue);
            }

        }
        private async void DetallePedido(object sender, EventArgs e)
        {
            var button = sender as Button;
            var pedido = button?.BindingContext as Pedido;
            if (pedido != null)
            {
                await Navigation.PushAsync(new DetallePedido(pedido));
            }
        }
        public async Task VerDetallesCommand(Pedido pedido)
        {
            DetallePedido detallePage = new DetallePedido(pedido);
            await Navigation.PushAsync(detallePage);
        }

        private async void Regresar(object sender, EventArgs e)
        {
            await MainPageView.Navigation.PopAsync();
        }
    }
}
