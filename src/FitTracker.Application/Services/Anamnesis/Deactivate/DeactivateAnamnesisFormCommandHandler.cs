using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Anamnesis.Deactivate
{
    internal sealed class DeactivateAnamnesisFormCommandHandler : ICommandHandler<DeactivateAnamnesisFormCommand>
    {
        private readonly IAnamnesisRepository _anamnesisRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateAnamnesisFormCommandHandler(IAnamnesisRepository anamnesisRepository, IUnitOfWork unitOfWork)
        {
            _anamnesisRepository = anamnesisRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeactivateAnamnesisFormCommand request, CancellationToken cancellationToken)
        {
            var form = await _anamnesisRepository.GetByIdAsync(request.FormId, cancellationToken);

            if (form is null)
            {
                return Result.Failure(new Error("AnamnesisForm.NotFound", "Formulário não encontrado"));
            }

            form.Deactivate();
            _anamnesisRepository.Update(form);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
