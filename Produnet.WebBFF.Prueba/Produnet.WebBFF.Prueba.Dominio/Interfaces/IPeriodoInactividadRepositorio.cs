using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio;

namespace Produnet.WebBFF.Prueba.Produnet.WebBFF.Prueba.Dominio.Interfaces
{
    public interface IPeriodoInactividadRepositorio
    {
        Task<PeriodoInactividad> ObtenerPorIdAsync(Guid id);
        Task<IEnumerable<PeriodoInactividad>> ObtenerTodasAsync();
        Task AgregarAsync(PeriodoInactividad periodoInactividad);
        Task ActualizarAsync(PeriodoInactividad periodoInactividad);
        Task EliminarAsync(Guid id);
    }
}
