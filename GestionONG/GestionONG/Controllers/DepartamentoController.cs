using GestionONG.Dtos;
using GestionONG.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Route("api/[controller]")]
[ApiController]
public class DepartamentoController : ControllerBase
{
    private readonly GestionOngContext _context;

    public DepartamentoController(GestionOngContext context)
    {
        _context = context;
    }

    [HttpPost("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> All([FromBody] DepartamentoPaginacionDto paginacionDto)
    {
        if (paginacionDto.pageNumber < 1 || paginacionDto.pageSize < 1)
        {
            return BadRequest(responseHandler.Error<string>("La página y el tamaño deben ser mayores que cero."));
        }

        var totalRegistros = await _context.Departamentos.CountAsync(); 
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacionDto.pageSize); 

        var skip = (paginacionDto.pageNumber - 1) * paginacionDto.pageSize;
        var departamentos = await _context.Departamentos
            .Skip(skip)
            .Take(paginacionDto.pageSize)
            .Select(d => new
            {
                id = d.Id,
                nombre = d.Nombre,
            })
            .ToListAsync();

        var response = new
        {
            TotalRegistros = totalRegistros,
            TotalPaginas = totalPaginas,
            Departamentos = departamentos
        };

        return Ok(responseHandler.Success(response));
    }

    [HttpGet("label")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<object>>> labelDepartamentos()
    {
        try
        {
            var departamentos = await _context.Departamentos
                .Select(d => new
                {
                    id = d.Id,
                    nombre = d.Nombre,
                })
                .ToListAsync();

            return Ok(departamentos);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al recuperar los datos de la base de datos.");
        }
    }


    [HttpPost("municipios")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> GetMunicipios([FromBody] DepartamentoMunicipioDto paginacionDto)
    {
        if (paginacionDto.pageNumber < 1 || paginacionDto.pageSize < 1)
        {
            return BadRequest(responseHandler.Error<string>("La página y el tamaño deben ser mayores que cero."));
        }

        var departamento = await _context.Departamentos
            .Include(d => d.Municipios)
            .FirstOrDefaultAsync(d => d.Id == paginacionDto.id);

        if (departamento == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        var totalRegistros = departamento.Municipios.Count;
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacionDto.pageSize);

        var skip = (paginacionDto.pageNumber - 1) * paginacionDto.pageSize;

        var municipios = departamento.Municipios
            .Skip(skip)
            .Take(paginacionDto.pageSize)
            .Select(m => new
            {
                id = m.Id,
                nombre = m.Nombre
            })
            .ToList();

        var response = new
        {
            TotalRegistros = totalRegistros,
            TotalPaginas = totalPaginas,
            Municipios = municipios
        };

        return Ok(responseHandler.Success(response));
    }

    [HttpPost("labelMunicipios")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> GetMunicipios([FromBody] int idDepartamento)
    {
        if (idDepartamento < 1)
        {
            return BadRequest(responseHandler.Error<string>("El ID del departamento debe ser mayor que cero."));
        }

        var departamento = await _context.Departamentos
            .Include(d => d.Municipios)
            .FirstOrDefaultAsync(d => d.Id == idDepartamento);

        if (departamento == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        var municipios = departamento.Municipios
            .Select(m => new
            {
                id = m.Id,
                nombre = m.Nombre
            })
            .ToList();

        var response = new
        {
            Municipios = municipios
        };

        return Ok(responseHandler.Success(response));
    }

    [HttpPost("one")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Municipio>> One([FromBody] DepartamentoOneDto oneDto)
    {
        var departamento = await _context.Departamentos
            .Where(m => m.Id == oneDto.id)
            .Select(m => new
            {
                id = m.Id,
                nombre = m.Nombre,
                
            })
            .FirstOrDefaultAsync();

        if (departamento == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        return Ok(responseHandler.Success(departamento));
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromBody] DepartamentoOneDto oneDto)
    {
        if (oneDto == null || oneDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("Ingree un ID valido."));
        }

        var departamento = await _context.Departamentos
            .Include(d => d.Municipios) 
            .FirstOrDefaultAsync(d => d.Id == oneDto.id);

        if (departamento == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        if (departamento.Municipios != null && departamento.Municipios.Any())
        {
            _context.Municipios.RemoveRange(departamento.Municipios);
        }

        _context.Departamentos.Remove(departamento);
        await _context.SaveChangesAsync();

        return Ok(responseHandler.Success<string>(""));
    }




    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Departamento>> Create([FromBody] DepartamentoCreateDto createDto)
    {
       
        if (string.IsNullOrWhiteSpace(createDto.nombre))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del departamento es requerido."));
        }

        var existeDepartamento = await _context.Departamentos
            .AnyAsync(d => d.Nombre == createDto.nombre);

        if (existeDepartamento)
        {
            return BadRequest(responseHandler.Error<string>("Registro ya existente."));
        }

        var departamento = new Departamento
        {
            Nombre = createDto.nombre,
        };

        _context.Departamentos.Add(departamento);

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
            id = departamento.Id,
            nombre = departamento.Nombre,
        };

        return CreatedAtAction(nameof(One), new { id = departamento.Id }, responseHandler.Success(result));
    }



    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] DepartamentoUpdateDto updateDto)
    {
        if (string.IsNullOrWhiteSpace(updateDto?.nombre))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del departamento es requerido."));
        }

        if (updateDto?.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("ID no valido"));
        }

        var departamento = await _context.Departamentos.FindAsync(updateDto.id);

        if (departamento == null)
        {
            return BadRequest(responseHandler.Error<string>("No existe el registro."));
        }

        departamento.Nombre = updateDto.nombre;

        _context.Departamentos.Update(departamento);
        await _context.SaveChangesAsync();

        var result = new
        {
            id = updateDto.id,
            nombre = updateDto.nombre,
        };

        return Ok(responseHandler.Success(result));
    }

}
