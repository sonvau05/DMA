namespace UniversityApi.Models
{
    public class Lecturer
    {
        public int LecturerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
