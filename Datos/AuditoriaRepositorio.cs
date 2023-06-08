using Microsoft.EntityFrameworkCore;
using SugahriStore.Modelos;


namespace SugahriStore.Datos
{
    public class AuditoriaRepositorio
    {
        private readonly BaseDeDatosContext _context;

        public AuditoriaRepositorio()
        {
            _context = new BaseDeDatosContext();
        }

        // Agregar una nueva auditoría a la base de datos
        public void AgregarAuditoria(Auditoria auditoria)
        {
            _context.Auditorias.Add(auditoria);
            _context.SaveChanges();
        }

        // Actualizar una auditoría en la base de datos
        public void ActualizarAuditoria(Auditoria auditoria)
        {
            try
            {
                _context.Entry(auditoria).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _context.Entry(auditoria).Reload();
                _context.SaveChanges();
            }
        }

        // Eliminar una auditoría de la base de datos
        public void EliminarAuditoria(Auditoria auditoria)
        {
            _context.Auditorias.Remove(auditoria);
            _context.SaveChanges();
        }

        // Obtener una auditoría por su ID
        public Auditoria ObtenerAuditoriaPorId(int id)
        {
            return _context.Auditorias.Find(id);
        }

        // Obtener todas las auditorías de la base de datos
        public List<Auditoria> ObtenerTodasLasAuditorias()
        {
            return _context.Auditorias.ToList();
        }
    }
}

