namespace Prototipo_1_SugahriStore.Modelos
{
    public class LineaPedido
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public Pedido Pedido { get; set; }
        public int PedidoId { get; set; }
    }
}