using System.ComponentModel.DataAnnotations;

namespace GestionONG.Dtos
{
    public class RubroDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "El nombre no debe tener más de 100 caracteres.")]
        public string? nombre { get; set; }
    }

    public class RubroUpdateDto
    {
        [Required]
        public int id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "El nombre no debe tener más de 50 caracteres.")]
        public string? nombre { get; set; }
    }

    public class RubroOneDto
    {
        public int id { get; set; }
    }

    public class RubroPaginacionDto
    {
        [Required]
        public int pageNumber { get; set; }
        [Required]
        public int pageSize { get; set; }
    }
}
