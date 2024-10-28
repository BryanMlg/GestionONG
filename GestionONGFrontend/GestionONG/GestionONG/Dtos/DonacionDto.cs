using System.ComponentModel.DataAnnotations;

namespace GestionONG.Dtos
{
    public class DonacionCreateDto
    {

        [Required(ErrorMessage = "El campo idRubro es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El idRubro debe ser un valor positivo.")]
        public int idRubro { get; set; }
        [Required(ErrorMessage = "El campo idProyecto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El idProyecto debe ser un valor positivo.")]
        public int idProyecto { get; set; }

        [Required(ErrorMessage = "El campo fechaDonacion es obligatorio.")]
        public DateOnly fechaDonacion { get; set; }

        [Required(ErrorMessage = "El campo nombreDonante es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombreDonante no puede exceder los 100 caracteres.")]
        public string? nombreDonante { get; set; }

        [Required(ErrorMessage = "El campo monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal monto { get; set; }
    }

    public class DonacionUpdateDto
    {
        [Required(ErrorMessage = "El campo id es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El id debe ser un valor positivo.")]
        public int id { get; set; }

        [Required(ErrorMessage = "El campo idProyecto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El idProyecto debe ser un valor positivo.")]
        public int idProyecto { get; set; }

        [Required(ErrorMessage = "El campo idRubro es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El idRubro debe ser un valor positivo.")]
        public int idRubro { get; set; }

        [Required(ErrorMessage = "El campo fechaDonacion es obligatorio.")]
        public DateOnly fechaDonacion { get; set; }

        [Required(ErrorMessage = "El campo nombreDonante es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombreDonante no puede exceder los 100 caracteres.")]
        public string? nombreDonante { get; set; }

        [Required(ErrorMessage = "El campo monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal monto { get; set; }
    }

    public class DonacionOneDto
    {
        public int id { get; set; }
    }

    public class DonacionPaginacionDto
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
