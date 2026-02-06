using HealthDesk.Domain.Common;

namespace HealthDesk.Domain.Entities
{
    public class DoctorAvailability : BaseEntity
    {
        public int DoctorId { get; private set; }
        public Doctor Doctor { get; private set; } = null!;
        public DayOfWeek DayOfWeek { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }

        private DoctorAvailability() { }

        public DoctorAvailability(int doctorId, DayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            if (doctorId <= 0)
                throw new ArgumentOutOfRangeException(nameof(doctorId), "DoctorId must be a positive number.");

            if (endTime <= startTime)
                throw new ArgumentException("EndTime must be after StartTime.", nameof(endTime));

            DoctorId = doctorId;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
