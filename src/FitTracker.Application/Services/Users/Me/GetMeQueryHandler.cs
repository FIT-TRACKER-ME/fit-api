using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Application.Services.Users.Login;
using FitTracker.Domain.Errors;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;
using FitTracker.Domain.ValueObjects;
using FitTracker.Domain.Entities.Users;

namespace FitTracker.Application.Services.Users.Me
{
    internal sealed class GetMeQueryHandler : IQueryHandler<GetMeQuery, UserResponse>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;
        private readonly IBlobStorageService _blobStorageService;

        public GetMeQueryHandler(IUserContext userContext, IUserRepository userRepository, IBlobStorageService blobStorageService)
        {
            _userContext = userContext;
            _userRepository = userRepository;
            _blobStorageService = blobStorageService;
        }

        public async Task<Result<UserResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContext.UserId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<UserResponse>(DomainErrors.User.NotFound);
            }

            string? avatarUrl = null;
            try 
            {
                avatarUrl = await _blobStorageService.GetDownloadUrlAsync(user.AvatarUrl ?? "", cancellationToken);
            }
            catch (Exception ex)
            {
                // Log error but don't fail the whole request
                Console.WriteLine($"[GetMeQueryHandler] Error fetching avatar URL: {ex.Message}");
            }
            
            return new UserResponse(
                user.Name, 
                user.Id.Value.ToString(), 
                user.Document?.Value ?? "", 
                user.Email.Value, 
                user.Phone, 
                (int)user.Role, 
                (int)user.Status, 
                user.AnamnesisFormId?.ToString(),
                avatarUrl);
        }
    }
}
