using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Patients.Commands.UpdatePatient
{
    public sealed class UpdatePatientHandler : IRequestHandler<UpdatePatientCommand, PatientDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;

        public UpdatePatientHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PatientDto> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Admin)
                throw new ForbiddenAccessException();

            var patient = await _unitOfWork.Patients.GetByIdWithUserAsync(request.Id)
                ?? throw new NotFoundException(request.Id);

            patient.UpdateFullName(request.FullName);

            await _unitOfWork.Patients.UpdateAsync(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PatientDto>(patient);
        }
    }
}
