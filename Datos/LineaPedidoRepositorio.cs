using System.Collections.Generic;
using System.Linq;
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

        public void Agregar(LineaPedido lineaPedido)
        {
            _dbcontext.Set<LineaPedido>().Add(lineaPedido);
            _dbcontext.SaveChanges();
        }

        public void Actualizar(LineaPedido lineaPedido)
        {
            _dbcontext.Set<LineaPedido>().Update(lineaPedido);
            _dbcontext.SaveChanges();
        }

        public void Eliminar(int lineaPedidoId)
        {
            var lineaPedido = _dbcontext.Set<LineaPedido>().Find(lineaPedidoId);
            if (lineaPedido != null)
            {
                _dbcontext.Set<LineaPedido>().Remove(lineaPedido);
                _dbcontext.SaveChanges();
            }
        }
        public List<LineaPedido> BuscarLineasPedidoPorPedido(int pedidoId)
        {
            return _dbcontext.Set<LineaPedido>()
                .Where(lp => lp.PedidoId == pedidoId)
                .ToList();
        }
    }
}

