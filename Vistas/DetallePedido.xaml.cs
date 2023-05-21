using SugahriStore.Modelos;

namespace SugahriStore;
public partial class DetallePedido : ContentPage
{
    private readonly Pedido _pedido;
    public string NombrePedido => _pedido.Nombre;
    public string Estado => _pedido.Estado;
    public string Divisa => _pedido.Divisa;
    public decimal PrecioTotal => _pedido.Total;

    public DetallePedido(Pedido pedido)
    {
        InitializeComponent();
        _pedido = pedido;
        BindingContext = this;
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

