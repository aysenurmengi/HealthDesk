namespace HealthDesk.Application.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartsAt { get; set; }  
        public string? Notes { get; set; }
    }
}