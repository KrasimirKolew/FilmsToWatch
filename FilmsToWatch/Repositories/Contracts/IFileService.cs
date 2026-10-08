namespace FilmsToWatch.Repositories.Contracts
{
    public interface IFileService
    {
        /// <summary>
        /// Saves an uploaded image to wwwroot/Uploads with a unique name.
        /// </summary>
        Task<ImageSaveResult> SaveImageAsync(IFormFile imageFile);

        /// <summary>
        /// Deletes an image from wwwroot/Uploads. Returns false if it did not exist.
        /// </summary>
        bool DeleteImage(string? imageFileName);
    }

    /// <summary>
    /// Result of saving an image: either the new file name, or an error to show the user.
    /// </summary>
    public class ImageSaveResult
    {
        public bool Success { get; private set; }

        public string FileName { get; private set; } = string.Empty;

        public string ErrorMessage { get; private set; } = string.Empty;

        public static ImageSaveResult Ok(string fileName)
            => new ImageSaveResult { Success = true, FileName = fileName };

        public static ImageSaveResult Fail(string errorMessage)
            => new ImageSaveResult { Success = false, ErrorMessage = errorMessage };
    }
}
