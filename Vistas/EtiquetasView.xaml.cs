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
            // Solicita al usuario que seleccione una carpeta para exportar los datos
            var folder = await FolderPicker.PickAsync(default);

            // Verifica si se seleccion� una carpeta y si tiene una ruta v�lida
            if (folder != null && folder.Folder != null)
            {
                // Obtiene la ruta de la carpeta seleccionada
                string direccion = $"{folder.Folder.Path}";

                // Obt�n una lista de los pedidos seleccionados que se van a exportar
                var pedidosAExportar = PedidosSeleccionados.ToList();

                // Verifica si existen pedidos seleccionados para exportar
                if (pedidosAExportar.Count != 0)
                {
                    // Solicita confirmaci�n al usuario antes de realizar la exportaci�n
                    bool confirmacion = await DisplayAlert("Confirmaci�n", $"�Est�s seguro de que deseas exportar {pedidosAExportar.Count} etiquetas?\n\nEsto puede tardar aproximadamente {pedidosAExportar.Count * 1.2857} segundos.", "S�", "No");

                    // Verifica si el usuario confirm� la exportaci�n
                    if (confirmacion)
                    {
                        // Muestra la barra de progreso
                        progressBar.IsVisible = true;

                        // Realiza la exportaci�n de etiquetas en segundo plano
                        await Task.Run(async () =>
                        {
                            for (int i = 0; i < PedidosSeleccionados.Count; i++)
                            {
                                // Crea la etiqueta para el pedido actual
                                await EtiquetaManager.CrearEtiqueta(direccion, Pedidos[i]);

                                // Actualiza el progreso del ProgressBar en el hilo principal de la interfaz de usuario
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    progressBar.Progress = (double)(i + 1) / PedidosSeleccionados.Count;
                                });
                            }
                        });

                        // Oculta la barra de progreso
                        progressBar.IsVisible = false;

                        // Muestra un mensaje de �xito al usuario
                        await DisplayAlert("�xito", "Etiquetas exportadas correctamente", "Aceptar");
                    }
                }
                else
                {
                    // Muestra un mensaje de error al usuario si no hay pedidos seleccionados para exportar
                    await DisplayAlert("Error", "No hay ning�n pedido seleccionado", "Aceptar");
                }
            }
        }
        catch (Exception ex)
        {
            // Maneja la excepci�n y muestra un mensaje de error al usuario
            await DisplayAlert("Error", "Ocurri� un error al seleccionar la carpeta", "Aceptar");
        }
    }

    private void FiltrarPorNombrePedido(string filtro)
    {
        // Filtra los pedidos por el nombre que contenga el filtro proporcionado, sin distinguir may�sculas y min�sculas
        Pedidos = _pedidos.Where(p => p.Nombre.ToLower().Contains(filtro.ToLower())).ToList();

        // Si no se encuentra ning�n pedido que cumpla el filtro, se restaura la lista original de pedidos
        if (!Pedidos.Any())
        {
            Pedidos = _pedidos;
        }
    }

    private void CambioDeTexto(object sender, TextChangedEventArgs e)
    {
        // Verifica si el texto proporcionado est� vac�o o solo contiene espacios en blanco
        if (string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            // Si el texto est� vac�o, se restaura la lista original de pedidos
            Pedidos = _pedidos;
        }
        else
        {
            // Si hay texto, se filtra los pedidos por el nombre que contenga el texto
            FiltrarPorNombrePedido(e.NewTextValue);
        }
    }

    private async void Regresar(object sender, EventArgs e)
    {
        // Realiza la acci�n de navegaci�n para regresar a la p�gina anterior de manera as�ncrona
        await MainPageView.Navigation.PopAsync();
    }

}