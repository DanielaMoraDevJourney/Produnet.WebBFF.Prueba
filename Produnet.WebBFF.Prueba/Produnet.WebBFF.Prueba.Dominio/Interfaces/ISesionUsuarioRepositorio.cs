using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio.Interfaces
{
    public interface ISesionUsuarioRepositorio
    {
        Task<SesionUsuario> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<SesionUsuario>> ObtenerTodasAsync();
        Task<IEnumerable<SesionUsuario>> ObtenerPorUsuarioAsync(string nombreUsuario);
        Task<IEnumerable<SesionUsuario>> ObtenerPorFechaAsync(DateTime fecha);
        Task AgregarAsync(SesionUsuario sesionUsuario);
        Task ActualizarAsync(SesionUsuario sesionUsuario);
        Task EliminarAsync(Guid id);
    }
}
