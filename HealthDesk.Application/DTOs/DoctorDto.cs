namespace HealthDesk.Application.DTOs
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string ClinicName { get; set; } = string.Empty;
        public string Specialty { get; set; } = string.Empty;

    }
}