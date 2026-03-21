using MediatR;
using FitTracker.Domain.Entities.Anamnesis;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Anamnesis.SubmitResponse;

public class SubmitAnamnesisResponseCommandHandler : ICommandHandler<SubmitAnamnesisResponseCommand>
{
    private readonly IAnamnesisResponseRepository _responseRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitAnamnesisResponseCommandHandler(
        IAnamnesisResponseRepository responseRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _responseRepository = responseRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SubmitAnamnesisResponseCommand request, CancellationToken cancellationToken)
    {
        var studentId = new UserId(request.StudentId);
        
        var response = new AnamnesisResponse(
            Guid.NewGuid(),
            request.FormId,
            studentId,
            request.AnswersJson
        );

        // Also clear the pending AnamnesisFormId from the user
        var student = await _userRepository.GetByIdAsync(studentId, cancellationToken);
        if (student != null)
        {
            student.CompleteAnamnesis();
        }

        _responseRepository.Add(response);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
