using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface responsável por definir as operações CRUD para o modelo Line.
    /// </summary>
    public interface ILineRepository
    {
        /// <summary>
        /// Obtém todas as linhas.
        /// </summary>
        /// <returns>Uma lista de objetos LineModel.</returns>
        Task<List<LineModel>> GetAllAsync();

        /// <summary>
        /// Obtém uma linha específica pelo seu ID.
        /// </summary>
        /// <param name="id">ID da linha.</param>
        /// <returns>O objeto LineModel correspondente ao ID, ou null se não for encontrado.</returns>
        Task<LineModel?> GetByIdAsync(int id);

        /// <summary>
        /// Obtém uma linha específica pelo seu nome.
        /// </summary>
        /// <param name="lineName">Nome da linha.</param>
        /// <returns>O objeto LineModel correspondente ao nome, ou null se não for encontrado.</returns>
        Task<LineModel?> GetByNameAsync(string lineName);

        /// <summary>
        /// Adiciona uma nova linha ao banco de dados.
        /// </summary>
        /// <param name="lineModel">O objeto LineModel a ser inserido.</param>
        /// <returns>O objeto LineModel inserido, ou null se a inserção falhar.</returns>
        Task<LineModel?> AddOrUpdateAsync(LineModel lineModel);

        /// <summary>
        /// Remove uma linha do banco de dados pelo seu ID.
        /// </summary>
        /// <param name="id">ID da linha a ser removida.</param>
        /// <returns>O objeto LineModel removido, ou null se a exclusão falhar.</returns>
        Task<LineModel?> DeleteAsync(int id);
    }
}
