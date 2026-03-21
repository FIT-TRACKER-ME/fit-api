using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Anamnesis.GetFormById;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Anamnesis.GetByPersonal;

public class GetAnamnesisFormsByPersonalQueryHandler : IQueryHandler<GetAnamnesisFormsByPersonalQuery, List<AnamnesisFormResponse>>
{
    private readonly IAnamnesisRepository _anamnesisRepository;
    private readonly IUserContext _userContext;

    public GetAnamnesisFormsByPersonalQueryHandler(IAnamnesisRepository anamnesisRepository, IUserContext userContext)
    {
        _anamnesisRepository = anamnesisRepository;
        _userContext = userContext;
    }

    public async Task<Result<List<AnamnesisFormResponse>>> Handle(GetAnamnesisFormsByPersonalQuery request, CancellationToken cancellationToken)
    {
        var forms = await _anamnesisRepository.GetByPersonalIdAsync(new UserId(_userContext.UserId), cancellationToken);

        var response = forms.Select(form => new AnamnesisFormResponse
        {
            Id = form.Id,
            Title = form.Title,
            Description = form.Description,
            SchemaJson = form.SchemaJson
        }).ToList();

        return Result.Success(response);
    }
}
