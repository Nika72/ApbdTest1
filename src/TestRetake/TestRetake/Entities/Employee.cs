namespace TestRetake.Entities
{
    public class Employee
    {
        public int EmpID { get; set; }
        public string EmpName { get; set; } = string.Empty;
        public string JobName { get; set; } = string.Empty;
        public int? ManagerID { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public decimal? Commission { get; set; }
        public int DepID { get; set; }
    }
}
