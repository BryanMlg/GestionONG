using GestionONG.Dtos;
using GestionONG.Models;
using GestionONG.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;
[Route("api/[controller]")]
[ApiController]
public class ProyectoController : ControllerBase
{
    private readonly GestionOngContext _context;

    public ProyectoController(GestionOngContext context)
    {
        _context = context;
    }

    // POST: ALL
[HttpPost("all")]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> All([FromBody] ProyectoPaginacionDto paginacionDto)
{
    if (paginacionDto.pageNumber < 1 || paginacionDto.pageSize < 1)
    {
        return BadRequest(responseHandler.Error<string>("La página y el tamaño deben ser mayores que cero."));
    }

    var totalRegistros = await _context.Proyectos.CountAsync(); 
    var totalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacionDto.pageSize); 

    var skip = (paginacionDto.pageNumber - 1) * paginacionDto.pageSize;
    var proyectos = await _context.Proyectos
        .Include(m => m.IdDepartamentoNavigation)
        .Include(m => m.IdMunicipioNavigation)
        .Skip(skip)
        .Take(paginacionDto.pageSize)
        .Select(m => new {
            id = m.Id,
            codigo = m.Codigo,
            nombre = m.Nombre,
            fechaInicio = m.FechaInicio,
            fechaFinal = m.FechaFin,
            idDepartamento = m.IdDepartamento,
            departamento = m.IdDepartamentoNavigation != null ? m.IdDepartamentoNavigation.Nombre : "No disponible",
            idMunicipio = m.IdMunicipio,
            municipio = m.IdMunicipioNavigation != null ? m.IdMunicipioNavigation.Nombre : "No disponible",
        })
        .ToListAsync();

    var response = new
    {
        TotalRegistros = totalRegistros,
        TotalPaginas = totalPaginas,
        Proyectos = proyectos
    };

    return Ok(responseHandler.Success(response));
}
    


    [HttpGet("label")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<object>>> LabelProyecto()
    {
        try
        {
            var proyectos = await _context.Proyectos
                .Select(d => new
                {
                    id = d.Id,
                    nombre = d.Nombre,
                })
                .ToListAsync();

            return Ok(proyectos);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al recuperar los datos de la base de datos.");
        }
    }

    [HttpPost("calculo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<CalculoProyecto>>>> Calculo([FromBody] int opcion = 1)
    {
        try
        {
            var results = new List<CalculoProyecto>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "[dbo].[calculoProyecto]";
                command.CommandType = CommandType.StoredProcedure;

                var param = new SqlParameter("@opcion", SqlDbType.Int) { Value = opcion };
                command.Parameters.Add(param);

                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(new CalculoProyecto
                        {
                            IdProyecto = reader.GetInt32(0),
                            NombreProyecto = reader.GetString(1),
                            TotalFondosRecibidos = reader.GetDecimal(2),
                            TotalGastado = reader.GetDecimal(3),
                            PorcentajeEjecucion = reader.GetDecimal(4)
                        });
                    }
                }
            }

            return Ok(responseHandler.Success(results));
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al ejecutar el procedimiento almacenado: " + ex.Message);
        }
    }

    [HttpPost("disponibilidad")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<RubroFinanzasDto>>>> Disponibilidad([FromBody] int idProyecto)
    {
        try
        {
            var results = new List<RubroFinanzasDto>();

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "[dbo].[obtenerDisponibilidad]";  // Llama al nuevo Stored Procedure
                command.CommandType = CommandType.StoredProcedure;

                var param = new SqlParameter("@idProyecto", SqlDbType.Int) { Value = idProyecto };
                command.Parameters.Add(param);

                if (command.Connection.State == ConnectionState.Closed)
                    await command.Connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        results.Add(new RubroFinanzasDto
                        {
                            RubroId = reader.GetInt32(0),
                            NombreRubro = reader.GetString(1),
                            TotalDonaciones = reader.GetDecimal(2),
                            TotalGastos = reader.GetDecimal(3),
                            DisponibilidadFondos = reader.GetDecimal(4) // Ya no se necesita cambiar aquí
                        });
                    }
                }
            }

            return Ok(responseHandler.Success(results)); // Devuelve la respuesta exitosa
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al ejecutar el procedimiento almacenado: " + ex.Message);
        }
    }



    [HttpPost("one")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Proyecto>> One([FromBody] ProyectoOneDto oneDto)
    {
        var proyecto = await _context.Proyectos
            .Where(m => m.Id == oneDto.id)
            .Include(m => m.IdDepartamentoNavigation)
            .Include(m => m.IdMunicipioNavigation)
            .Select(m => new
            {
                id = m.Id,
                codigo = m.Codigo,
                nombre = m.Nombre,
                fechaInicio = m.FechaInicio,
                fechaFin = m.FechaFin,
                idDepartamento = m.IdDepartamento,
                departamento = m.Nombre,
                idMunicipio = m.IdMunicipio,
                municipio = m.Nombre
            })
            .FirstOrDefaultAsync();

        if (proyecto == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        return Ok(responseHandler.Success(proyecto));
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromBody] ProyectoOneDto oneDto)
    {
        if (oneDto == null || oneDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("Ingrese un ID valido."));
        }

        var proyecto = await _context.Proyectos.FindAsync(oneDto?.id);

        if (proyecto == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        _context.Proyectos.Remove(proyecto);
        await _context.SaveChangesAsync();

        return Ok(responseHandler.Success<string>(""));
    }




    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Proyecto>> Create([FromBody] ProyectoCreateDto createDto)
    {
        if (string.IsNullOrWhiteSpace(createDto.nombre))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del proyecto es requerido."));
        }

        string codigoGenerado = await ProyectoHelper.GenerarCodigoProyecto(_context);

        var proyecto = new Proyecto
        {
            Nombre = createDto.nombre,
            Codigo = codigoGenerado,
            IdDepartamento = createDto.idDepartamento,
            IdMunicipio = createDto.idMunicipio,
            FechaInicio = createDto.fechaInicio,  
            FechaFin = createDto.fechaFin
        };

        _context.Proyectos.Add(proyecto);

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
            id = proyecto.Id,
            nombre = proyecto.Nombre,
            codigo = proyecto.Codigo,
            fechaInicio = proyecto.FechaInicio,
            fechaFin = proyecto.FechaFin,
        };

        return CreatedAtAction(nameof(One), new { id = proyecto.Id }, responseHandler.Success(result));
    }


    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] ProyectoUpdateDto updateDto)
    {

        if (string.IsNullOrWhiteSpace(updateDto.nombre))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del proyecto es requerido."));
        }

        if (updateDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("ID no válido."));
        }

        var proyecto = await _context.Proyectos.FindAsync(updateDto.id);

        if (proyecto == null)
        {
            return BadRequest(responseHandler.Error<string>("No existe el registro."));
        }

        proyecto.Nombre = updateDto.nombre;
        proyecto.IdDepartamento = updateDto.idDepartamento;
        proyecto.IdMunicipio = updateDto.idMunicipio;
        proyecto.FechaInicio = updateDto.fechaInicio;
        proyecto.FechaFin = updateDto.fechaFin;

        _context.Proyectos.Update(proyecto);
        await _context.SaveChangesAsync();

        var result = new
        {
            id = updateDto.id,
            nombre = updateDto.nombre,
        };

        return Ok(responseHandler.Success(result));
    }

}
