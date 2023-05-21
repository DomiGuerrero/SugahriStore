using SugahriStore.Modelos;

namespace SugahriStore;
public partial class DetallePedido : ContentPage
{
    private readonly Pedido _pedido;
    public string NombrePedido => _pedido.Nombre;
    public string Estado => _pedido.Estado;
    public string Divisa => _pedido.Divisa;
    public decimal PrecioTotal => _pedido.Total;

    private ListView LineasPedidoListView;

    public DetallePedido(Pedido pedido)
    {
        InitializeComponent();
        _pedido = pedido;
        BindingContext = this;

        // Configurar el ListView con las líneas de pedido
        LineasPedidoListView = new ListView
        {
            ItemsSource = _pedido.LineasPedido,
            ItemTemplate = new DataTemplate(() =>
            {
                var nameLabel = new Label();
                nameLabel.SetBinding(Label.TextProperty, "Nombre");
                var priceLabel = new Label();
                priceLabel.SetBinding(Label.TextProperty, "Precio");
                var quantityLabel = new Label();
                quantityLabel.SetBinding(Label.TextProperty, "Cantidad");

                var viewCell = new ViewCell
                {
                    View = new StackLayout
                    {
                        Children = { nameLabel, priceLabel, quantityLabel }
                    }
                };

                return viewCell;
            })
        };

        // Crear el ScrollView para el ListView de líneas de pedido
        var scrollView = new ScrollView
        {
            Content = LineasPedidoListView
        };

        // Agregar el ScrollView al StackLayout principal
        //StackLayout.Children.Add(scrollView);
    }

    private void GuardarPedidoCommand(object sender, EventArgs e)
    {
       
    }

    private void VerPedidoCommand(object sender, EventArgs e)
    {
        
    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        
    }

    private void Regresar(object sender, EventArgs e)
    {
        
    }

    private void CambioDeTexto(object sender, EventArgs e)
    {
        // Código para actualizar el pedido cuando se cambia la cantidad de unidades del producto
    }
}

