using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio.DTOs;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio.Interfaces;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Aplicacion.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionUsuarioControlador : ControllerBase
    {
        private readonly ISesionUsuarioRepositorio _sesionUsuarioRepositorio;
        private readonly IPeriodoInactividadRepositorio _periodoInactividadRepositorio;
        private readonly IPeriodoActividadRepositorio _periodoActividadRepositorio;

        public SesionUsuarioControlador(ISesionUsuarioRepositorio sesionUsuarioRepositorio, IPeriodoInactividadRepositorio periodoInactividadRepositorio, IPeriodoActividadRepositorio periodoActividadRepositorio)
        {
            _sesionUsuarioRepositorio = sesionUsuarioRepositorio;
            _periodoInactividadRepositorio = periodoInactividadRepositorio;
            _periodoActividadRepositorio = periodoActividadRepositorio;
        }

        [HttpPost("iniciar")]
        public async Task<IActionResult> IniciarDia()
        {
            var nombreEquipo = Environment.MachineName;
            var nombreUsuario = Environment.UserName;
            var hoy = DateTime.Today;

            var sesionExistente = await _sesionUsuarioRepositorio.ObtenerPorUsuarioAsync(nombreUsuario);
            if (sesionExistente.Any(s => s.HoraInicio.Date == hoy))
            {
                return BadRequest("El día ya ha sido iniciado para hoy.");
            }

            var sesionUsuario = new SesionUsuario
            {
                Id = Guid.NewGuid(),
                NombreEquipo = nombreEquipo,
                NombreUsuario = nombreUsuario,
                HoraInicio = DateTime.Now
            };

            await _sesionUsuarioRepositorio.AgregarAsync(sesionUsuario);

            var periodoActividad = new PeriodoActividad
            {
                Id = Guid.NewGuid(),
                SesionUsuarioId = sesionUsuario.Id,
                HoraInicio = DateTime.Now
            };

            await _periodoActividadRepositorio.AgregarAsync(periodoActividad);

            return Ok(ConvertirADto(sesionUsuario));
        }

        [HttpPost("finalizar/{nombreUsuario}")]
        public async Task<IActionResult> FinalizarDia(string nombreUsuario)
        {
            var sesionUsuario = (await _sesionUsuarioRepositorio.ObtenerPorUsuarioAsync(nombreUsuario)).FirstOrDefault(su => su.HoraFin == null);

            if (sesionUsuario == null)
            {
                return NotFound("Sesión no encontrada.");
            }

            sesionUsuario.HoraFin = DateTime.Now;
            await _sesionUsuarioRepositorio.ActualizarAsync(sesionUsuario);

            var periodoActividad = (await _periodoActividadRepositorio.ObtenerTodasAsync())
                .FirstOrDefault(pa => pa.SesionUsuarioId == sesionUsuario.Id && pa.HoraFin == null);

            if (periodoActividad != null)
            {
                periodoActividad.HoraFin = DateTime.Now;
                await _periodoActividadRepositorio.ActualizarAsync(periodoActividad);
            }

            var periodoInactividadExistente = (await _periodoInactividadRepositorio.ObtenerTodasAsync())
                .FirstOrDefault(pi => pi.SesionUsuarioId == sesionUsuario.Id && pi.HoraFin == null);

            if (periodoInactividadExistente != null)
            {
                periodoInactividadExistente.HoraFin = DateTime.Now;
                await _periodoInactividadRepositorio.ActualizarAsync(periodoInactividadExistente);
            }

            return Ok(ConvertirADto(sesionUsuario));
        }

        [HttpGet("usuario/{nombreUsuario}")]
        public async Task<IActionResult> ObtenerSesionesPorUsuario(string nombreUsuario)
        {
            var sesiones = await _sesionUsuarioRepositorio.ObtenerPorUsuarioAsync(nombreUsuario);
            return Ok(sesiones.Select(ConvertirADto));
        }

        [HttpGet("fecha/{fecha}")]
        public async Task<IActionResult> ObtenerSesionesPorFecha(DateTime fecha)
        {
            var sesiones = await _sesionUsuarioRepositorio.ObtenerPorFechaAsync(fecha);
            return Ok(sesiones.Select(ConvertirADto));
        }

        private static SesionUsuarioDto ConvertirADto(SesionUsuario sesionUsuario)
        {
            var totalInactividad = sesionUsuario.PeriodosInactividad
                .Where(pi => pi.HoraFin != null)
                .Aggregate(TimeSpan.Zero, (sum, pi) => sum + (pi.HoraFin.Value - pi.HoraInicio));

            var totalActividad = sesionUsuario.PeriodosActividad
                .Where(pa => pa.HoraFin != null)
                .Aggregate(TimeSpan.Zero, (sum, pa) => sum + (pa.HoraFin.Value - pa.HoraInicio));

            return new SesionUsuarioDto
            {
                Id = sesionUsuario.Id,
                NombreEquipo = sesionUsuario.NombreEquipo,
                NombreUsuario = sesionUsuario.NombreUsuario,
                HoraInicio = sesionUsuario.HoraInicio,
                HoraFin = sesionUsuario.HoraFin,
                PeriodosInactividad = sesionUsuario.PeriodosInactividad.Select(pi => new PeriodoInactividadDto
                {
                    Id = pi.Id,
                    HoraInicio = pi.HoraInicio,
                    HoraFin = pi.HoraFin
                }).ToList(),
                PeriodosActividad = sesionUsuario.PeriodosActividad.Select(pa => new PeriodoActividadDto
                {
                    Id = pa.Id,
                    HoraInicio = pa.HoraInicio,
                    HoraFin = pa.HoraFin
                }).ToList(),
                TotalInactividad = totalInactividad,
                TotalActividad = totalActividad
            };
        }
    }
}
