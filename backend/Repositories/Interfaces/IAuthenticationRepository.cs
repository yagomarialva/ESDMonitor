using BiometricFaceApi.Models;
using System.Threading.Tasks;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface IAuthenticationRepository
    {
        /// <summary>
        /// Authenticates a user by login and password.
        /// </summary>
        /// <param name="login">The login of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>An AuthenticationModel if authentication is successful, null otherwise.</returns>
        Task<AuthenticationModel?> AuthenticateUserAsync(string login, string password);

        /// <summary>
        /// Adds a new authentication entry.
        /// </summary>
        /// <param name="userAuth">The AuthenticationModel containing the user's credentials.</param>
        /// <returns>The created AuthenticationModel.</returns>
        Task<AuthenticationModel?> AddAsync(AuthenticationModel userAuth);

        /// <summary>
        /// Updates an existing authentication entry.
        /// </summary>
        /// <param name="userAuth">The updated AuthenticationModel.</param>
        /// <param name="id">The ID of the authentication entry to update.</param>
        /// <returns>The updated AuthenticationModel.</returns>
        Task<AuthenticationModel?> UpdateAsync(AuthenticationModel userAuth, int id);

        /// <summary>
        /// Retrieves an authentication entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the authentication entry.</param>
        /// <returns>An AuthenticationModel if found, null otherwise.</returns>
        Task<AuthenticationModel?> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves an authentication entry by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>An AuthenticationModel if found, null otherwise.</returns>
        Task<AuthenticationModel?> GetByUsernameAsync(string username);

        /// <summary>
        /// Retrieves an authentication entry by badge ID.
        /// </summary>
        /// <param name="badgeId">The badge ID of the user.</param>
        /// <returns>An AuthenticationModel if found, null otherwise.</returns>
        Task<AuthenticationModel?> GetByBadgeAsync(string? badgeId);

        /// <summary>
        /// Deletes an authentication entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the authentication entry to delete.</param>
        /// <returns>The deleted AuthenticationModel if found, null otherwise.</returns>
        Task<AuthenticationModel?> DeleteAsync(int id);
    }
}
