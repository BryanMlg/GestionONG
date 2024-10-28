using GestionONG.Dtos;
using GestionONG.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class OrdenCompraController : ControllerBase
{
    private readonly GestionOngContext _context;

    public OrdenCompraController(GestionOngContext context)
    {
        _context = context;
    }

    [HttpPost("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> All([FromBody] OrdenCompraPaginacionDto paginacionDto)
    {
        if (paginacionDto.pageNumber < 1 || paginacionDto.pageSize < 1)
        {
            return BadRequest(responseHandler.Error<string>("La página y el tamaño deben ser mayores que cero."));
        }

        var totalRegistros = await _context.OrdenCompras.CountAsync();
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacionDto.pageSize);

        var skip = (paginacionDto.pageNumber - 1) * paginacionDto.pageSize;

        var ordenes = await _context.OrdenCompras
            .Include(o => o.IdProyectoNavigation)
            .Skip(skip)
            .Take(paginacionDto.pageSize)
            .Select(o => new
            {
                id = o.Id,
                idProyecto = o.IdProyecto,
                Proyecto = o.IdProyectoNavigation.Nombre,
                proveedor = o.Proveedor,
                fechaOrden = o.FechaOrden,
                montoTotal = o.MontoTotal
            })
            .ToListAsync();

        var response = new
        {
            TotalRegistros = totalRegistros,
            TotalPaginas = totalPaginas,
            OrdenesCompras = ordenes
        };

        return Ok(responseHandler.Success(response));
    }

    [HttpPost("one")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<object>>> One([FromBody] OrdenCompraOneDto oneDto)
    {
        var ordenCompra = await _context.OrdenCompras
            .Include(o => o.IdProyectoNavigation)
            .Where(o => o.Id == oneDto.id)
            .Select(o => new
            {
                id = o.Id,
                idProyecto = o.IdProyecto,
                Proyecto = o.IdProyectoNavigation.Nombre,
                proveedor = o.Proveedor,
                fechaOrden = o.FechaOrden,
                montoTotal = o.MontoTotal
            })
            .FirstOrDefaultAsync();

        if (ordenCompra == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        return Ok(responseHandler.Success(ordenCompra));
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromBody] OrdenCompraOneDto oneDto)
    {
        if (oneDto == null || oneDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("Ingrese un ID valido."));
        }

        var ordenCompra = await _context.OrdenCompras.FindAsync(oneDto?.id);

        if (ordenCompra == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        _context.OrdenCompras.Remove(ordenCompra);
        await _context.SaveChangesAsync();

        return Ok(responseHandler.Success<string>(""));
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrdenCompra>> Create([FromBody] OrdenCompraCreateDto createDto)
    {
        var proyectoExists = await _context.Proyectos.AnyAsync(p => p.Id == createDto.idProyecto);

        if (string.IsNullOrWhiteSpace(createDto.proveedor))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del rubro es requerido."));
        }

        if (!proyectoExists)
        {
            return BadRequest(responseHandler.Error<string>("El ID del proyecto no existe."));
        }

        var ordenCompra = new OrdenCompra
        {
            IdProyecto = createDto.idProyecto,
            Proveedor = createDto.proveedor,
            FechaOrden = createDto.fechaOrden,
            MontoTotal = createDto.montoTotal
        };

        _context.OrdenCompras.Add(ordenCompra);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al guardar en la base de datos.");
        }

        var result = new
        {
            id = ordenCompra.Id,
            idProyecto = ordenCompra.IdProyecto,
            proveedor = ordenCompra.Proveedor,
            fechaOrden = ordenCompra.FechaOrden,
            montoTotal = ordenCompra.MontoTotal
        };

        return CreatedAtAction(nameof(One), new { id = ordenCompra.Id }, responseHandler.Success(result));
    }

    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] OrdenCompraUpdateDto updateDto)
    {
        if (string.IsNullOrWhiteSpace(updateDto.proveedor))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del rubro es requerido."));
        }

        if (updateDto == null || updateDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("ID no válido"));
        }

        var ordenCompra = await _context.OrdenCompras.FindAsync(updateDto.id);
        if (ordenCompra == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        ordenCompra.IdProyecto = updateDto.idProyecto;
        ordenCompra.Proveedor = updateDto.proveedor;
        ordenCompra.FechaOrden = updateDto.fechaOrden;
        ordenCompra.MontoTotal = updateDto.montoTotal;

        await _context.SaveChangesAsync();

        return Ok(responseHandler.Success<string>("Registro actualizado correctamente"));
    }
}
