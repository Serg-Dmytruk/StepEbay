using Microsoft.AspNetCore.Http;
using StepEbay.Main.Common.Models.Image;

namespace StepEbay.Main.Api.Common.Services.FileServices
{
    public interface IFileService
    {
        Task<ImageResponseDto> SaveImage(IFormFile file);
    }
}
