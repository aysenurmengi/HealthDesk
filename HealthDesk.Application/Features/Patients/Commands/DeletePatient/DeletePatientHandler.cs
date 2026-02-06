using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using MediatR;

namespace HealthDesk.Application.Features.Patients.Commands.DeletePatient
{
    public sealed class DeletePatientHandler : IRequestHandler<DeletePatientCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeletePatientHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Admin)
                throw new ForbiddenAccessException();

            var patient = await _unitOfWork.Patients.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(request.Id);

            await _unitOfWork.Patients.DeleteAsync(patient);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
