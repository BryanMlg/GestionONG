using System.ComponentModel.DataAnnotations;

namespace GestionONG.Dtos
{
    public class DetalleOrdenCompraCreateDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "El nombreProducto no debe tener más de 100 caracteres.")]
        public string? nombreProducto { get; set; }
        [Required(ErrorMessage = "El campo idOrdenCompra es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El idOrdenCompra debe ser un valor positivo.")]
        public int idOrdenCompra { get; set; }

        [Required(ErrorMessage = "El campo idRubro es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El idRubro debe ser un valor positivo.")]
        public int idRubro { get; set; }

        [Required(ErrorMessage = "El campo monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal monto { get; set; }
    }

    public class DetalleOrdenCompraUpdateDto
    {
        [Required(ErrorMessage = "El campo id es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El id debe ser un valor positivo.")]
        public int id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "El nombreProducto no debe tener más de 100 caracteres.")]
        public string? nombreProducto { get; set; }

        [Required(ErrorMessage = "El campo idOrdenCompra es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El idOrdenCompra debe ser un valor positivo.")]
        public int idOrdenCompra { get; set; }

        [Required(ErrorMessage = "El campo idRubro es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El idRubro debe ser un valor positivo.")]
        public int idRubro { get; set; }

        [Required(ErrorMessage = "El campo monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal monto { get; set; }
    }

    public class DetalleOrdenCompraOneDto
    {
        [Required(ErrorMessage = "El campo id es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El id debe ser un valor positivo.")]
        public int id { get; set; }
    }

    public class DetalleOrdenCompraPaginacionDto
    {
        [Required(ErrorMessage = "El campo pageNumber es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El pageNumber debe ser un valor positivo.")]
        public int pageNumber { get; set; }

        [Required(ErrorMessage = "El campo pageSize es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El pageSize debe ser un valor positivo y no mayor a 100.")]
        public int pageSize { get; set; }
    }
}
