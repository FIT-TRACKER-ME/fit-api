using FitTracker.Application.Abstractions;
using FitTracker.Application.Abstractions.Messaging;
using FitTracker.Domain.Shared;

namespace FitTracker.Application.Services.Workouts.UploadVideo
{
    internal sealed class UploadWorkoutVideoCommandHandler : ICommandHandler<UploadWorkoutVideoCommand, UploadVideoResponse>
    {
        private readonly IBlobStorageService _blobStorageService;

        public UploadWorkoutVideoCommandHandler(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<Result<UploadVideoResponse>> Handle(UploadWorkoutVideoCommand request, CancellationToken cancellationToken)
        {
            var url = await _blobStorageService.UploadFileAsync(
                request.FileStream, 
                request.FileName, 
                request.ContentType,
                cancellationToken: cancellationToken);

            return new UploadVideoResponse(url);
        }
    }
}
