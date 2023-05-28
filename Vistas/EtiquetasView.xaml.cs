using SugahriStore.Datos;
using SugahriStore.Modelos;
using System.Collections.ObjectModel;

namespace SugahriStore.Vistas;

public partial class EtiquetasView : ContentPage
{
    private static PedidosRepositorio PedidosRepositorio = new();
    public MainPage MainPageView;
    private List<Pedido> _pedidos;
    private List<Pedido> _pedidosFiltrados;
    public List<Pedido> Pedidos
    {
        get => _pedidosFiltrados;
        set
        {
            _pedidosFiltrados = value;
            OnPropertyChanged(nameof(Pedidos));
        }
    }

    // Nueva propiedad para almacenar los pedidos seleccionados
    public ObservableCollection<Pedido> PedidosSeleccionados { get; set; }



    public EtiquetasView(MainPage mainPage)
    {
        InitializeComponent();
        _pedidos = PedidosRepositorio.ObtenerPedidos();
        _pedidosFiltrados = _pedidos;
        MainPageView = mainPage;



        /*for (int i = 0; i < Pedidos.Count; i++)
        {
            EtiquetaManager.CrearEtiqueta("C:\\databases", "etiqueta" + i, Pedidos[i]);
        }*/
    }
    private void Seleccionar(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var pedido = checkBox?.BindingContext as Pedido;

        if (pedido != null)
        {
            if (e.Value)
            {
                // Agregar el pedido a la lista de pedidos seleccionados
                PedidosSeleccionados.Add(pedido);
            }
            else
            {
                // Eliminar el pedido de la lista de pedidos seleccionados
                PedidosSeleccionados.Remove(pedido);
            }
        }
    }
    private void BorrarSeleccionados(object sender, EventArgs e)
    {
        // Obt�n una lista de los pedidos seleccionados que se van a borrar
        var pedidosABorrar = PedidosSeleccionados.ToList();
        if (pedidosABorrar.Count != 0)
        {

            // Borrar los pedidos de la base de datos
            foreach (var pedido in pedidosABorrar)
            {
                PedidosRepositorio.BorrarPedido(pedido);
            }

            // Borrar los pedidos de la lista local
            _pedidos.RemoveAll(p => pedidosABorrar.Contains(p));

            // Actualizar la lista de pedidos filtrados y la lista de pedidos seleccionados
            Pedidos = _pedidos.ToList();
            PedidosSeleccionados.Clear();
            DisplayAlert("�xito", "Pedidos borrados correctamente", "Aceptar");
        }
        else
        {
            DisplayAlert("Error", "No hay ning�n pedido seleccionado", "Aceptar");
        }

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
            await Navigation.PushAsync(new DetallePedido(MainPageView, pedido));
        }
    }

    public async Task VerDetallesCommand(Pedido pedido)
    {
        DetallePedido detallePage = new DetallePedido(MainPageView, pedido);
        await Navigation.PushAsync(detallePage);
    }

    private async void Regresar(object sender, EventArgs e)
    {
        await MainPageView.Navigation.PopAsync();
    }
}