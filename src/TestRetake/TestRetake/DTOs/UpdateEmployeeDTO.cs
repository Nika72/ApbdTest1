using System.ComponentModel.DataAnnotations;

namespace TestRetake.DTOs{
    public class UpdateEmployeeDTO
    {
        [Required]
        public int EmpID { get; set; }

        [Required]
        [StringLength(100)]
        public string EmpName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string JobName { get; set; } = string.Empty;

        public int? ManagerID { get; set; }

        [Required]
        public decimal Salary { get; set; }

        public decimal? Commission { get; set; }

        [Required]
        public int DepID { get; set; }
    }
}
