using CommunityToolkit.Maui.Storage;
using SugahriStore.Datos;
using SugahriStore.Logica;
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
        BindingContext = this;

        // Inicializar la lista de pedidos seleccionados
        PedidosSeleccionados = new ObservableCollection<Pedido>();
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
    private async void ExportarSeleccionados(object sender, EventArgs e)
    {
        var folder = await FolderPicker.PickAsync(default);

        string direccion = $"{folder.Folder.Path}";
        // Obtén una lista de los pedidos seleccionados que se van a borrar
        var pedidosABorrar = PedidosSeleccionados.ToList();
        if (pedidosABorrar.Count != 0)
        {

            for (int i = 0; i < PedidosSeleccionados.Count; i++)
            {
                EtiquetaManager.CrearEtiqueta(direccion, Pedidos[i]);
            }
            DisplayAlert("Éxito", "Etiquetas exportadas correctamente", "Aceptar");
        }
        else
        {
            DisplayAlert("Error", "No hay ningún pedido seleccionado", "Aceptar");
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

    private async void Regresar(object sender, EventArgs e)
    {
        await MainPageView.Navigation.PopAsync();
    }
}