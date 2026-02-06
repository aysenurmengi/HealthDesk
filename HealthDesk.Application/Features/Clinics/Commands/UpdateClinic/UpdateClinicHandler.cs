using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Clinics.Commands.UpdateClinic
{
    public sealed class UpdateClinicHandler : IRequestHandler<UpdateClinicCommand, ClinicDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public UpdateClinicHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<ClinicDto> Handle(UpdateClinicCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Admin)
                throw new ForbiddenAccessException();

            var clinic = await _unitOfWork.Clinics.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(request.Id);

            clinic.UpdateDetails(request.Name, request.City, request.District, request.Address, request.PhoneNumber, request.Specialty);

            await _unitOfWork.Clinics.UpdateAsync(clinic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ClinicDto>(clinic);
        }
    }
}
