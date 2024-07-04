using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio.Interfaces;

public class VerificarInactividadService : BackgroundService
{
    private const int UmbralInactividad = 300000; // 5 minutos en milisegundos
    private static bool _usuarioInactivo = false;
    private readonly IServiceProvider _serviceProvider;

    public VerificarInactividadService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await VerificarInactividad();
            await Task.Delay(1000, stoppingToken); // Verificar cada segundo
        }
    }

    private async Task VerificarInactividad()
    {
        using var scope = _serviceProvider.CreateScope();
        var sesionUsuarioRepositorio = scope.ServiceProvider.GetRequiredService<ISesionUsuarioRepositorio>();
        var periodoInactividadRepositorio = scope.ServiceProvider.GetRequiredService<IPeriodoInactividadRepositorio>();
        var periodoActividadRepositorio = scope.ServiceProvider.GetRequiredService<IPeriodoActividadRepositorio>();

        var tiempoInactividad = ObtenerTiempoInactividad();

        if (tiempoInactividad.TotalMilliseconds >= UmbralInactividad && !_usuarioInactivo)
        {
            _usuarioInactivo = true;

            var nombreEquipo = Environment.MachineName;
            var nombreUsuario = Environment.UserName;
            var sesionActiva = (await sesionUsuarioRepositorio.ObtenerPorUsuarioAsync(nombreUsuario))
                .FirstOrDefault(su => su.NombreEquipo == nombreEquipo && su.HoraFin == null);

            if (sesionActiva != null)
            {
                var periodoInactividad = new PeriodoInactividad
                {
                    Id = Guid.NewGuid(),
                    SesionUsuarioId = sesionActiva.Id,
                    HoraInicio = DateTime.Now.Subtract(tiempoInactividad),
                    HoraFin = null
                };

                await periodoInactividadRepositorio.AgregarAsync(periodoInactividad);

                var periodoActividad = (await periodoActividadRepositorio.ObtenerTodasAsync())
                    .FirstOrDefault(pa => pa.SesionUsuarioId == sesionActiva.Id && pa.HoraFin == null);

                if (periodoActividad != null)
                {
                    periodoActividad.HoraFin = DateTime.Now.Subtract(tiempoInactividad);
                    await periodoActividadRepositorio.ActualizarAsync(periodoActividad);
                }
            }
        }
        else if (tiempoInactividad.TotalMilliseconds < UmbralInactividad && _usuarioInactivo)
        {
            _usuarioInactivo = false;

            var nombreEquipo = Environment.MachineName;
            var nombreUsuario = Environment.UserName;
            var sesionActiva = (await sesionUsuarioRepositorio.ObtenerPorUsuarioAsync(nombreUsuario))
                .FirstOrDefault(su => su.NombreEquipo == nombreEquipo && su.HoraFin == null);

            if (sesionActiva != null)
            {
                var periodoInactividadExistente = (await periodoInactividadRepositorio.ObtenerTodasAsync())
                    .FirstOrDefault(pi => pi.SesionUsuarioId == sesionActiva.Id && pi.HoraFin == null);

                if (periodoInactividadExistente != null)
                {
                    periodoInactividadExistente.HoraFin = DateTime.Now;
                    await periodoInactividadRepositorio.ActualizarAsync(periodoInactividadExistente);
                }

                var nuevoPeriodoActividad = new PeriodoActividad
                {
                    Id = Guid.NewGuid(),
                    SesionUsuarioId = sesionActiva.Id,
                    HoraInicio = DateTime.Now
                };

                await periodoActividadRepositorio.AgregarAsync(nuevoPeriodoActividad);
            }
        }
    }

    private TimeSpan ObtenerTiempoInactividad()
    {
        LASTINPUTINFO ultimaEntradaInfo = new LASTINPUTINFO();
        ultimaEntradaInfo.cbSize = (uint)Marshal.SizeOf(ultimaEntradaInfo);
        GetLastInputInfo(ref ultimaEntradaInfo);

        uint tiempoInactividad = (uint)Environment.TickCount - ultimaEntradaInfo.dwTime;
        return TimeSpan.FromMilliseconds(tiempoInactividad);
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
