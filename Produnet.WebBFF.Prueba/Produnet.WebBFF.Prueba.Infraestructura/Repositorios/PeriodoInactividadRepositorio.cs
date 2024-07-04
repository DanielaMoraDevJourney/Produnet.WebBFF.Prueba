using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio.Interfaces;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Infraestructura.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Infraestructura.Repositorios
{
    public class PeriodoInactividadRepositorio : IPeriodoInactividadRepositorio
    {
        private readonly AplicacionDbContext _contexto;

        public PeriodoInactividadRepositorio(AplicacionDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<PeriodoInactividad> ObtenerPorIdAsync(Guid id)
        {
            return await _contexto.PeriodosInactividad.FindAsync(id);
        }

        public async Task<IEnumerable<PeriodoInactividad>> ObtenerTodasAsync()
        {
            return await _contexto.PeriodosInactividad.ToListAsync();
        }

        public async Task AgregarAsync(PeriodoInactividad periodoInactividad)
        {
            _contexto.PeriodosInactividad.Add(periodoInactividad);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(PeriodoInactividad periodoInactividad)
        {
            _contexto.PeriodosInactividad.Update(periodoInactividad);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(Guid id)
        {
            var periodoInactividad = await ObtenerPorIdAsync(id);
            _contexto.PeriodosInactividad.Remove(periodoInactividad);
            await _contexto.SaveChangesAsync();
        }
    }
}
