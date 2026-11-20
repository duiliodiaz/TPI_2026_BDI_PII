using CineApp.Back.DTOs;
using CineApp.Back.Models;
using CineApp.Back.Repositories.Implementations;
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
        private readonly IClienteRepository _clienteRepository;

        public ComprobanteService(
            IComprobanteRepository comprobanteRepository,
            IFormaPagoRepository formaPagoRepository,
            IProductoRepository productoRepository,
            IClienteRepository clienteRepository)
        {
            _comprobanteRepository = comprobanteRepository;
            _formaPagoRepository = formaPagoRepository;
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<(bool Exito, string Mensaje)> ActualizarComprobanteAsync(int id, ComprobanteRequestDTO dto)
        {
            var comprobanteExistente = await _comprobanteRepository.GetComprobanteByIdAsync(id);

            if (comprobanteExistente == null || !comprobanteExistente.Activo)
                return (false, "El comprobante no existe o ya fue anulado, no se puede modificar.");

            // validar que tenga al menos un detalle
            if (dto.Detalles == null || dto.Detalles.Count == 0)
                return (false, "El comprobante debe contener al menos un producto.");

            // validar que el detalle no tenga productos duplicados (ej 2 veces coca)
            var productosDuplicados = dto.Detalles
                .GroupBy(d => d.IdProducto)
                .Any(g => g.Count() > 1);

            if (productosDuplicados)
                return (false, "No se puede repetir el mismo producto en múltiples renglones del detalle. Sume las cantidades.");

            var fechaComprobante = dto.Fecha;
            // no permite fechas posteriores a ahora
            if (fechaComprobante > DateTime.Now)
                return (false, "La fecha del comprobante no puede ser futura.");

            var cliente = await _clienteRepository.GetClienteByIdAsync(id);
            // validar cliente existente y activo
            if (cliente == null || !cliente.Activo)
                return (false, "El cliente seleccionado no existe o está dado de baja.");

            var formaPagoExiste = await _formaPagoRepository.ExistsAsync(dto.IdFormaPago);

            if (!formaPagoExiste)
                return (false, "La forma de pago seleccionada no es válida.");

            var nuevosDetalles = new List<DetallesComprobanteProducto>();

            foreach (var detalle in dto.Detalles)
            {
                if (detalle.CantidadProductos <= 0)
                    return (false, $"La cantidad para el producto ID {detalle.IdProducto} debe ser mayor a cero.");

                var producto = await _productoRepository.GetProductoByIdAsync(detalle.IdProducto);

                if (producto == null || !producto.Activo)
                    return (false, $"El producto con ID '{detalle.IdProducto}' no existe o fue dado de baja.");

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

            var actualizado = await _comprobanteRepository.UpdateComprobanteAsync(comprobanteExistente, nuevosDetalles);

            if (actualizado)
                return (true, "Comprobante registrado correctamente.");
            else
                return (false, "Ocurrió un error al intentar actualizar el comprobante.");
        }

        public async Task<(bool Exito, string Mensaje)> AñadirComprobanteAsync(ComprobanteRequestDTO dto)
        {
            if (dto.Detalles == null || dto.Detalles.Count == 0)
                return (false, "El comprobante debe contener al menos un producto.");

            var productosDuplicados = dto.Detalles
                .GroupBy(d => d.IdProducto)
                .Any(g => g.Count() > 1);

            if (productosDuplicados)
                return (false, "No se puede repetir el mismo producto en múltiples renglones del detalle. Sume las cantidades.");

            var fechaComprobante = dto.Fecha;

            if (fechaComprobante > DateTime.Now)
                return (false, "La fecha del comprobante no puede ser futura.");

            var cliente = await _clienteRepository.GetClienteByIdAsync(dto.IdCliente);

            if (cliente == null || !cliente.Activo)
                return (false, "El cliente seleccionado no existe o está dado de baja.");

            var formaPagoExiste = await _formaPagoRepository.ExistsAsync(dto.IdFormaPago);

            if (!formaPagoExiste)
                return (false, "La forma de pago seleccionada no es válida.");

            var detallesComprobante = new List<DetallesComprobanteProducto>();

            foreach (var detalle in dto.Detalles)
            {
                if (detalle.CantidadProductos <= 0)
                    return (false, $"La cantidad para el producto ID {detalle.IdProducto} debe ser mayor a cero.");

                var producto = await _productoRepository.GetProductoByIdAsync(detalle.IdProducto);

                if (producto == null || !producto.Activo)
                    return (false, $"El producto con ID '{detalle.IdProducto}' no existe o fue dado de baja.");

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

            var creado = await _comprobanteRepository.AddComprobanteAsync(nuevoComprobante);

            if (creado)
                return (true, "Comprobante registrado correctamente.");
            else
                return (false, "Ocurrió un error al intentar crear el comprobante.");
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
