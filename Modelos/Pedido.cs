﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prototipo_1_SugahriStore.Modelos
{
    public class Pedido
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Estado { get; set; }
        public string EstadoDeEnvio { get; set; }
        public string Divisa { get; set; }
        public decimal Total { get; set; }
        public List<LineaPedido> LineasPedido { get; set; }
    }
}
