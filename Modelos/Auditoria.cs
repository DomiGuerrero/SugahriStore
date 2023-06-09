
namespace SugahriStore.Modelos
{
    public class Auditoria
    {
        public int Id { get; set; } // Identificador único de la auditoría
        public DateTime Fecha { get; set; } // Fecha en la que se realizó la auditoría
        public string NombrePedido { get; set; } // Nombre del pedido asociado a la auditoría
    }

}
