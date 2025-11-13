using AutoMapper;
using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using HealthDesk.Application.DTOs;
using MediatR;

namespace HealthDesk.Application.Features.Prescriptions.Queries
{
    public sealed class GetPrescriptionByPatientHandler : IRequestHandler<GetPrescriptionByPatientQuery, IEnumerable<PrescriptionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUser;
        public GetPrescriptionByPatientHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IEnumerable<PrescriptionDto>> Handle(GetPrescriptionByPatientQuery request, CancellationToken cancellationToken)
        {
            if(_currentUser.UserId is null)
                throw new UnauthorizedAccessException("User not authenticated.");

            if(_currentUser.Role != UserRoles.Patient)
                throw new ForbiddenAccessException();

            var prescriptions = await _unitOfWork.Prescriptions.GetByPatientIdAsync(_currentUser.UserId.Value);
            
            if(prescriptions is null || !prescriptions.Any())
                throw new NotFoundException($"PatientId={_currentUser.UserId.Value}");

            return _mapper.Map<IEnumerable<PrescriptionDto>>(prescriptions);
        }
    }
}