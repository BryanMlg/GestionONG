using System.ComponentModel.DataAnnotations;

namespace GestionONG.Dtos
{
    public class ProyectoCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no debe tener más de 100 caracteres.")]
        public string? nombre { get; set; } 
        [Required(ErrorMessage = "El campo idDepartamento es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de idDepartamento no puede ser negativo.")]
        public int? idDepartamento { get; set; }
        [Required(ErrorMessage = "El campo idMunicipio es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de idMunicipio no puede ser negativo.")]
        public int? idMunicipio { get; set; }
        [Required(ErrorMessage = "El campo fechaInicio es obligatorio.")]
        public DateOnly fechaInicio { get; set; }
        [Required(ErrorMessage = "El campo fechaFin es obligatorio.")]
        public DateOnly fechaFin { get; set; }


    }

    public class ProyectoUpdateDto
    {
        [Required]
        public int id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no debe tener más de 100 caracteres.")]
        public string? nombre { get; set; } 
        [Required(ErrorMessage = "El campo idDepartamento es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de idDepartamento no puede ser negativo.")]
        public int? idDepartamento { get; set; }
        [Required(ErrorMessage = "El campo idMunicipio es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de idMunicipio no puede ser negativo.")]
        public int? idMunicipio { get; set; }
        [Required(ErrorMessage = "El campo fechaInicio es obligatorio.")]
        public DateOnly fechaInicio { get; set; }
        [Required(ErrorMessage = "El campo fechaFin es obligatorio.")]
        public DateOnly fechaFin { get; set; }
    }

    public class ProyectoPaginacionDto
    {
        [Required]
        public int pageNumber { get; set; }

        [Required]
        public int pageSize { get; set; }
    }

    public class ProyectoOneDto
    {
        [Required]
        public int id { get; set; }
    }
    public class CalculoProyectoDto
    {
        public int? opcion { get; set; } = 1; 
    }
    public class RubroFinanzasDto
    {
        public int RubroId { get; set; }
        public string NombreRubro { get; set; }
        public decimal TotalDonaciones { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal DisponibilidadFondos { get; set; }
    }


}
