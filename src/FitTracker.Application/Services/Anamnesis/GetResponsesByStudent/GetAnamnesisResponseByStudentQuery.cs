using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Anamnesis.GetResponsesByStudent
{
    public record GetAnamnesisResponseByStudentQuery(Guid StudentId) : IQuery<List<AnamnesisResponseDto>>;

    public record AnamnesisResponseDto(
        Guid Id,
        Guid FormId,
        string FormTitle,
        string ResponsesJson,
        DateTime CreatedAt);
}
