using Microsoft.EntityFrameworkCore;
using SugahriStore.Datos;
using SugahriStore.Modelos;

namespace SugahriStore.Repositorios
{
    public class LineaPedidoRepositorio
    {
        private readonly DbContext _dbcontext;

        public LineaPedidoRepositorio()
        {
            _dbcontext = new BaseDeDatosContext();
        }

        // Agregar una nueva línea de pedido a la base de datos
        public void Agregar(LineaPedido lineaPedido)
        {
            _dbcontext.Set<LineaPedido>().Add(lineaPedido);
            _dbcontext.SaveChanges();
        }

        // Actualizar los datos de una línea de pedido en la base de datos
        public void Actualizar(LineaPedido lineaPedido)
        {
            _dbcontext.Set<LineaPedido>().Update(lineaPedido);
            _dbcontext.SaveChanges();
        }

        // Eliminar una línea de pedido de la base de datos
        public void Eliminar(int lineaPedidoId)
        {
            var lineaPedido = _dbcontext.Set<LineaPedido>().Find(lineaPedidoId);
            if (lineaPedido != null)
            {
                _dbcontext.Set<LineaPedido>().Remove(lineaPedido);
                _dbcontext.SaveChanges();
            }
        }

        // Buscar todas las líneas de pedido asociadas a un pedido específico
        public List<LineaPedido> BuscarLineasPedidoPorPedido(int pedidoId)
        {
            return _dbcontext.Set<LineaPedido>()
                .Where(lp => lp.PedidoId == pedidoId)
                .ToList();
        }
    }
}
