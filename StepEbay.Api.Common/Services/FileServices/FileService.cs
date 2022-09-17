using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using StepEbay.Common.Helpers;
using StepEbay.Main.Common.Models.Image;

namespace StepEbay.Main.Api.Common.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<ImageResponseDto> SaveImage(IFormFile file)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);

            ImageHelper.SaveToPath(Path.Combine(_environment.WebRootPath, "images", fileName), stream);

            return new ImageResponseDto { FileName = fileName };
        }
    }
}
