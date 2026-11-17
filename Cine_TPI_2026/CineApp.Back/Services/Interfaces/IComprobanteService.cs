using CineApp.Back.DTOs.Comprobantes;
using CineApp.Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Services.Interfaces
{
    public interface IComprobanteService
    {
        Task<List<ComprobanteResponseDTO>> ObtenerTodosComprobantesAsync();

        Task<ComprobanteResponseDTO?> ObtenerComprobantePorIdAsync(int id);

        Task<bool> AñadirComprobanteAsync(ComprobanteRequestDTO dto);

        Task<bool> ActualizarComprobanteAsync(int id, ComprobanteRequestDTO dto);

        Task<bool> EliminarComprobanteAsync(int id);

        Task<List<FormasPago>> ObtenerTodasFormasPagoAsync();
    }
}
