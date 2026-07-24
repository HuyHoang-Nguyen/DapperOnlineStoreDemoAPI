using Demo.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Demo.Domain.Services
{
    public class LocalImageStorageService : IImageStorageService
    {
        private readonly IWebHostEnvironment _environmoent;
        private const string UploadFolder = "uploads/products";
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFileSizeBytes = 5 * 1024 * 1024;

        public LocalImageStorageService(IWebHostEnvironment environmoent)
        {
            _environmoent = environmoent;
        }
        public async Task<string> SaveAsync(IFormFile file)
        {
            ValidateFile(file);

            var extension = Path.GetExtension(file.Name).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid()}{extension}";

            var folderPath = Path.Combine(_environmoent.WebRootPath, UploadFolder);
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return $"/{UploadFolder}/{fileName}";
        }
        public async Task<List<string>> SaveManyAsync(IEnumerable<IFormFile> files)
        {
            var urls = new List<string>();
            foreach (var file in files)
            {
                urls.Add(await SaveAsync(file));
            }
            return urls ;
        }
        public Task DeleteAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return Task.CompletedTask;
            }
            var fileName = Path.GetFileName(imageUrl);
            var filePath = Path.Combine(_environmoent.WebRootPath, UploadFolder, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }
        private static void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty");
            }
            if (file.Length > MaxFileSizeBytes)
            {
                throw new ArgumentException("File exceeds max size of 5MB");
            }
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                throw new ArgumentException($"File type {extension} is now allowed.");
            }
        }
    }
}
