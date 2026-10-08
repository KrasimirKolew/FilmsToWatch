using FilmsToWatch.Repositories.Contracts;

namespace FilmsToWatch.Repositories.Services
{
    public class FileService : IFileService
    {
        private const string UploadsFolder = "Uploads";

        // 10 MB
        private const long MaxFileSizeBytes = 10 * 1024 * 1024;

        private static readonly string[] AllowedExtensions =
            { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

        private readonly IWebHostEnvironment environment;
        private readonly ILogger<FileService> logger;

        public FileService(IWebHostEnvironment environment, ILogger<FileService> logger)
        {
            this.environment = environment;
            this.logger = logger;
        }

        public async Task<ImageSaveResult> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return ImageSaveResult.Fail("The selected file is empty.");
            }

            if (imageFile.Length > MaxFileSizeBytes)
            {
                return ImageSaveResult.Fail(
                    $"The image is too large. The maximum size is {MaxFileSizeBytes / (1024 * 1024)} MB.");
            }

            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
            {
                return ImageSaveResult.Fail(
                    $"Only {string.Join(", ", AllowedExtensions)} files are allowed.");
            }

            if (string.IsNullOrEmpty(imageFile.ContentType)
                || !imageFile.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            {
                return ImageSaveResult.Fail("The selected file is not an image.");
            }

            try
            {
                var uploadsPath = GetUploadsPath();
                Directory.CreateDirectory(uploadsPath); // does nothing if it already exists

                var newFileName = Guid.NewGuid().ToString() + extension;
                var fullPath = Path.Combine(uploadsPath, newFileName);

                // Await the copy so the whole file is written before the stream is closed
                await using (var stream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    await imageFile.CopyToAsync(stream);
                }

                return ImageSaveResult.Ok(newFileName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Saving image {FileName} failed", imageFile.FileName);

                // Shown only on the admin film form, so the real reason helps with fixing it
                return ImageSaveResult.Fail($"The image could not be saved: {ex.Message}");
            }
        }

        public bool DeleteImage(string? imageFileName)
        {
            if (string.IsNullOrWhiteSpace(imageFileName))
            {
                return false;
            }

            try
            {
                // Path.GetFileName stops names like "../x" from escaping the Uploads folder
                var fullPath = Path.Combine(GetUploadsPath(), Path.GetFileName(imageFileName));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Deleting image {FileName} failed", imageFileName);
                return false;
            }
        }

        private string GetUploadsPath()
        {
            // WebRootPath is null when the wwwroot folder is missing, so fall back to building it
            var webRoot = environment.WebRootPath
                ?? Path.Combine(environment.ContentRootPath, "wwwroot");

            return Path.Combine(webRoot, UploadsFolder);

        }
    }
}
