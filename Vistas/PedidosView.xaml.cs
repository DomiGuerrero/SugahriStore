using SugahriStore.Modelos;
using System.Collections.ObjectModel;
using SugahriStore.Datos;
using SugahriStore.Logica;

namespace SugahriStore
{
    public partial class PedidosView : ContentPage
    {
        public MainPage MainPageView;
        private List<Pedido> _pedidos;
        private List<Pedido> _pedidosFiltrados;
        PedidosRepositorio PedidosRepositorio = new PedidosRepositorio();
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

        public PedidosView(MainPage mainPage)
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

        private async void BorrarSeleccionados(object sender, EventArgs e)
        {
            if (MainPageView.Usuario.Rol.Nombre.Equals("ADMIN"))
            {
                    // Obtén una lista de los pedidos seleccionados que se van a borrar
                    var pedidosABorrar = PedidosSeleccionados.ToList();
                if (pedidosABorrar.Count != 0)
                {
                    bool confirmacion = await DisplayAlert("Confirmación", "¿Estás seguro de que deseas borrar los pedidos seleccionados?", "Sí", "No");
                    if (confirmacion)
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
                        await DisplayAlert("Éxito", "Pedidos borrados correctamente", "Aceptar");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No hay ningún pedido seleccionado", "Aceptar");
                }
            }
            else
            {
                // Mostrar mensaje de acceso restringido si el usuario no es administrador
                DisplayAlert("Acceso Restringido", "No puede acceder a esta funcionalidad sin permisos de administrador", "Aceptar");
            }
        }

        private void FiltrarPorNombrePedido(string filtro)
        {
            // Filtrar la lista de pedidos por el nombre del pedido
            Pedidos = _pedidos.Where(p => p.Nombre.ToLower().Contains(filtro.ToLower())).ToList();

            // Si no hay pedidos que coincidan con el filtro, mostrar todos los pedidos
            if (!Pedidos.Any())
            {
                Pedidos = _pedidos;
            }
        }

        private void CambioDeTexto(object sender, TextChangedEventArgs e)
        {
            // Verificar si el nuevo texto está en blanco o nulo
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                // Si el texto está vacío, mostrar todos los pedidos
                Pedidos = _pedidos;
            }
            else
            {
                // Filtrar los pedidos por el nuevo texto ingresado
                FiltrarPorNombrePedido(e.NewTextValue);
            }
        }

        private async void DetallePedido(object sender, EventArgs e)
        {
            if (MainPageView.Usuario.Rol.Nombre.Equals("ADMIN")) { 
                // Obtener el botón que generó el evento
                var button = sender as Button;

                // Obtener el pedido asociado al botón
                var pedido = button?.BindingContext as Pedido;

                // Verificar si se obtuvo un pedido válido
                if (pedido != null)
                {
                    // Navegar a la página de detalle del pedido, pasando el pedido como parámetro
                    await Navigation.PushAsync(new DetallePedido(MainPageView, pedido));
                }
            }
            else
            {
                // Mostrar mensaje de acceso restringido si el usuario no es administrador
                DisplayAlert("Acceso Restringido", "No puede acceder a esta funcionalidad sin permisos de administrador", "Aceptar");
            }
        }

        public async Task VerDetallesCommand(Pedido pedido)
        {
            // Crear una instancia de la página de detalle del pedido, pasando el pedido y la vista principal como parámetros
            DetallePedido detallePage = new DetallePedido(MainPageView, pedido);

            // Navegar a la página de detalle del pedido
            await Navigation.PushAsync(detallePage);
        }

        private async void Regresar(object sender, EventArgs e)
        {
            // Regresar a la página anterior
            await MainPageView.Navigation.PopAsync();
        }

    }
}
