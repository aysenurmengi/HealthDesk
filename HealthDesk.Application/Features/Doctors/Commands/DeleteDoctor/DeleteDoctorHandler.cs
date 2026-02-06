using HealthDesk.Application.Common.Constants;
using HealthDesk.Application.Common.Exceptions;
using HealthDesk.Application.Common.Interfaces;
using MediatR;

namespace HealthDesk.Application.Features.Doctors.Commands.DeleteDoctor
{
    public sealed class DeleteDoctorHandler : IRequestHandler<DeleteDoctorCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteDoctorHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUser)
        {
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != UserRoles.Admin)
                throw new ForbiddenAccessException();

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(request.Id);

            await _unitOfWork.Doctors.DeleteAsync(doctor);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
