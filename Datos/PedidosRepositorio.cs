using SugahriStore.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugahriStore.Datos
{
    public class PedidosRepositorio
    {
        private readonly BaseDeDatosContext _dbContext;

        public PedidosRepositorio()
        {
            _dbContext = new BaseDeDatosContext();
        }

        public void InsertarPedido(Pedido pedido)
        {
            _dbContext.Pedidos.Add(pedido);
            _dbContext.SaveChanges();
        }
        public void InsertarPedidos(List<Pedido> pedidos)
        {
            _dbContext.Pedidos.AddRange(pedidos);
            _dbContext.SaveChanges();
        }

        public Pedido ObtenerPedidoPorId(int id)
        {
            return _dbContext.Pedidos.FirstOrDefault(p => p.Id == id);
        }

        public List<Pedido> ObtenerPedidos()
        {
            return _dbContext.Pedidos.ToList();
        }

        public void ActualizarPedido(Pedido pedidoActualizado)
        {
            var pedidoExistente = _dbContext.Pedidos.FirstOrDefault(p => p.Id == pedidoActualizado.Id);
            if (pedidoExistente != null)
            {
                pedidoExistente.Nombre = pedidoActualizado.Nombre;
                pedidoExistente.Estado = pedidoActualizado.Estado;
                pedidoExistente.Divisa = pedidoActualizado.Divisa;
                pedidoExistente.Total = pedidoActualizado.Total;

                _dbContext.SaveChanges();
            }
        }

        public void BorrarPedidoPorId(int id)
        {
            Pedido pedido = _dbContext.Pedidos.FirstOrDefault(p => p.Id == id);
            if (pedido != null)
            {
                _dbContext.Pedidos.Remove(pedido);
                _dbContext.SaveChanges();
            }
        }
        public void BorrarPedido(Pedido pedido)
        {
            _dbContext.Pedidos.Remove(pedido);
            _dbContext.SaveChanges();
        }

    }
}

