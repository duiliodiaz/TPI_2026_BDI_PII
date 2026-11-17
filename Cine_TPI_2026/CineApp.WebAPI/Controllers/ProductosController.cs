using CineApp.Back.DTOs.Clientes;
using CineApp.Back.DTOs.Productos;
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
        public async Task<IActionResult> GetAllProductos()
        {
            var lstProductos = await _productoService.ObtenerTodosProductosAsync();

            return Ok(lstProductos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductoById(int id)
        {
            var producto = await _productoService.ObtenerProductoPorIdAsync(id);

            return Ok(producto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducto([FromBody] ProductoDTO dto)
        {
            bool creado = await _productoService.AñadirProductoAsync(dto);

            if (!creado)
            {
                return BadRequest("No se pudo crear el producto. Verifique los datos ingresados.");
            }

            return Ok(new { mensaje = "Producto creado exitosamente" });
            //return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, [FromBody] ProductoDTO dto)
        {
            if (dto.IdProducto != 0 && id != dto.IdProducto)
            {
                return BadRequest("El ID de la URL no coincide con el del cuerpo de la petición.");
            }

            bool actualizado = await _productoService.ActualizarProductoAsync(id, dto);

            if (!actualizado)
            {
                return NotFound("No se encontró el producto activo o los datos son inválidos.");
            }

            return Ok(new { mensaje = "Producto actualizado exitosamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            bool eliminado = await _productoService.EliminarProductoAsync(id);

            if (!eliminado)
            {
                return NotFound("No se encontró el producto activo.");
            }

            return NoContent();
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
