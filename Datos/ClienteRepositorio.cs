using Microsoft.EntityFrameworkCore;
using SugahriStore.Modelos;

namespace SugahriStore.Datos
{
    public class ClienteRepositorio
    {
        private readonly BaseDeDatosContext _context;

        public ClienteRepositorio()
        {
            _context = new BaseDeDatosContext();
        }

        public List<Cliente> ObtenerTodos()
        {
            return _context.Clientes.ToList();
        }

        public Cliente ObtenerPorId(int id)
        {
            return _context.Clientes.Find(id);
        }

        public void Agregar(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        public void Actualizar(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Eliminar(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
        }

        public Cliente ObtenerClientePorPedido(Pedido pedido)
        {
            return _context.Clientes.FirstOrDefault(c => c.Id == pedido.ClienteId);
        }
    }
}
