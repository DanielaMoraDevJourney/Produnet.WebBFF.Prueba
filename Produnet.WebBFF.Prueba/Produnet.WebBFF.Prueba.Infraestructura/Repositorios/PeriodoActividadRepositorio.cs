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
    public class PeriodoActividadRepositorio : IPeriodoActividadRepositorio
    {
        private readonly AplicacionDbContext _contexto;

        public PeriodoActividadRepositorio(AplicacionDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<PeriodoActividad> ObtenerPorIdAsync(Guid id)
        {
            return await _contexto.PeriodosActividad.FindAsync(id);
        }

        public async Task<IEnumerable<PeriodoActividad>> ObtenerTodasAsync()
        {
            return await _contexto.PeriodosActividad.ToListAsync();
        }

        public async Task AgregarAsync(PeriodoActividad periodoActividad)
        {
            _contexto.PeriodosActividad.Add(periodoActividad);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(PeriodoActividad periodoActividad)
        {
            _contexto.PeriodosActividad.Update(periodoActividad);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(Guid id)
        {
            var periodoActividad = await ObtenerPorIdAsync(id);
            _contexto.PeriodosActividad.Remove(periodoActividad);
            await _contexto.SaveChangesAsync();
        }
    }
}
