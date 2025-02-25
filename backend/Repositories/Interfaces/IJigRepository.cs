using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface responsável por definir as operações CRUD para o modelo Jig.
    /// </summary>
    public interface IJigRepository
    {
        /// <summary>
        /// Busca todas as entradas de Jig no banco de dados.
        /// </summary>
        /// <returns>Uma lista de objetos JigModel.</returns>
        Task<List<JigModel>> GetAllAsync();

        /// <summary>
        /// Busca um Jig específico pelo seu ID.
        /// </summary>
        /// <param name="jigId">ID do Jig.</param>
        /// <returns>O JigModel correspondente ao ID, ou null se não for encontrado.</returns>
        Task<JigModel?> GetByIdAsync(int jigId);

        Task<JigModel?> GetJigBySnAsync(string serialNumber);

        Task<JigModel?> GetJigSerialNumberAsync(string serialNumber);

        /// <summary>
        /// Busca um Jig específico pelo seu nome.
        /// </summary>
        /// <param name="jigName">Nome do Jig.</param>
        /// <returns>O JigModel correspondente ao nome, ou null se não for encontrado.</returns>
        Task<JigModel?> GetByNameAsync(string jigName);

        /// <summary>
        /// Adiciona ou atualiza um Jig no banco de dados.
        /// </summary>
        /// <param name="jig">Objeto JigModel contendo os dados do Jig.</param>
        /// <returns>O objeto JigModel atualizado ou inserido.</returns>
        Task<JigModel?> AddOrUpdateAsync(JigModel jig);

        /// <summary>
        /// Remove um Jig do banco de dados pelo seu ID.
        /// </summary>
        /// <param name="id">ID do Jig a ser removido.</param>
        /// <returns>True se a exclusão for bem-sucedida, false caso contrário.</returns>
        Task<bool> DeleteAsync(int id);
    }
}
