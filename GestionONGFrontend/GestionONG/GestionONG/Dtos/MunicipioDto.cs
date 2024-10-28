using System.ComponentModel.DataAnnotations;

namespace GestionONG.Dtos
{
    public class MunicipioCreateDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "El nombre no debe tener más de 100 caracteres.")]
        public string? nombre { get; set; }
        [Required(ErrorMessage = "El campo idDepartamento es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de idDepartamento no puede ser negativo.")]
        public int? idDepartamento { get; set; }
    }

    public class MunicipioUpdateDto
    {
        [Required]
        public int id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "El nombre no debe tener más de 100 caracteres.")]
        public string? nombre { get; set; }
        [Required]
        public int? idDepartamento { get; set; }
    }

    public class MunicipioOneDto
    {
        public int id { get; set; }
    }

    public class MunicipioPaginacionDto
    {
        [Required]
        public int pageNumber { get; set; }
        [Required]
        public int pageSize { get; set; }
    }
}
