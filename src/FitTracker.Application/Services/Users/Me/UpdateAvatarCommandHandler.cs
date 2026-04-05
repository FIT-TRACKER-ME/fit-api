using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Entities.Users;
using FitTracker.Domain.Errors;
using FitTracker.Domain.Repositories;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Users.Me
{
    internal sealed class UpdateAvatarCommandHandler : ICommandHandler<UpdateAvatarCommand, string>
    {
        private readonly IUserContext _userContext;
        private readonly IUserRepository _userRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAvatarCommandHandler(
            IUserContext userContext,
            IUserRepository userRepository,
            IBlobStorageService blobStorageService,
            IUnitOfWork unitOfWork)
        {
            _userContext = userContext;
            _userRepository = userRepository;
            _blobStorageService = blobStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(UpdateAvatarCommand request, CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContext.UserId);
            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return Result.Failure<string>(DomainErrors.User.NotFound);
            }

            // Optional: delete old avatar
            if (!string.IsNullOrEmpty(user.AvatarUrl))
            {
                await _blobStorageService.DeleteFileAsync(user.AvatarUrl, cancellationToken);
            }

            // Upload new avatar to "avatars" folder
            var avatarUrl = await _blobStorageService.UploadFileAsync(
                request.FileStream,
                request.FileName,
                request.ContentType,
                "avatars",
                cancellationToken);

            user.UpdateAvatarUrl(avatarUrl);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Return signed URL
            var signedUrl = await _blobStorageService.GetDownloadUrlAsync(avatarUrl, cancellationToken);

            return Result.Success(signedUrl);
        }
    }
}
