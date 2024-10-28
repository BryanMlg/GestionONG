using System.ComponentModel.DataAnnotations;

namespace GestionONG.Dtos
{
    public class DepartamentoCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no debe tener más de 100 caracteres.")]
        public string? nombre { get; set; } 

    }

    public class DepartamentoUpdateDto
    {
        [Required]
        public int id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "El nombre no debe tener más de 100 caracteres.")]
        public string? nombre { get; set; }
    }

    public class DepartamentoPaginacionDto
    {
        [Required]
        public int pageNumber { get; set; }

        [Required]
        public int pageSize { get; set; }
    }

    public class DepartamentoMunicipioDto
    {
        [Required]
        public int pageNumber { get; set; }

        [Required]
        public int pageSize { get; set; }

        [Required]
        public int id { get; set; }
    }

    public class DepartamentoOneDto
    {
        [Required]
        public int id { get; set; }
    }
}
