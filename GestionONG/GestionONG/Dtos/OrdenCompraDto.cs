using System.ComponentModel.DataAnnotations;

namespace GestionONG.Dtos
{
    public class OrdenCompraCreateDto
    {
        [Required(ErrorMessage = "El campo idProyecto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El idProyecto debe ser un valor positivo.")]
        public int idProyecto { get; set; }

        [Required(ErrorMessage = "El campo proveedor es obligatorio.")]
        [StringLength(100, ErrorMessage = "El proveedor no puede exceder los 100 caracteres.")]
        public string? proveedor { get; set; }

        [Required(ErrorMessage = "El campo fechaorden es obligatorio.")]
        public DateOnly fechaOrden { get; set; }

        [Required(ErrorMessage = "El campo montoTotal es obligatorio.")]
        public decimal montoTotal { get; set; }
    }

    public class OrdenCompraUpdateDto
    {
        [Required(ErrorMessage = "El campo id es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El id debe ser un valor positivo.")]
        public int id { get; set; }

        [Required(ErrorMessage = "El campo idProyecto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El idProyecto debe ser un valor positivo.")]
        public int idProyecto { get; set; }

        [Required(ErrorMessage = "El campo proveedor es obligatorio.")]
        [StringLength(100, ErrorMessage = "El proveedor no puede exceder los 100 caracteres.")]
        public string? proveedor { get; set; }

        [Required(ErrorMessage = "El campo fechaorden es obligatorio.")]
        public DateOnly fechaOrden { get; set; }

        [Required(ErrorMessage = "El campo montoTotal es obligatorio.")]
        public decimal montoTotal { get; set; }
    }

    public class OrdenCompraOneDto
    {
        [Required(ErrorMessage = "El campo id es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El id debe ser un valor positivo.")]
        public int id { get; set; }
    }

    public class OrdenCompraPaginacionDto
    {
        [Required(ErrorMessage = "El campo pagenumber es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El pagenumber debe ser un valor positivo.")]
        public int pageNumber { get; set; }

        [Required(ErrorMessage = "El campo pagesize es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El pagesize debe ser un valor positivo y no mayor a 100.")]
        public int pageSize { get; set; }
    }
}
