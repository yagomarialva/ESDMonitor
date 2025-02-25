using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface para o repositório de links entre estações e linhas.
    /// </summary>
    public interface ILinkStationAndLineRepository
    {
        /// <summary>
        /// Obtém todos os links de estações e linhas.
        /// </summary>
        /// <returns>Uma lista de modelos de links de estações e linhas.</returns>
        Task<List<LinkStationAndLineModel>> GetAllLinksAsync();

        /// <summary>
        /// Obtém um link específico pelo ID.
        /// </summary>
        /// <param name="id">O ID do link a ser buscado.</param>
        /// <returns>O modelo do link correspondente ou null se não encontrado.</returns>
        Task<LinkStationAndLineModel?> GetByLinkIdAsync(int id);

        /// <summary>
        /// Obtém todos os links associados a uma linha específica.
        /// </summary>
        /// <param name="lineId">O ID da linha cujos links devem ser buscados.</param>
        /// <returns>Uma lista de modelos de links associados à linha ou null se não encontrados.</returns>
        Task<List<LinkStationAndLineModel>?> GetByLineIdAsync(int lineId);

        /// <summary>
        /// Obtém todos os links associados a uma estação específica.
        /// </summary>
        /// <param name="stationId">O ID da estação cujos links devem ser buscados.</param>
        /// <returns>Uma lista de modelos de links associados à estação ou null se não encontrados.</returns>
        Task<List<LinkStationAndLineModel>?> GetByStationIdAsync(int stationId);

        /// <summary>
        /// Obtém um link específico com base nos IDs da linha e da estação.
        /// </summary>
        /// <param name="lineId">O ID da linha.</param>
        /// <param name="stationId">O ID da estação.</param>
        /// <returns>O modelo do link correspondente ou null se não encontrado.</returns>
        Task<LinkStationAndLineModel?> GetByLineIdAndStationIdAsync(int lineId, int stationId);

        /// <summary>
        /// Inclui um novo link de estação e linha no repositório.
        /// </summary>
        /// <param name="model">O modelo do link a ser incluído.</param>
        /// <returns>O modelo do link incluído.</returns>
        Task<LinkStationAndLineModel?> IncludeAsync(LinkStationAndLineModel model);

        /// <summary>
        /// Deleta um link específico pelo ID.
        /// </summary>
        /// <param name="id">O ID do link a ser deletado.</param>
        /// <returns>O modelo do link deletado.</returns>
        Task<LinkStationAndLineModel?> DeleteAsync(int id);
    }
}
