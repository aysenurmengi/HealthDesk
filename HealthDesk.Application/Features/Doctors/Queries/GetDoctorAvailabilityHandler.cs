using HealthDesk.Application.Common.Helpers;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Queries
{
    public sealed class GetDoctorAvailabilityHandler : IRequestHandler<GetDoctorAvailabilityQuery, IEnumerable<AvailableSlotDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDoctorAvailabilityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AvailableSlotDto>> Handle(GetDoctorAvailabilityQuery request, CancellationToken cancellationToken)
        {
            var dateUtc = request.Date.Kind switch
            {
                DateTimeKind.Utc => request.Date,
                DateTimeKind.Local => request.Date.ToUniversalTime(),
                _ => DateTime.SpecifyKind(request.Date, DateTimeKind.Utc)
            };

            var day = dateUtc.DayOfWeek;
            var blocks = await _unitOfWork.DoctorAvailabilities.GetByDoctorAndDayAsync(request.DoctorId, day);
            var appointments = await _unitOfWork.Appointments.GetByDoctorAndDateAsync(request.DoctorId, dateUtc);

            var availableStarts = AvailabilitySlotHelper.BuildAvailableSlots(dateUtc, blocks, appointments);
            return availableStarts.Select(start => new AvailableSlotDto
            {
                StartsAt = start,
                EndsAt = start.AddMinutes(30)
            });
        }
    }
}
