using GestionONG.Dtos;
using GestionONG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class ProyectoRubroController : ControllerBase
{
    private readonly GestionOngContext _context;

    public ProyectoRubroController(GestionOngContext context)
    {
        _context = context;
    }

    [HttpPost("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> All([FromBody] ProyectoRubroPaginacionDto paginacionDto)
    {
        if (paginacionDto.pageNumber < 1 || paginacionDto.pageSize < 1)
        {
            return BadRequest(responseHandler.Error<string>("La página y el tamaño deben ser mayores que cero."));
        }

        var totalRegistros = await _context.ProyectoRubros
            .Where(pr => pr.IdProyecto == paginacionDto.id)
            .CountAsync();

        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacionDto.pageSize);
        var skip = (paginacionDto.pageNumber - 1) * paginacionDto.pageSize;

        var proyectoRubros = await _context.ProyectoRubros
            .Where(pr => pr.IdProyecto == paginacionDto.id)
            .Include(pr => pr.IdProyectoNavigation)
            .Include(pr => pr.IdRubroNavigation)
            .Select(pr => new
            {
                id = pr.Id, 
                idProyecto = pr.IdProyecto,
                proyecto = pr.IdProyectoNavigation.Nombre,
                idRubro = pr.IdRubro,
                nombreRubro = pr.IdRubroNavigation.Nombre,
                presupuesto = pr.Presupuesto
            })
            .Skip(skip)
            .Take(paginacionDto.pageSize)
            .ToListAsync();

        var response = new
        {
            TotalRegistros = totalRegistros,
            TotalPaginas = totalPaginas,
            ProyectoRubros = proyectoRubros
        };

        return Ok(responseHandler.Success(response));
    }





    [HttpPost("labelProyectoRubros")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> GetProyectoRubros([FromBody] int idProyecto)
    {
        if (idProyecto < 1)
        {
            return BadRequest(responseHandler.Error<string>("El ID del proyecto debe ser mayor que cero."));
        }

        var proyectoRubros = await _context.ProyectoRubros
            .Where(pr => pr.IdProyecto == idProyecto)
            .Include(pr => pr.IdRubroNavigation) 
            .Select(r => new
            {
                id = r.Id,
                idRubro = r.IdRubro,
                nombreRubro = r.IdRubroNavigation.Nombre
            })
            .ToListAsync();

        if (!proyectoRubros.Any())
        {
            return NotFound(responseHandler.Error<string>("No se encontraron rubros para el proyecto especificado."));
        }

        var response = new
        {
            ProyectoRubros = proyectoRubros
        };

        return Ok(responseHandler.Success(response));
    }


    [HttpPost("one")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProyectoRubro>> One([FromBody] RubroOneDto oneDto)
    {
        var proyectoRubro = await _context.ProyectoRubros
            .Where(pr => pr.Id == oneDto.id)
            .Select(pr => new {
                id = pr.Id,
                idRubro = pr.IdRubro,
                idProyecto = pr.IdProyecto,
                nombreProyecto = pr.IdProyectoNavigation.Nombre,
                nombreRubro = pr.IdRubroNavigation.Nombre
            })
            .FirstOrDefaultAsync();

        if (proyectoRubro == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        return Ok(responseHandler.Success(proyectoRubro));
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromBody] RubroOneDto oneDto)
    {
        if (oneDto == null || oneDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("Ingrese un ID valido."));
        }

        var proyectoRubro = await _context.ProyectoRubros.FindAsync(oneDto.id);

        if (proyectoRubro == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        _context.ProyectoRubros.Remove(proyectoRubro);
        await _context.SaveChangesAsync();

        return Ok(responseHandler.Success<string>(""));
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)] 
    public async Task<ActionResult<ProyectoRubro>> Create([FromBody] ProyectoRubroCreateDto createDto)
    {
        if (!createDto.idRubro.HasValue || !createDto.idProyecto.HasValue)
        {
            return BadRequest(responseHandler.Error<string>("Los campos idRubro e idProyecto son requeridos."));
        }

        var existingRubro = await _context.ProyectoRubros
            .FirstOrDefaultAsync(pr => pr.IdRubro == createDto.idRubro.Value && pr.IdProyecto == createDto.idProyecto.Value);

        if (existingRubro != null)
        {
            return Conflict(responseHandler.Error<string>("Ya existe un rubro con el mismo ID en este proyecto.")); 
        }

        var proyectoRubro = new ProyectoRubro
        {
            IdRubro = createDto.idRubro.Value,
            IdProyecto = createDto.idProyecto.Value,
        };

        _context.ProyectoRubros.Add(proyectoRubro);
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
            id = proyectoRubro.Id,
            idRubro = proyectoRubro.IdRubro,
            idProyecto = proyectoRubro.IdProyecto,
        };

        return CreatedAtAction(nameof(One), new { id = proyectoRubro.Id }, responseHandler.Success(result));
    }


    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] ProyectoRubroCreateDto updateDto)
    {
        if (!updateDto.idRubro.HasValue || !updateDto.idProyecto.HasValue || updateDto.idProyecto <= 0)
        {
            return BadRequest(responseHandler.Error<string>("Los campos idRubro y idProyecto son requeridos."));
        }

        var proyectoRubro = await _context.ProyectoRubros.FindAsync(updateDto.idProyecto);

        if (proyectoRubro == null)
        {
            return BadRequest(responseHandler.Error<string>("No existe el registro."));
        }

        proyectoRubro.IdRubro = updateDto.idRubro.Value;
        proyectoRubro.IdProyecto = updateDto.idProyecto.Value;

        _context.ProyectoRubros.Update(proyectoRubro);
        await _context.SaveChangesAsync();

        var result = new
        {
            id = proyectoRubro.Id,
            idRubro = proyectoRubro.IdRubro,
            idProyecto = proyectoRubro.IdProyecto,
        };

        return Ok(responseHandler.Success(result));
    }
}
