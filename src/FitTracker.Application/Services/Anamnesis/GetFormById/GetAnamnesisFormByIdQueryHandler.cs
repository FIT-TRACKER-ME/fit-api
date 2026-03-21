using FitTracker.Domain.Repositories;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Anamnesis.GetFormById;

public class GetAnamnesisFormByIdQueryHandler : IQueryHandler<GetAnamnesisFormByIdQuery, AnamnesisFormResponse>
{
    private readonly IAnamnesisRepository _anamnesisRepository;

    public GetAnamnesisFormByIdQueryHandler(IAnamnesisRepository anamnesisRepository)
    {
        _anamnesisRepository = anamnesisRepository;
    }

    public async Task<Result<AnamnesisFormResponse>> Handle(GetAnamnesisFormByIdQuery request, CancellationToken cancellationToken)
    {
        var form = await _anamnesisRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (form == null) return Result.Failure<AnamnesisFormResponse>(Error.NullValue);

        return Result.Success(new AnamnesisFormResponse
        {
            Id = form.Id,
            Title = form.Title,
            Description = form.Description,
            SchemaJson = form.SchemaJson
        });
    }
}
