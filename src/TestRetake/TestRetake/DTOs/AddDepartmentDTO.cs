using System.ComponentModel.DataAnnotations;

namespace TestRetake.DTOs
{
    public class AddDepartmentDTO
    {
        [Required]
        [StringLength(50)]
        public string DepName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string DepLocation { get; set; } = string.Empty;
    }
}
