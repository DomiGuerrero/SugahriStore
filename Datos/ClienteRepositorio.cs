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

        // Obtener todos los clientes de la base de datos
        public List<Cliente> ObtenerTodos()
        {
            return _context.Clientes.ToList();
        }

        // Obtener un cliente por su ID
        public Cliente ObtenerPorId(int id)
        {
            return _context.Clientes.Find(id);
        }

        // Agregar un nuevo cliente a la base de datos
        public void Agregar(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        // Actualizar los datos de un cliente en la base de datos
        public void Actualizar(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            _context.SaveChanges();
        }

        // Eliminar un cliente de la base de datos
        public void Eliminar(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
            _context.SaveChanges();
        }

        // Obtener el cliente asociado a un pedido específico
        public Cliente ObtenerClientePorPedido(Pedido pedido)
        {
            return _context.Clientes.FirstOrDefault(c => c.Id == pedido.ClienteId);
        }
    }
}
