namespace UniversityApi.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? Dean { get; set; }

        public ICollection<Lecturer>? Lecturers { get; set; }
    }
}
