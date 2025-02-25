using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface para o repositório de funções de usuário.
    /// </summary>
    public interface IRolesRepository
    {
        /// <summary>
        /// Obtém todas as funções.
        /// </summary>
        /// <returns>Uma lista de objetos <see cref="RolesModel"/>.</returns>
        Task<IEnumerable<RolesModel>> GetAllRolesAsync();

        /// <summary>
        /// Obtém uma função pelo ID.
        /// </summary>
        /// <param name="id">O ID da função.</param>
        /// <returns>Um objeto <see cref="RolesModel"/> se encontrado; caso contrário, <c>null</c>.</returns>
        Task<RolesModel?> GetRoleByIdAsync(int id);

        /// <summary>
        /// Obtém uma função pelo nome.
        /// </summary>
        /// <param name="roleName">O nome da função.</param>
        /// <returns>Um objeto <see cref="RolesModel"/> se encontrado; caso contrário, <c>null</c>.</returns>
        Task<RolesModel?> GetRoleByNameAsync(string roleName);

        /// <summary>
        /// Adiciona uma nova função.
        /// </summary>
        /// <param name="roleModel">O objeto <see cref="RolesModel"/> a ser adicionado.</param>
        /// <returns>O objeto <see cref="RolesModel"/> adicionado.</returns>
        Task<RolesModel> AddOrUpdateRoleAsync(RolesModel roleModel);

        /// <summary>
        /// Remove uma função pelo ID.
        /// </summary>
        /// <param name="id">O ID da função a ser removida.</param>
        /// <returns>O objeto <see cref="RolesModel"/> removido, se encontrado; caso contrário, <c>null</c>.</returns>
        Task<RolesModel?> DeleteRoleAsync(int id);
    }
}
