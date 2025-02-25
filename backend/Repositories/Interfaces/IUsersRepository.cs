using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface for User repository operations.
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of users.</returns>
        Task<List<UserModel>> GetAllAsync();

        /// <summary>
        /// Retrieves all users asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of users.</returns>
        Task<List<UserModel>> GetListUsersAsync(int page, int pageSize);

        /// <summary>
        /// Retrieves a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>A task representing the asynchronous operation, with the user model if found; otherwise, null.</returns>
        Task<UserModel?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves a user by their badge number asynchronously.
        /// </summary>
        /// <param name="badge">The badge number of the user.</param>
        /// <returns>A task representing the asynchronous operation, with the user model if found; otherwise, null.</returns>
        Task<UserModel?> GetByBadgeAsync(string badge);

        /// <summary>
        /// Retrieves a user by their name asynchronously.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>A task representing the asynchronous operation, with the user model if found; otherwise, null.</returns>
        Task<UserModel?> GetByNameAsync(string name);

        /// <summary>
        /// Adds a new user asynchronously.
        /// </summary>
        /// <param name="user">The user model to add.</param>
        /// <returns>A task representing the asynchronous operation, with the added user model.</returns>
        Task<UserModel> AddAsync(UserModel user);

        /// <summary>
        /// Updates an existing user asynchronously.
        /// </summary>
        /// <param name="user">The user model with updated information.</param>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <returns>A task representing the asynchronous operation, with the updated user model.</returns>
        Task<UserModel?> UpdateAsync(UserModel user, int id);

        /// <summary>
        /// Deletes a user by their unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>A task representing the asynchronous operation, with the deleted user model.</returns>
        Task<UserModel> DeleteAsync(int id);

        Task<int> GetTotalUserCount();


    }
}
