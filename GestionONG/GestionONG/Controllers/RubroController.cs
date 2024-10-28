using GestionONG.Dtos;
using GestionONG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class RubroController : ControllerBase
{
    private readonly GestionOngContext _context;

    public RubroController(GestionOngContext context)
    {
        _context = context;
    }

    [HttpPost("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> All([FromBody] RubroPaginacionDto paginacionDto)
    {
        if (paginacionDto.pageNumber < 1 || paginacionDto.pageSize < 1)
        {
            return BadRequest(responseHandler.Error<string>("La página y el tamaño deben ser mayores que cero."));
        }

        var totalRegistros = await _context.Rubros.CountAsync();
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacionDto.pageSize);

        var skip = (paginacionDto.pageNumber - 1) * paginacionDto.pageSize;

        var rubros = await _context.Rubros
            .Skip(skip)
            .Take(paginacionDto.pageSize)
            .Select(r => new {
                id = r.Id,
                nombre = r.Nombre,
            })
            .ToListAsync();

        var response = new
        {
            TotalRegistros = totalRegistros,
            TotalPaginas = totalPaginas,
            Rubros = rubros
        };

        return Ok(responseHandler.Success(response));
    }


    [HttpGet("label")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<object>>> LabelRubro()
    {
        try
        {
            var rubros = await _context.Rubros
                .Select(d => new
                {
                    id = d.Id,
                    nombre = d.Nombre,
                })
                .ToListAsync();

            return Ok(rubros);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al recuperar los datos de la base de datos.");
        }
    }

    // POST: ONE
    [HttpPost("one")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Rubro>> One([FromBody] RubroOneDto oneDto)
    {
        var rubro = await _context.Rubros
            .Where(r => r.Id == oneDto.id)
            .Select(r => new {
                id = r.Id,
                nombre = r.Nombre,
            })
            .FirstOrDefaultAsync();

        if (rubro == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        return Ok(responseHandler.Success(rubro));
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

        var rubro = await _context.Rubros.FindAsync(oneDto.id);

        if (rubro == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        _context.Rubros.Remove(rubro);
        await _context.SaveChangesAsync();

        return Ok(responseHandler.Success<string>(""));
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Rubro>> Create([FromBody] RubroDto createDto)
    {
        if (string.IsNullOrWhiteSpace(createDto.nombre))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del rubro es requerido."));
        }

        var existeRubro = await _context.Rubros
            .AnyAsync(r => r.Nombre == createDto.nombre);

        if (existeRubro)
        {
            return BadRequest(responseHandler.Error<string>("Registro ya existente."));
        }

        var rubro = new Rubro
        {
            Nombre = createDto.nombre,
        };

        _context.Rubros.Add(rubro);
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
            id = rubro.Id,
            nombre = rubro.Nombre,
        };

        return CreatedAtAction(nameof(One), new { id = rubro.Id }, responseHandler.Success(result));
    }

    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] RubroUpdateDto updateDto)
    {
        if (string.IsNullOrWhiteSpace(updateDto.nombre))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del rubro es requerido."));
        }

        if (updateDto == null || updateDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("ID no válido"));
        }

        var rubro = await _context.Rubros.FindAsync(updateDto.id);

        if (rubro == null)
        {
            return BadRequest(responseHandler.Error<string>("No existe el registro."));
        }

        rubro.Nombre = updateDto.nombre;

        _context.Rubros.Update(rubro);
        await _context.SaveChangesAsync();

        var result = new
        {
            id = rubro.Id,
            nombre = rubro.Nombre,
        };

        return Ok(responseHandler.Success(result));
    }
}
