using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Domain.Entities;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Commands.AddDoctorAvailability
{
    public sealed class AddDoctorAvailabilityHandler : IRequestHandler<AddDoctorAvailabilityCommand>
    {
        private static readonly TimeSpan LunchStart = new(12, 0, 0);
        private static readonly TimeSpan LunchEnd = new(13, 0, 0);
        private static readonly TimeSpan ClosingTime = new(17, 0, 0);

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public AddDoctorAvailabilityHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task Handle(AddDoctorAvailabilityCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Admin && _currentUser.Role != UserRoles.Doctor)
                throw new ForbiddenAccessException();

            if (request.EndTime > ClosingTime)
                throw new InvalidOperationException("Availability cannot extend beyond 17:00.");

            if (request.StartTime < LunchEnd && request.EndTime > LunchStart)
                throw new InvalidOperationException("Availability cannot overlap 12:00-13:00.");

            if (_currentUser.Role == UserRoles.Doctor)
            {
                var doctor = await _unitOfWork.Doctors.GetByUserIdAsync(_currentUser.UserId!.Value)
                    ?? throw new ForbiddenAccessException();

                if (doctor.Id != request.DoctorId)
                    throw new ForbiddenAccessException();
            }

            var existing = await _unitOfWork.DoctorAvailabilities
                .GetByDoctorAndDayAsync(request.DoctorId, request.DayOfWeek);

            var overlaps = existing.Any(a =>
                request.StartTime < a.EndTime && request.EndTime > a.StartTime);

            if (overlaps)
                throw new InvalidOperationException("Availability overlaps with existing schedule.");

            var availability = new DoctorAvailability(
                request.DoctorId,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime
            );

            await _unitOfWork.DoctorAvailabilities.AddAsync(availability);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
