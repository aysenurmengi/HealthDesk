using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Clinics.Queries
{
    public sealed class GetClinicByIdHandler : IRequestHandler<GetClinicByIdQuery, ClinicDto>
    {
        public readonly IUnitOfWork _unitOfWork;
        public readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        public GetClinicByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;

        }
        public async Task<ClinicDto> Handle(GetClinicByIdQuery request, CancellationToken cancellationToken)
        {
            var clinic = await _unitOfWork.Clinics.GetByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Clinic with ID {request.Id} not found.");

            if (_currentUser.Role is null)
                throw new UnauthorizedAccessException("User not authenticated.");

            if (_currentUser.Role == UserRoles.Doctor)
            {
                var doctor = await _unitOfWork.Doctors.GetByIdAsync(_currentUser.UserId!.Value);
                if (doctor.ClinicId != clinic.Id)
                    throw new UnauthorizedAccessException("You are not authorized to view this clinic.");
            }
            return _mapper.Map<ClinicDto>(clinic);
        }
    }
}