using CineApp.Back.DTOs.Comprobantes;
using CineApp.Back.Models;
using CineApp.Back.Repositories.Interfaces;
using CineApp.Back.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineApp.Back.Services.Implementations
{
    public class ComprobanteService : IComprobanteService
    {
        private readonly IComprobanteRepository _comprobanteRepository;
        private readonly IFormaPagoRepository _formaPagoRepository;
        private readonly IProductoRepository _productoRepository;

        public ComprobanteService(
            IComprobanteRepository comprobanteRepo,
            IFormaPagoRepository formaPagoRepo,
            IProductoRepository productoRepo)
        {
            _comprobanteRepository = comprobanteRepo;
            _formaPagoRepository = formaPagoRepo;
            _productoRepository = productoRepo;
        }

        public async Task<bool> ActualizarComprobanteAsync(int id, ComprobanteRequestDTO dto)
        {
            if (dto.IdCliente <= 0 || dto.IdFormaPago <= 0 || dto.Detalles == null || dto.Detalles.Count == 0)
                return false;

            var comprobanteExistente = await _comprobanteRepository.GetComprobanteByIdAsync(id);

            if (comprobanteExistente == null)
                return false;

            var nuevosDetalles = new List<DetallesComprobanteProducto>();

            foreach (var detalle in dto.Detalles)
            {
                if (detalle.CantidadProductos <= 0)
                    return false;

                var producto = await _productoRepository.GetProductoByIdAsync(detalle.IdProducto);

                if (producto == null)
                    return false;

                nuevosDetalles.Add(new DetallesComprobanteProducto
                {
                    IdComprobante = id,
                    IdProducto = detalle.IdProducto,
                    CantidadProductos = detalle.CantidadProductos,
                    PrecioUnitario = producto.PreUnitario
                });
            }

            comprobanteExistente.IdCliente = dto.IdCliente;
            comprobanteExistente.Fecha = dto.Fecha;
            comprobanteExistente.IdFormaPago = dto.IdFormaPago;

            return await _comprobanteRepository.UpdateComprobanteAsync(comprobanteExistente, nuevosDetalles);
        }

        public async Task<bool> AñadirComprobanteAsync(ComprobanteRequestDTO dto)
        {
            if (dto.IdCliente <= 0 || dto.IdFormaPago <= 0 || dto.Detalles == null || dto.Detalles.Count == 0)
                return false;

            var detallesComprobante = new List<DetallesComprobanteProducto>();

            foreach (var detalle in dto.Detalles)
            {
                if (detalle.CantidadProductos <= 0)
                    return false;

                var producto = await _productoRepository.GetProductoByIdAsync(detalle.IdProducto);

                if (producto == null)
                    return false;

                detallesComprobante.Add(new DetallesComprobanteProducto
                {
                    IdProducto = detalle.IdProducto,
                    CantidadProductos = detalle.CantidadProductos,
                    PrecioUnitario = producto.PreUnitario
                });
            }

            var nuevoComprobante = new Comprobante
            {
                IdCliente = dto.IdCliente,
                Fecha = dto.Fecha,
                IdFormaPago = dto.IdFormaPago,
                Activo = true,
                DetallesComprobanteProductos = detallesComprobante
            };

            return await _comprobanteRepository.AddComprobanteAsync(nuevoComprobante);
        }

        public async Task<bool> EliminarComprobanteAsync(int id)
        {
            return await _comprobanteRepository.DeleteComprobanteAsync(id);
        }

        public async Task<ComprobanteResponseDTO?> ObtenerComprobantePorIdAsync(int id)
        {
            var comprobante = await _comprobanteRepository.GetComprobanteByIdAsync(id);

            if (comprobante == null)
            {
                return null;
            }
            else
            {
                return MapearComprobanteHaciaDTO(comprobante);
            }
        }

        public async Task<List<FormasPago>> ObtenerTodasFormasPagoAsync()
        {
            return await _formaPagoRepository.GetAllFormasPagoAsync();
        }

        public async Task<List<ComprobanteResponseDTO>> ObtenerTodosComprobantesAsync()
        {
            var lista = await _comprobanteRepository.GetAllComprobantesAsync();

            return lista.Select(MapearComprobanteHaciaDTO).ToList();
        }

        private static ComprobanteResponseDTO MapearComprobanteHaciaDTO(Comprobante comprobante)
        {
            return new ComprobanteResponseDTO
            {
                IdComprobante = comprobante.IdComprobante,
                IdCliente = comprobante.IdCliente,
                ClienteNombreCompleto = comprobante.IdClienteNavigation != null
                    ? $"{comprobante.IdClienteNavigation.Nombre} {comprobante.IdClienteNavigation.Apellido}"
                    : string.Empty,
                Fecha = comprobante.Fecha,
                IdFormaPago = comprobante.IdFormaPago,
                FormaPagoNombre = comprobante.IdFormaPagoNavigation?.Descripcion ?? string.Empty,

                Detalles = comprobante.DetallesComprobanteProductos.Select(d => new DetalleComprobanteResponseDTO
                {
                    IdDetalle = d.IdDetalle,
                    IdProducto = d.IdProducto,
                    ProductoNombre = d.IdProductoNavigation?.Descripcion ?? string.Empty,
                    CantidadProductos = d.CantidadProductos,
                    PrecioUnitario = d.PrecioUnitario
                }).ToList()
            };
        }
    }
}
