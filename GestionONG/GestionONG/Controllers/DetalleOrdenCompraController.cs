using GestionONG.Dtos;
using GestionONG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class DetalleOrdenCompraController : ControllerBase
{
    private readonly GestionOngContext _context;

    public DetalleOrdenCompraController(GestionOngContext context)
    {
        _context = context;
    }

    [HttpPost("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> All([FromBody] DetalleOrdenCompraPaginacionDto paginacionDto)
    {
        if (paginacionDto.pageNumber < 1 || paginacionDto.pageSize < 1)
        {
            return BadRequest(responseHandler.Error<string>("La página y el tamaño deben ser mayores que cero."));
        }

        var totalRegistros = await _context.DetalleOrdenCompras.CountAsync();
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacionDto.pageSize);

        var skip = (paginacionDto.pageNumber - 1) * paginacionDto.pageSize;

        var detalles = await _context.DetalleOrdenCompras
            .Include(oc => oc.IdOrdenCompraNavigation)
            .Include(r => r.IdRubroNavigation)
            .Skip(skip)
            .Take(paginacionDto.pageSize)
            .Select(d => new {
                id = d.Id,
                idOrdenCompra = d.IdOrdenCompra,
                ordenCompra = d.IdOrdenCompraNavigation.MontoTotal,
                fechaOrdenCompra = d.IdOrdenCompraNavigation.FechaOrden,
                idRubro = d.IdRubro,
                rubro = d.IdRubroNavigation.Nombre,
                nombreProducto = d.NombreProducto,
                monto = d.Monto,
            })
            .ToListAsync();

        var response = new
        {
            TotalRegistros = totalRegistros,
            TotalPaginas = totalPaginas,
            Detalles = detalles
        };

        return Ok(responseHandler.Success(response));
    }

    [HttpPost("one")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DetalleOrdenCompraOneDto>> One([FromBody] DetalleOrdenCompraOneDto oneDto)
    {
        var detalle = await _context.DetalleOrdenCompras
            .Where(d => d.Id == oneDto.id)
            .Select(d => new {
                id = d.Id,
                idOrdenCompra = d.IdOrdenCompra,
                ordenCompra = d.IdOrdenCompraNavigation.MontoTotal,
                fechaOrdenCompra = d.IdOrdenCompraNavigation.FechaOrden,
                idRubro = d.IdRubro,
                rubro = d.IdRubroNavigation.Nombre,
                nombreProducto = d.NombreProducto,
                monto = d.Monto,
            })
            .FirstOrDefaultAsync();

        if (detalle == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        return Ok(responseHandler.Success(detalle));
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromBody] DetalleOrdenCompraOneDto oneDto)
    {
        if (oneDto == null || oneDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("Ingrese un ID valido."));
        }

        var detalle = await _context.DetalleOrdenCompras.FindAsync(oneDto.id);

        if (detalle == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        _context.DetalleOrdenCompras.Remove(detalle);
        await _context.SaveChangesAsync();

        return Ok(responseHandler.Success<string>(""));
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DetalleOrdenCompraCreateDto>> Create([FromBody] DetalleOrdenCompraCreateDto createDto)
    {
        if (createDto.idOrdenCompra <= 0 || createDto.idRubro <= 0 || createDto.monto <= 0)
        {
            return BadRequest(responseHandler.Error<string>("Todos los campos son requeridos y deben ser mayores que cero."));
        }

        var detalleOrdenCompra = new DetalleOrdenCompra
        {
            IdOrdenCompra = createDto.idOrdenCompra,
            IdRubro = createDto.idRubro,
            Monto = createDto.monto,
            NombreProducto = createDto.nombreProducto,
        };

        _context.DetalleOrdenCompras.Add(detalleOrdenCompra);

        try
        {
            await _context.SaveChangesAsync();

            var proyectoRubro = await _context.ProyectoRubros
                .Where(pr => pr.IdProyecto == pr.IdProyecto && pr.IdRubro == createDto.idRubro)
                .FirstOrDefaultAsync();

            if (proyectoRubro != null)
            {
                proyectoRubro.Presupuesto -= createDto.monto; 
                await _context.SaveChangesAsync();
            }

            var nuevoTotal = await _context.DetalleOrdenCompras
                .Where(doc => doc.IdOrdenCompra == createDto.idOrdenCompra)
                .SumAsync(doc => doc.Monto);

            var ordenCompra = await _context.OrdenCompras.FindAsync(createDto.idOrdenCompra);
            if (ordenCompra != null)
            {
                ordenCompra.MontoTotal = nuevoTotal;
                await _context.SaveChangesAsync();
            }
        }
        catch (DbUpdateException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al guardar en la base de datos.");
        }

        var result = new
        {
            id = detalleOrdenCompra.Id,
            idOrdenCompra = detalleOrdenCompra.IdOrdenCompra,
            idRubro = detalleOrdenCompra.IdRubro,
            monto = detalleOrdenCompra.Monto,
        };

        return CreatedAtAction(nameof(One), new { id = detalleOrdenCompra.Id }, responseHandler.Success(result));
    }


    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] DetalleOrdenCompraUpdateDto updateDto)
    {
        if (updateDto == null || updateDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("ID no válido."));
        }

        var detalleOrdenCompra = await _context.DetalleOrdenCompras.FindAsync(updateDto.id);

        if (detalleOrdenCompra == null)
        {
            return BadRequest(responseHandler.Error<string>("No existe el registro."));
        }

        detalleOrdenCompra.IdOrdenCompra = updateDto.idOrdenCompra;
        detalleOrdenCompra. IdRubro = updateDto.idRubro;
        detalleOrdenCompra.Monto = updateDto.monto;
        detalleOrdenCompra.NombreProducto = updateDto.nombreProducto;

        _context.DetalleOrdenCompras.Update(detalleOrdenCompra);
        await _context.SaveChangesAsync();

        var result = new
        {
            id = detalleOrdenCompra.Id,
            idOrdenCompra = detalleOrdenCompra.IdOrdenCompra,
            idRubro = detalleOrdenCompra.IdRubro,
            monto = detalleOrdenCompra.Monto,
        };

        return Ok(responseHandler.Success(result));
    }
}
