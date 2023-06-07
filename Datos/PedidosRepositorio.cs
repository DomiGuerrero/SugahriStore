using Microsoft.EntityFrameworkCore;
using SugahriStore.Modelos;

namespace SugahriStore.Datos
{
    public class PedidosRepositorio
    {
        private readonly BaseDeDatosContext _dbContext;

        public PedidosRepositorio()
        {
            _dbContext = new BaseDeDatosContext();
        }

        // Insertar un nuevo pedido en la base de datos
        public void InsertarPedido(Pedido pedido)
        {
            _dbContext.Pedidos.Add(pedido);
            _dbContext.SaveChanges();
        }

        // Insertar una lista de pedidos en la base de datos
        public void InsertarPedidos(List<Pedido> pedidos)
        {
            foreach (var pedido in pedidos)
            {
                var pedidoExistente = _dbContext.Pedidos.Include(p => p.LineasPedido).FirstOrDefault(p => p.Nombre == pedido.Nombre);
                if (pedidoExistente != null)
                {
                    // Si ya existe un pedido con el mismo nombre, se actualizan las líneas de pedido
                    pedidoExistente.LineasPedido.Clear();
                    pedidoExistente.LineasPedido.AddRange(pedido.LineasPedido);
                }
                else
                {
                    // Si no existe un pedido con el mismo nombre, se agrega el nuevo pedido
                    _dbContext.Pedidos.Add(pedido);
                }
            }

            _dbContext.SaveChanges();
        }

        // Obtener un pedido por su ID
        public Pedido ObtenerPedidoPorId(int id)
        {
            return _dbContext.Pedidos.FirstOrDefault(p => p.Id == id);
        }

        // Obtener todos los pedidos
        public List<Pedido> ObtenerPedidos()
        {
            return _dbContext.Pedidos.ToList();
        }

        // Actualizar un pedido existente en la base de datos
        public void ActualizarPedido(Pedido pedidoActualizado)
        {
            var pedidoExistente = _dbContext.Pedidos.FirstOrDefault(p => p.Id == pedidoActualizado.Id);
            if (pedidoExistente != null)
            {
                // Se actualizan los datos del pedido existente con los nuevos datos
                pedidoExistente.Nombre = pedidoActualizado.Nombre;
                pedidoExistente.Estado = pedidoActualizado.Estado;
                pedidoExistente.Divisa = pedidoActualizado.Divisa;
                pedidoExistente.Total = pedidoActualizado.Total;

                _dbContext.SaveChanges();
            }
        }

        // Borrar un pedido por su ID
        public void BorrarPedidoPorId(int id)
        {
            Pedido pedido = _dbContext.Pedidos.FirstOrDefault(p => p.Id == id);
            if (pedido != null)
            {
                _dbContext.Pedidos.Remove(pedido);
                _dbContext.SaveChanges();
            }
        }

        // Borrar un pedido de la base de datos
        public void BorrarPedido(Pedido pedido)
        {
            _dbContext.Pedidos.Remove(pedido);
            _dbContext.SaveChanges();
        }
    }
}

