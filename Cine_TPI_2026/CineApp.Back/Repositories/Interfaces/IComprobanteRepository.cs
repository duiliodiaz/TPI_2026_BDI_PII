using CineApp.Back.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Repositories.Interfaces
{
    public interface IComprobanteRepository
    {
        Task<int> GetIdNewComprobanteAsync();
        Task<List<Comprobante>> GetAllComprobantesAsync();

        Task<Comprobante?> GetComprobanteByIdAsync(int id);

        Task<bool> AddComprobanteAsync(Comprobante comprobante);

        Task<bool> UpdateComprobanteAsync(Comprobante comprobante, List<DetallesComprobanteProducto> nuevosDetalles);

        Task<bool> DeleteComprobanteAsync(int id);
    }
}
