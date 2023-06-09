using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugahriStore.Modelos
{
    public class Pedido
    {
        public int Id { get; set; } // Identificador del pedido
        public string Nombre { get; set; } // Nombre del pedido
        public string Email { get; set; } // Correo electrónico asociado al pedido
        public string Estado { get; set; } // Estado financiero del pedido
        public string EstadoDeEnvio { get; set; } // Estado de envío del pedido
        public string Divisa { get; set; } // Divisa utilizada en el pedido
        public decimal Total { get; set; } // Total del pedido
        public int AuditoriaId { get; set; } // Identificador de la auditoría asociada al pedido
        public Auditoria Auditoria { get; set; } // Referencia a la auditoría asociada al pedido
        public Cliente Cliente { get; set; } // Cliente asociado al pedido
        public int ClienteId { get; set; } // Identificador del cliente asociado al pedido
        public List<LineaPedido> LineasPedido { get; set; } // Lista de líneas de pedido asociadas al pedido
    }

}
