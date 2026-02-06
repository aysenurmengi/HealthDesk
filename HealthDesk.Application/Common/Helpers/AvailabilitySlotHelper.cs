using HealthDesk.Domain.Entities;

namespace HealthDesk.Application.Common.Helpers
{
    public static class AvailabilitySlotHelper
    {
        private static readonly TimeSpan SlotDuration = new(0, 30, 0);
        private static readonly TimeSpan SlotBreak = new(0, 10, 0);
        private static readonly TimeSpan LunchStart = new(12, 0, 0);
        private static readonly TimeSpan LunchEnd = new(13, 0, 0);
        private static readonly TimeSpan ClosingTime = new(17, 0, 0);

        public static IReadOnlyCollection<DateTime> BuildAvailableSlots(
            DateTime date,
            IEnumerable<DoctorAvailability> blocks,
            IEnumerable<Appointment> appointments)
        {
            var bookedTimes = new HashSet<TimeSpan>(appointments.Select(a => a.StartsAt.TimeOfDay));
            var slots = new List<DateTime>();

            foreach (var block in blocks)
            {
                var blockStart = block.StartTime;
                var blockEnd = block.EndTime > ClosingTime ? ClosingTime : block.EndTime;

                var cursor = blockStart;
                while (cursor + SlotDuration <= blockEnd)
                {
                    var slotEnd = cursor + SlotDuration;
                    var overlapsLunch = cursor < LunchEnd && slotEnd > LunchStart;

                    if (!overlapsLunch && !bookedTimes.Contains(cursor))
                    {
                        slots.Add(date.Date + cursor);
                    }

                    cursor = cursor + SlotDuration + SlotBreak;
                }
            }

            return slots;
        }
    }
}
