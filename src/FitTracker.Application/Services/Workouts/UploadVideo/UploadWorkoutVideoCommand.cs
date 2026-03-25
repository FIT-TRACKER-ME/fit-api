using FitTracker.Application.Abstractions.Messaging;

namespace FitTracker.Application.Services.Workouts.UploadVideo
{
    public record UploadWorkoutVideoCommand(
        Stream FileStream, 
        string FileName, 
        string ContentType) : ICommand<UploadVideoResponse>;

    public record UploadVideoResponse(string Url);
}
