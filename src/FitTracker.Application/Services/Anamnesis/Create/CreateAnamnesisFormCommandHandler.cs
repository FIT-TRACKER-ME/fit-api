using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Anamnesis.GetFormById;
using FitTracker.Domain.Entities.Anamnesis;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Anamnesis.Create;

public class CreateAnamnesisFormCommandHandler : ICommandHandler<CreateAnamnesisFormCommand, AnamnesisFormResponse>
{
    private readonly IAnamnesisRepository _anamnesisRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CreateAnamnesisFormCommandHandler(IAnamnesisRepository anamnesisRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _anamnesisRepository = anamnesisRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<AnamnesisFormResponse>> Handle(CreateAnamnesisFormCommand request, CancellationToken cancellationToken)
    {
        var form = new AnamnesisForm(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.SchemaJson,
            new UserId(_userContext.UserId)
        );

        _anamnesisRepository.Add(form);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new AnamnesisFormResponse
        {
            Id = form.Id,
            Title = form.Title,
            Description = form.Description,
            SchemaJson = form.SchemaJson
        });
    }
}
