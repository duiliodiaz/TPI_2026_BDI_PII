using CineApp.Back.DTOs;
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
        Task<int> ObtenerIdNuevoComprobanteAsync();
        Task<List<ComprobanteResponseDTO>> ObtenerTodosComprobantesAsync();

        Task<ComprobanteResponseDTO?> ObtenerComprobantePorIdAsync(int id);

        Task<(bool Exito, string Mensaje)> AñadirComprobanteAsync(ComprobanteRequestDTO dto);

        Task<(bool Exito, string Mensaje)> ActualizarComprobanteAsync(int id, ComprobanteRequestDTO dto);

        Task<bool> EliminarComprobanteAsync(int id);

        Task<List<FormasPago>> ObtenerTodasFormasPagoAsync();
    }
}
