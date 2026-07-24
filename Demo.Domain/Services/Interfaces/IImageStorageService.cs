using Microsoft.AspNetCore.Http;

namespace Demo.Domain.Services.Interfaces
{
    public interface IImageStorageService
    {
        Task<string> SaveAsync(IFormFile file);
        Task<List<string>> SaveManyAsync(IEnumerable<IFormFile> files);
        Task DeleteAsync(string imageUrl);
    }
}
