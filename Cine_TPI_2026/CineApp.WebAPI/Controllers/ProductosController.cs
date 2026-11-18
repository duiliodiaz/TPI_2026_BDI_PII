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
            var creado = await _productoService.AñadirProductoAsync(dto);


            if (!creado.Exito)
                return BadRequest(new { mensaje = creado.Mensaje });

            return Ok(new { mensaje = creado.Mensaje });
            //return StatusCode(201, new { mensaje = resultado.Mensaje });
            //return Created();
            //return Ok(new { mensaje = "Producto creado exitosamente" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, [FromBody] ProductoDTO dto)
        {
            if (id <= 0)
                return BadRequest(new { mensaje = "El ID debe ser mayor a cero." });
            

            if (dto.IdProducto != 0 && id != dto.IdProducto)
                return BadRequest(new { mensaje = "El ID de la URL no coincide con el ID del producto enviado." });
            

            var actualizado = await _productoService.ActualizarProductoAsync(id, dto);

            if (!actualizado.Exito)
                return BadRequest(new { mensaje = actualizado.Mensaje });
            

            return Ok(new { mensaje = actualizado.Mensaje });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            bool eliminado = await _productoService.EliminarProductoAsync(id);

            if (!eliminado)
                return NotFound("No se encontró el producto activo.");

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
