using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Servixa.Shared.Commen.Responses;

namespace Servixa.Abstractions.Interfaces
{
    public interface IFileService
    {
        Task<ApiResponse<string>> UploadFileAsync(IFormFile file, string directory);
        Task<ApiResponse<bool>> DeleteFileAsync(string fileUrl);
        string GetFileUrl(string fileName, string directory);
    }
}
