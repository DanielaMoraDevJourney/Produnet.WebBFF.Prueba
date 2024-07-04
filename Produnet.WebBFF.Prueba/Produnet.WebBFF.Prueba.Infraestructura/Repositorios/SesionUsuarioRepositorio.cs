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
    public class SesionUsuarioRepositorio : ISesionUsuarioRepositorio
    {
        private readonly AplicacionDbContext _contexto;

        public SesionUsuarioRepositorio(AplicacionDbContext contexto)
        {
            _contexto = contexto;
        }

        public async Task<SesionUsuario> ObtenerPorIdAsync(Guid id)
        {
            return await _contexto.SesionesUsuario
                .Include(s => s.PeriodosInactividad)
                .Include(s => s.PeriodosActividad)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<SesionUsuario>> ObtenerTodasAsync()
        {
            return await _contexto.SesionesUsuario
                .Include(s => s.PeriodosInactividad)
                .Include(s => s.PeriodosActividad)
                .ToListAsync();
        }

        public async Task<IEnumerable<SesionUsuario>> ObtenerPorUsuarioAsync(string nombreUsuario)
        {
            return await _contexto.SesionesUsuario
                .Include(s => s.PeriodosInactividad)
                .Include(s => s.PeriodosActividad)
                .Where(s => s.NombreUsuario == nombreUsuario)
                .ToListAsync();
        }

        public async Task<IEnumerable<SesionUsuario>> ObtenerPorFechaAsync(DateTime fecha)
        {
            return await _contexto.SesionesUsuario
                .Include(s => s.PeriodosInactividad)
                .Include(s => s.PeriodosActividad)
                .Where(s => s.HoraInicio.Date == fecha.Date)
                .ToListAsync();
        }

        public async Task AgregarAsync(SesionUsuario sesionUsuario)
        {
            _contexto.SesionesUsuario.Add(sesionUsuario);
            await _contexto.SaveChangesAsync();
        }

        public async Task ActualizarAsync(SesionUsuario sesionUsuario)
        {
            _contexto.SesionesUsuario.Update(sesionUsuario);
            await _contexto.SaveChangesAsync();
        }

        public async Task EliminarAsync(Guid id)
        {
            var sesionUsuario = await ObtenerPorIdAsync(id);
            _contexto.SesionesUsuario.Remove(sesionUsuario);
            await _contexto.SaveChangesAsync();
        }
    }
}
