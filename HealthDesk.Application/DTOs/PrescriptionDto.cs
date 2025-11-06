namespace HealthDesk.Application.DTOs
{
    public class PrescriptionDto
    {
        public int Id { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}