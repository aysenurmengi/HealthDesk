using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using HealthDesk.Application.Features.Patients.Queries;
using MediatR;

namespace HealthDesk.Application.Patients.Queries
{
    public sealed class GetPatientByIdHandler : IRequestHandler<GetPatientByIdQuery, PatientDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        public GetPatientByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<PatientDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Patients.GetByIdAsync(request.Id);

            if (_currentUser.UserId is null)
                throw new UnauthorizedAccessException("User not authenticated.");

            if (_currentUser.Role != UserRoles.Admin && 
                !(_currentUser.Role == UserRoles.Patient && _currentUser.UserId.Value == request.Id))
            {
                throw new ForbiddenAccessException();
            }

            return _mapper.Map<PatientDto>(patient);
        }
    }
}