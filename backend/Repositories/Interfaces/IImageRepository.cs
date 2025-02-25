using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface for image repository operations.
    /// </summary>
    public interface IImageRepository
    {
        /// <summary>
        /// Retrieves all images asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of images.</returns>
        Task<List<ImageModel?>> GetAllImagesAsync();

        /// <summary>
        /// Retrieves an image by its unique identifier asynchronously.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image.</param>
        /// <returns>A task representing the asynchronous operation, with the image model.</returns>
        Task<ImageModel?> GetImageByIdAsync(int imageId);
        Task<ImageModel?> GetByImageAsync(string img);
        Task<ImageModel?> GetByEmbeddingAsync(string embedding);

        /// <summary>
        /// Retrieves an image by the associated user's unique identifier asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task representing the asynchronous operation, with the image model.</returns>
        Task<ImageModel?> GetImageByUserIdAsync(int userId);

        /// <summary>
        /// Adds a new image asynchronously.
        /// </summary>
        /// <param name="image">The image model to add.</param>
        /// <returns>A task representing the asynchronous operation, with the added image model.</returns>
        Task<ImageModel?> AddImageAsync(ImageModel image);

        /// <summary>
        /// Updates an existing image asynchronously.
        /// </summary>
        /// <param name="image">The image model with updated information.</param>
        /// <returns>A task representing the asynchronous operation, with the updated image model, or null if not found.</returns>
        Task<ImageModel?> UpdateImageAsync(ImageModel image);

        /// <summary>
        /// Deletes an image by its unique identifier asynchronously.
        /// </summary>
        /// <param name="imageId">The unique identifier of the image to delete.</param>
        /// <returns>A task representing the asynchronous operation, indicating whether the deletion was successful.</returns>
        Task<bool> DeleteImageAsync(int imageId);

        Task<List<ImageModel?>> GetListImagesAsync(int page, int pageSize);
    }
}
