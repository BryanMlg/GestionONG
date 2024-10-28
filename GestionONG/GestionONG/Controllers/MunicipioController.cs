using GestionONG.Dtos;
using GestionONG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class MunicipioController : ControllerBase
{
    private readonly GestionOngContext _context;

    public MunicipioController(GestionOngContext context)
    {
        _context = context;
    }

    [HttpPost("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> All([FromBody] MunicipioPaginacionDto paginacionDto)
    {
        if (paginacionDto.pageNumber < 1 || paginacionDto.pageSize < 1)
        {
            return BadRequest(responseHandler.Error<string>("La página y el tamaño deben ser mayores que cero."));
        }

        var totalRegistros = await _context.Municipios.CountAsync(); 
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacionDto.pageSize); 

        var skip = (paginacionDto.pageNumber - 1) * paginacionDto.pageSize;

        var municipios = await _context.Municipios
            .Include(m => m.IdDepartamentoNavigation) 
            .Skip(skip)
            .Take(paginacionDto.pageSize)
            .Select(m => new {
                id = m.Id,
                nombre = m.Nombre,
                idDepartamento = m.IdDepartamento,
                departamento = m.Nombre 
            })
            .ToListAsync();

        var response = new
        {
            TotalRegistros = totalRegistros,
            TotalPaginas = totalPaginas,
            Municipios = municipios
        };

        return Ok(responseHandler.Success(response));
    }


    [HttpGet("label")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<object>>> LabelMunicipios()
    {
        try
        {
            var municipios = await _context.Municipios
                .Select(d => new
                {
                    id = d.Id,
                    nombre = d.Nombre,
                })
                .ToListAsync();

            return Ok(municipios);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al recuperar los datos de la base de datos.");
        }
    }

    [HttpPost("one")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Municipio>> One([FromBody] MunicipioOneDto oneDto)
    {
        var municipio = await _context.Municipios
            .Include(m => m.IdDepartamentoNavigation) 
            .Where(m => m.Id == oneDto.id)
            .Select(m => new
            {
                id = m.Id,
                nombre = m.Nombre,
                idDepartamento = m.IdDepartamento,
                departamento = m.Nombre,
            })
            .FirstOrDefaultAsync();

        if (municipio == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        return Ok(responseHandler.Success(municipio));
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromBody] MunicipioOneDto oneDto)
    {
        if (oneDto == null || oneDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("Ingrese un ID valido."));
        }

        var municipio = await _context.Municipios.FindAsync(oneDto?.id);

        if (municipio == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        _context.Municipios.Remove(municipio);
        await _context.SaveChangesAsync();

        return Ok(responseHandler.Success<string>(""));
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Municipio>> Create([FromBody] MunicipioCreateDto createDto)
    {
        if (string.IsNullOrWhiteSpace(createDto.nombre))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del departamento es requerido."));
        }

        var existeMunicipio = await _context.Municipios
            .AnyAsync(d => d.Nombre == createDto.nombre);

        if (existeMunicipio)
        {
            return BadRequest(responseHandler.Error<string>("Registro ya existente."));
        }

        var departamentoExists = await _context.Departamentos.AnyAsync(d => d.Id == createDto.idDepartamento);
        if (!departamentoExists)
        {
            return BadRequest(responseHandler.Error<string>("El ID del departamento no existe o fue deshabilitado."));
        }

        var municipio = new Municipio
        {
            Nombre = createDto.nombre,
            IdDepartamento = createDto.idDepartamento,
        };

        _context.Municipios.Add(municipio);
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
            id = municipio.Id,
            nombre = municipio.Nombre,
        };

        return CreatedAtAction(nameof(One), new { id = municipio.Id }, responseHandler.Success(result));
    }

    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] MunicipioUpdateDto updateDto)
    {
        if (string.IsNullOrWhiteSpace(updateDto.nombre))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del departamento es requerido."));
        }

        if (updateDto == null || updateDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("ID no válido"));
        }

        var municipio = await _context.Municipios.FindAsync(updateDto.id);

        if (municipio == null)
        {
            return BadRequest(responseHandler.Error<string>("No existe el registro."));
        }

        if (updateDto.idDepartamento <= 0) 
        {
            return BadRequest(responseHandler.Error<string>("ID de departamento no válido."));
        }

        var departamentoExists = await _context.Departamentos.AnyAsync(d => d.Id == updateDto.idDepartamento);
        if (!departamentoExists)
        {
            return BadRequest(responseHandler.Error<string>("El ID del departamento no existe."));
        }

        municipio.Nombre = updateDto.nombre;
        municipio.IdDepartamento = updateDto.idDepartamento;
      

        _context.Municipios.Update(municipio);
        await _context.SaveChangesAsync();

        var result = new
        {
            id = municipio.Id,
            nombre = updateDto.nombre,
        };

        return Ok(responseHandler.Success(result));
    }

}
