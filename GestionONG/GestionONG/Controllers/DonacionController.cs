using GestionONG.Dtos;
using GestionONG.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class DonacionController : ControllerBase
{
    private readonly GestionOngContext _context;

    public DonacionController(GestionOngContext context)
    {
        _context = context;
    }

    [HttpPost("all")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<IEnumerable<object>>>> All([FromBody] DonacionPaginacionDto paginacionDto)
    {
        if (paginacionDto.pageNumber < 1 || paginacionDto.pageSize < 1)
        {
            return BadRequest(responseHandler.Error<string>("La página y el tamaño deben ser mayores que cero."));
        }

        var totalRegistros = await _context.Donacions.CountAsync();
        var totalPaginas = (int)Math.Ceiling((double)totalRegistros / paginacionDto.pageSize);

        var skip = (paginacionDto.pageNumber - 1) * paginacionDto.pageSize;

        var donaciones = await _context.Donacions
            .Include(d => d.IdProyectoNavigation)
            .Include(d => d.IdRubroNavigation)
            .Skip(skip)
            .Take(paginacionDto.pageSize)
            .Select(d => new
            {
                id = d.Id,
                idProyecto = d.IdProyecto,
                proyecto = d.IdProyectoNavigation.Nombre,
                idRubro = d.IdRubro,
                rubro = d.IdRubroNavigation.Nombre,
                fechaDonacion = d.FechaDonacion,
                nombreDonante = d.NombreDonante,
                monto = d.Monto
            })
            .ToListAsync();

        var response = new
        {
            TotalRegistros = totalRegistros,
            TotalPaginas = totalPaginas,
            Donaciones = donaciones
        };

        return Ok(responseHandler.Success(response));
    }

    [HttpPost("one")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<object>>> One([FromBody] DonacionOneDto oneDto)
    {
        var donacion = await _context.Donacions
            .Include(d => d.IdProyectoNavigation)
            .Include(d => d.IdRubroNavigation)
            .Where(d => d.Id == oneDto.id)
            .Select(d => new
            {
                id = d.Id,
                idProyecto = d.IdProyecto,
                proyecto = d.IdProyectoNavigation.Nombre,
                idRubro = d.IdRubro,
                rubro = d.IdRubroNavigation.Nombre,
                fechaDonacion = d.FechaDonacion,
                nombreDonante = d.NombreDonante,
                monto = d.Monto,
            })
            .FirstOrDefaultAsync();

        if (donacion == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        return Ok(responseHandler.Success(donacion));
    }

    [HttpDelete("delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete([FromBody] DonacionOneDto oneDto)
    {
        if (oneDto == null || oneDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("Ingrese un ID valido."));
        }

        var donacion = await _context.Donacions.FindAsync(oneDto?.id);

        if (donacion == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        _context.Donacions.Remove(donacion);
        await _context.SaveChangesAsync();

        return Ok(responseHandler.Success<string>(""));
    }

    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Donacion>> Create([FromBody] DonacionCreateDto createDto)
    {
        var proyectoExists = await _context.Proyectos.AnyAsync(p => p.Id == createDto.idProyecto);

        if (string.IsNullOrWhiteSpace(createDto.nombreDonante))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del donante es requerido."));
        }

        if (!proyectoExists)
        {
            return BadRequest(responseHandler.Error<string>("El ID del proyecto no existe."));
        }

        var rubroExists = await _context.Rubros.AnyAsync(r => r.Id == createDto.idRubro);
        if (!rubroExists)
        {
            return BadRequest(responseHandler.Error<string>("El ID del rubro no existe."));
        }

        var donacion = new Donacion
        {
            IdProyecto = createDto.idProyecto,
            IdRubro = createDto.idRubro,
            FechaDonacion = createDto.fechaDonacion,
            NombreDonante = createDto.nombreDonante,
            Monto = createDto.monto
        };

        _context.Donacions.Add(donacion);

        try
        {
            await _context.SaveChangesAsync();

            var proyectoRubro = await _context.ProyectoRubros
                .Where(pr => pr.IdProyecto == createDto.idProyecto && pr.IdRubro == createDto.idRubro)
                .FirstOrDefaultAsync();

            if (proyectoRubro != null)
            {
                proyectoRubro.Presupuesto += createDto.monto;
                await _context.SaveChangesAsync();
            }
        }
        catch (DbUpdateException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error al guardar en la base de datos.");
        }

        var result = new
        {
            id = donacion.Id,
            idProyecto = donacion.IdProyecto,
            idRubro = donacion.IdRubro,
            fechaDonacion = donacion.FechaDonacion,
            nombreDonante = donacion.NombreDonante,
            monto = donacion.Monto
        };

        return CreatedAtAction(nameof(One), new { id = donacion.Id }, responseHandler.Success(result));
    }


    [HttpPut("update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] DonacionUpdateDto updateDto)
    {
        if (string.IsNullOrWhiteSpace(updateDto.nombreDonante))
        {
            return BadRequest(responseHandler.Error<string>("El nombre del rubro es requerido."));
        }

        if (updateDto == null || updateDto.id <= 0)
        {
            return BadRequest(responseHandler.Error<string>("ID no válido"));
        }

        var donacion = await _context.Donacions.FindAsync(updateDto.id);
        if (donacion == null)
        {
            return BadRequest(responseHandler.Error<string>("Registro no Encontrado."));
        }

        donacion.IdProyecto = updateDto.idProyecto;
        donacion.IdRubro = updateDto.idRubro;
        donacion.FechaDonacion = updateDto.fechaDonacion;
        donacion.NombreDonante = updateDto.nombreDonante;
        donacion.Monto = updateDto.monto;

        await _context.SaveChangesAsync();

        return Ok(responseHandler.Success<string>("Registro actualizado correctamente"));
    }
}
