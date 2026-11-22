using CineApp.Back.DTOs;
using CineApp.Back.Services.Implementations;
using CineApp.Back.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CineApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ICatalogoProductoService _catalogoProductoService;

        public ProductosController(IProductoService productoService, ICatalogoProductoService catalogoProductoService)
        {
            this._productoService = productoService;
            this._catalogoProductoService = catalogoProductoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProductos(bool? activo, int? tipo, string? nombre)
        {
            var lstProductos = await _productoService.ObtenerTodosProductosAsync(activo, tipo, nombre);

            return Ok(lstProductos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductoById(int id)
        {
            var producto = await _productoService.ObtenerProductoPorIdAsync(id);
            
            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducto([FromBody] ProductoRequestDTO dto)
        {
            var creado = await _productoService.AñadirProductoAsync(dto);


            if (!creado.Exito)
                return BadRequest(new { mensaje = creado.Mensaje });

            return StatusCode(201, new
            {
                creado.Mensaje, creado.CreatedProduct
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, [FromBody] ProductoRequestDTO dto)
        {
            if (id <= 0)
                return BadRequest(new { mensaje = "El ID debe ser mayor a cero." });
            

            var actualizado = await _productoService.ActualizarProductoAsync(id, dto);

            if (!actualizado.Exito)
                return BadRequest(new { mensaje = actualizado.Mensaje });
            

            return Ok(new
            {
                actualizado.Mensaje, actualizado.NewProduct
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var eliminado = await _productoService.EliminarProductoAsync(id);

            if (!eliminado.Exito)
                return NotFound(new
                {
                    eliminado.Mensaje
                });

            return Ok(new
            {
                eliminado.Mensaje, eliminado.DeletedProduct
            });
        }

        [HttpGet("tipos")]
        public async Task<IActionResult> GetAllTiposProducto()
        {
            var lstTiposProducto = await _catalogoProductoService.ObtenerTodosTiposProductoAsync();

            return Ok(lstTiposProducto);
        }

        [HttpGet("tamaños")]
        public async Task<IActionResult> GetAllTamañosProducto()
        {
            var lstTamañosProducto = await _catalogoProductoService.ObtenerTodosTamañosProductoAsync();

            return Ok(lstTamañosProducto);
        }
    }
}
