using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using HealthDesk.Domain.Entities;
using MediatR;

namespace HealthDesk.Application.Features.Clinics.Commands.CreateClinic
{
    public sealed class CreateClinicHandler : IRequestHandler<CreateClinicCommand, ClinicDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public CreateClinicHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<ClinicDto> Handle(CreateClinicCommand request, CancellationToken cancellationToken)
        {

            if (_currentUser.Role != UserRoles.Admin)
                throw new UnauthorizedAccessException("Only admins can create clinics.");

            var clinic =  new Clinic
            (
                request.Name,
                request.Address,
                request.PhoneNumber,
                request.Specialty
            );

            await _unitOfWork.Clinics.AddAsync(clinic);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ClinicDto>(clinic);
        }
    }
}