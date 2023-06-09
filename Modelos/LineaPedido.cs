namespace SugahriStore.Modelos
{
    public class LineaPedido
    {
        public int Id { get; set; }  // Identificador de la línea de pedido
        public string Nombre { get; set; }  // Nombre de la línea de pedido
        public decimal Precio { get; set; }  // Precio de la línea de pedido
        public int Cantidad { get; set; }  // Cantidad de productos en la línea de pedido
        public Pedido Pedido { get; set; }  // Pedido al que pertenece esta línea de pedido
        public int PedidoId { get; set; }  // Identificador del pedido al que pertenece esta línea de pedido
    }

}