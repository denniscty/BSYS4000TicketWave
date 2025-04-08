using Microsoft.AspNetCore.Hosting;

namespace TicketWave.Services
{
    public class ImageUploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        public ImageUploadService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<(bool Success, string? FileName, string? ErrorMessage)> UploadAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return (false, null, "No file was uploaded.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!_allowedExtensions.Contains(extension))
                return (false, null, "Only JPG, PNG, GIF, and WEBP image formats are allowed.");

            if (file.Length > _maxFileSize)
                return (false, null, "File size must not exceed 5 MB.");

            var fileName = $"{Guid.NewGuid()}{extension}";
            var savePath = Path.Combine(_env.WebRootPath, "images", "events");

            if (!Directory.Exists(savePath))
                Directory.CreateDirectory(savePath);

            var fullPath = Path.Combine(savePath, fileName);
            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return (true, fileName, null);
        }
    }
}
