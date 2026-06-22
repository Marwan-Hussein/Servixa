using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Servixa.Abstractions.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Services.Implementations
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<ApiResponse<string>> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                return new ApiResponse<string>("No file", 400);

            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var uploadsFolder = Path.Combine(webRoot, "uploads", folderName);
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return new ApiResponse<string>($"/uploads/{folderName}/{uniqueFileName}", "Ok");
        }

        public Task<ApiResponse<bool>> DeleteFileAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return Task.FromResult(new ApiResponse<bool>(false, "Invalid url", 400));

            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var filePath = Path.Combine(webRoot, fileUrl.TrimStart('/'));
            filePath = filePath.Replace("/", "\\");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(new ApiResponse<bool>(true, "Deleted"));
            }

            return Task.FromResult(new ApiResponse<bool>(false, "Not found", 404));
        }

        public string GetFileUrl(string fileName, string directory)
        {
            return $"/uploads/{directory}/{fileName}";
        }
    }
}
