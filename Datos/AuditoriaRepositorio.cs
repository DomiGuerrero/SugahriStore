using Microsoft.EntityFrameworkCore;
using SugahriStore.Modelos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugahriStore.Datos
{
    public class AuditoriaRepositorio
    {
        private readonly BaseDeDatosContext _context;

        public AuditoriaRepositorio()
        {
            _context = new BaseDeDatosContext();
        }

        public void AgregarAuditoria(Auditoria auditoria)
        {
            _context.Auditorias.Add(auditoria);
            _context.SaveChanges();
        }

        public void ActualizarAuditoria(Auditoria auditoria)
        {
            try
            {
                _context.Entry(auditoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _context.Entry(auditoria).Reload();
                _context.SaveChanges();
            }
        }



        public void EliminarAuditoria(Auditoria auditoria)
        {
            _context.Auditorias.Remove(auditoria);
            _context.SaveChanges();
        }

        public Auditoria ObtenerAuditoriaPorId(int id)
        {
            return _context.Auditorias.Find(id);
        }

        public List<Auditoria> ObtenerTodasLasAuditorias()
        {
            return _context.Auditorias.ToList();
        }
    }


}
