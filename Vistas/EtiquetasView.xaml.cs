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
        try
        {
            var folder = await FolderPicker.PickAsync(default);

            if (folder != null && folder.Folder != null) // Verifica si se seleccionó una carpeta y si tiene una ruta válida
            {
                string direccion = $"{folder.Folder.Path}";

                // Obtén una lista de los pedidos seleccionados que se van a borrar
                var pedidosAExportar = PedidosSeleccionados.ToList();
                if (pedidosAExportar.Count != 0)
                {
                    bool confirmacion = await DisplayAlert("Confirmación", $"¿Estás seguro de que deseas exportar {pedidosAExportar.Count} etiquetas?\n\nEsto puede tardar aproximadamente {pedidosAExportar.Count * 1.2857} segundos.", "Sí", "No");
                    if (confirmacion)
                    {
                        progressBar.IsVisible = true;

                        await Task.Run(async () =>
                        {
                            for (int i = 0; i < PedidosSeleccionados.Count; i++)
                            {
                                await EtiquetaManager.CrearEtiqueta(direccion, Pedidos[i]);

                                // Actualiza el progreso del ProgressBar en el hilo de trabajo
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    progressBar.Progress = (double)(i + 1) / PedidosSeleccionados.Count;
                                });
                            }
                        });

                        progressBar.IsVisible = false;

                        await DisplayAlert("Éxito", "Etiquetas exportadas correctamente", "Aceptar");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No hay ningún pedido seleccionado", "Aceptar");
                }
            }
        }
        catch (Exception ex)
        {
            // Maneja la excepción y muestra un mensaje adecuado al usuario
            await DisplayAlert("Error", "Ocurrió un error al seleccionar la carpeta", "Aceptar");
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