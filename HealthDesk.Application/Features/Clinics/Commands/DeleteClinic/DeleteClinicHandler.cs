using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using MediatR;

namespace HealthDesk.Application.Features.Clinics.Commands.DeleteClinic
{
    public sealed class DeleteClinicHandler : IRequestHandler<DeleteClinicCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteClinicHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task Handle(DeleteClinicCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Admin)
                throw new ForbiddenAccessException();

            var clinic = await _unitOfWork.Clinics.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(request.Id);

            await _unitOfWork.Clinics.DeleteAsync(clinic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
