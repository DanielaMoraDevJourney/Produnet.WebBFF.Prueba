using Microsoft.AspNetCore.Mvc;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Aplicacion.Servicios;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Aplicacion.Controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class SesionUsuarioControlador : ControllerBase
    {
        private readonly SesionUsuarioServicio _sesionUsuarioServicio;

        public SesionUsuarioControlador(SesionUsuarioServicio sesionUsuarioServicio)
        {
            _sesionUsuarioServicio = sesionUsuarioServicio;
        }

        [HttpPost("iniciar")]
        public async Task<IActionResult> IniciarDia()
        {
            try
            {
                var sesion = await _sesionUsuarioServicio.IniciarDia();
                return Ok(sesion);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("finalizar/{sesionId}")]
        public async Task<IActionResult> FinalizarDia(Guid sesionId)
        {
            try
            {
                var sesion = await _sesionUsuarioServicio.FinalizarDia(sesionId);
                return Ok(sesion);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("usuario/{nombreUsuario}")]
        public async Task<IActionResult> ObtenerSesionesPorUsuario(string nombreUsuario)
        {
            var sesiones = await _sesionUsuarioServicio.ObtenerSesionesPorUsuario(nombreUsuario);
            return Ok(sesiones);
        }

        [HttpGet("fecha/{fecha}")]
        public async Task<IActionResult> ObtenerSesionesPorFecha(DateTime fecha)
        {
            var sesiones = await _sesionUsuarioServicio.ObtenerSesionesPorFecha(fecha);
            return Ok(sesiones);
        }

        [HttpGet("inactividad")]
        public IActionResult ObtenerTiempoInactividad()
        {
            var tiempoInactividad = _sesionUsuarioServicio.ObtenerTiempoInactividad();
            return Ok(tiempoInactividad);
        }
    }
}

