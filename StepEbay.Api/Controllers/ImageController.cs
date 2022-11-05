using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepEbay.Main.Api.Common.Services.FileServices;
using StepEbay.Main.Common.Models.Image;
using Swashbuckle.AspNetCore.Annotations;

namespace StepEbay.Main.Api.Controllers
{
    [Authorize]
    [Route("image")]
    public class ImageController : ControllerBase
    {
        private readonly IFileService _fileService;
        public ImageController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload")]
        [SwaggerResponse(200, Type = typeof(ImageResponseDto))]
        public async Task<ImageResponseDto> UploadImageTemp(int number, IFormFile file)
        {
            return await _fileService.SaveImage(file);
        }
    }
}
