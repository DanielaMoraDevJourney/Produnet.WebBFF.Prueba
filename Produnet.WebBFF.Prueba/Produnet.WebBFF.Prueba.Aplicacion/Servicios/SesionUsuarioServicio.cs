using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Infraestructura.Data.Context;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Aplicacion.Servicios
{
    public class SesionUsuarioServicio
    {
        private readonly AplicacionDbContext _contexto;
        private const int UmbralInactividad = 300000; 
        private Timer _timer;

        public SesionUsuarioServicio(AplicacionDbContext contexto)
        {
            _contexto = contexto;
            _timer = new Timer(VerificarInactividad, null, 0, 1000); 
        }

        public async Task<SesionUsuario> IniciarDia()
        {
            var nombreEquipo = Environment.MachineName;
            var nombreUsuario = Environment.UserName;
            var hoy = DateTime.Today;

            var sesionExistente = await _contexto.SesionesUsuario
                .FirstOrDefaultAsync(us => us.NombreEquipo == nombreEquipo && us.NombreUsuario == nombreUsuario && us.HoraInicio.Date == hoy);

            if (sesionExistente != null)
            {
                throw new InvalidOperationException("El día ya ha sido iniciado para hoy.");
            }

            var sesionUsuario = new SesionUsuario
            {
                Id = Guid.NewGuid(),
                NombreEquipo = nombreEquipo,
                NombreUsuario = nombreUsuario,
                HoraInicio = DateTime.Now
            };

            _contexto.SesionesUsuario.Add(sesionUsuario);
            await _contexto.SaveChangesAsync();

            return sesionUsuario;
        }

        public async Task<SesionUsuario> FinalizarDia(Guid sesionId)
        {
            var sesionUsuario = await _contexto.SesionesUsuario.FindAsync(sesionId);

            if (sesionUsuario == null)
            {
                throw new KeyNotFoundException("Sesión no encontrada.");
            }

            sesionUsuario.HoraFin = DateTime.Now;
            await _contexto.SaveChangesAsync();

            return sesionUsuario;
        }

        public async Task<IEnumerable<SesionUsuario>> ObtenerSesionesPorUsuario(string nombreUsuario)
        {
            return await _contexto.SesionesUsuario
                .Include(us => us.PeriodosInactividad)
                .Where(us => us.NombreUsuario == nombreUsuario)
                .ToListAsync();
        }

        public async Task<IEnumerable<SesionUsuario>> ObtenerSesionesPorFecha(DateTime fecha)
        {
            return await _contexto.SesionesUsuario
                .Include(us => us.PeriodosInactividad)
                .Where(us => us.HoraInicio.Date == fecha.Date)
                .ToListAsync();
        }

        public TimeSpan ObtenerTiempoInactividad()
        {
            LASTINPUTINFO ultimaEntradaInfo = new LASTINPUTINFO();
            ultimaEntradaInfo.cbSize = (uint)Marshal.SizeOf(ultimaEntradaInfo);
            GetLastInputInfo(ref ultimaEntradaInfo);

            uint tiempoInactividad = (uint)Environment.TickCount - ultimaEntradaInfo.dwTime;
            return TimeSpan.FromMilliseconds(tiempoInactividad);
        }

        private async void VerificarInactividad(object state)
        {
            var tiempoInactividad = ObtenerTiempoInactividad();

            if (tiempoInactividad.TotalMilliseconds >= UmbralInactividad)
            {
                var nombreEquipo = Environment.MachineName;
                var nombreUsuario = Environment.UserName;
                var sesionActiva = await _contexto.SesionesUsuario
                    .Where(su => su.NombreEquipo == nombreEquipo && su.NombreUsuario == nombreUsuario && su.HoraFin == null)
                    .FirstOrDefaultAsync();

                if (sesionActiva != null)
                {
                    var periodoInactividadExistente = await _contexto.PeriodosInactividad
                        .Where(pi => pi.SesionUsuarioId == sesionActiva.Id && pi.HoraFin == null)
                        .FirstOrDefaultAsync();

                    if (periodoInactividadExistente == null)
                    {
                        var periodoInactividad = new PeriodoInactividad
                        {
                            Id = Guid.NewGuid(),
                            SesionUsuarioId = sesionActiva.Id,
                            HoraInicio = DateTime.Now.Subtract(tiempoInactividad),
                            HoraFin = null
                        };

                        _contexto.PeriodosInactividad.Add(periodoInactividad);
                        await _contexto.SaveChangesAsync();
                    }
                    else
                    {
                        periodoInactividadExistente.HoraFin = DateTime.Now;
                        await _contexto.SaveChangesAsync();
                    }
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }
    }
}
