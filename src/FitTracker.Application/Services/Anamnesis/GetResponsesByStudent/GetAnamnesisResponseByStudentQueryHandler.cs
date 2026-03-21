using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Anamnesis.GetResponsesByStudent
{
    internal sealed class GetAnamnesisResponseByStudentQueryHandler : IQueryHandler<GetAnamnesisResponseByStudentQuery, List<AnamnesisResponseDto>>
    {
        private readonly IAnamnesisRepository _anamnesisRepository;
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;

        public GetAnamnesisResponseByStudentQueryHandler(IAnamnesisRepository anamnesisRepository, IUserContext userContext, IUserRepository userRepository)
        {
            _anamnesisRepository = anamnesisRepository;
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public async Task<Result<List<AnamnesisResponseDto>>> Handle(GetAnamnesisResponseByStudentQuery request, CancellationToken cancellationToken)
        {
            if (_userContext.Role == FitTracker.Domain.Enums.UserRole.Student)
            {
                if (_userContext.UserId != request.StudentId)
                {
                    return Result.Failure<List<AnamnesisResponseDto>>(new Error("Unauthorized", "Você não tem permissão para acessar as respostas deste aluno."));
                }
            }
            else if (_userContext.Role == FitTracker.Domain.Enums.UserRole.Personal)
            {
                var students = await _userRepository.GetStudentsByPersonalIdAsync(new UserId(_userContext.UserId), cancellationToken);
                if (!students.Any(s => s.Id.Value == request.StudentId))
                {
                    return Result.Failure<List<AnamnesisResponseDto>>(new Error("Unauthorized", "Você não tem permissão para acessar as respostas deste aluno."));
                }
            }

            var responses = await _anamnesisRepository.GetResponsesByStudentIdAsync(new UserId(request.StudentId), cancellationToken);

            var result = new List<AnamnesisResponseDto>();

            foreach (var response in responses)
            {
                var form = await _anamnesisRepository.GetByIdAsync(response.AnamnesisFormId, cancellationToken);
                var jsonResponse = response.ResponsesJson;

                if (form != null && !string.IsNullOrEmpty(form.SchemaJson))
                {
                    try
                    {
                        var schema = System.Text.Json.JsonDocument.Parse(form.SchemaJson);
                        var questions = schema.RootElement.GetProperty("questions").EnumerateArray()
                            .ToDictionary(q => q.GetProperty("id").GetString()!, q => q.GetProperty("label").GetString()!);

                        var answers = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(response.ResponsesJson);
                        if (answers != null)
                        {
                            var hydratedAnswers = new Dictionary<string, string>();
                            foreach (var kvp in answers)
                            {
                                // If the key is an ID found in the schema, use the label instead
                                if (questions.TryGetValue(kvp.Key, out var label))
                                {
                                    hydratedAnswers[label] = kvp.Value;
                                }
                                else
                                {
                                    hydratedAnswers[kvp.Key] = kvp.Value;
                                }
                            }
                            jsonResponse = System.Text.Json.JsonSerializer.Serialize(hydratedAnswers);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Fallback to original JSON if something fails
                        Console.WriteLine($"Error hydrating anamnesis response: {ex.Message}");
                    }
                }

                result.Add(new AnamnesisResponseDto(
                    response.Id,
                    response.AnamnesisFormId,
                    form?.Title ?? "Formulário Desconhecido",
                    jsonResponse,
                    response.CreatedAt));
            }

            return result;
        }
    }
}
