using System.ComponentModel.DataAnnotations;

namespace GestionONG.Dtos
{
    public class ProyectoRubroCreateDto
    {
        [Required(ErrorMessage = "El campo idRubro es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de idRubro no puede ser negativo.")]
        public int? idRubro { get; set; }
        [Required(ErrorMessage = "El campo idProyecto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de idProyecto no puede ser negativo.")]
        public int? idProyecto { get; set; }
    }

    public class ProyectoRubroUpdateDto
    {
        [Required]
        public int id { get; set; }
        [Required(ErrorMessage = "El campo idRubro es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de idRubro no puede ser negativo.")]
        public int? idRubro { get; set; }
        [Required(ErrorMessage = "El campo idProyecto es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor de idProyecto no puede ser negativo.")]
        public int? idProyecto { get; set; }
    }

    public class ProyectoRubroOneDto
    {
        public int id { get; set; }
    }

    public class ProyectoRubroPaginacionDto
    {
        [Required]
        public int id { get; set; }
        [Required]
        public int pageNumber { get; set; }
        [Required]
        public int pageSize { get; set; }
    }
}
