using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    /// <summary>
    /// Interface para operações relacionadas à entidade StationView.
    /// </summary>
    public interface IStationViewRepository
    {
        /// <summary>
        /// Obtém todas as StationViews disponíveis.
        /// </summary>
        /// <returns>Uma lista de StationViewModel.</returns>
        Task<List<StationViewModel>> GetAllStationViewsAsync();

        /// <summary>
        /// Obtém uma StationView específica pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da StationView.</param>
        /// <returns>O modelo StationViewModel correspondente ou null se não for encontrado.</returns>
        Task<StationViewModel?> GetStationViewByIdAsync(int id);

        Task<StationViewModel?> GetStationViewByLinkStAndLineByIdAsync(int linkStationAndLineId, int seguenceNumber);
        Task<StationViewModel?> DeleteMonitorEsdByStationView(int id);

        /// <summary>
        /// Obtém uma StationView específica pela posição sequencial.
        /// </summary>
        /// <param name="id">O ID da sequência de posição.</param>
        /// <returns>O modelo StationViewModel correspondente ou null se não for encontrado.</returns>
        Task<StationViewModel?> GetByPositionSequenceIdAsync(int id);

        /// <summary>
        /// Inclui uma nova StationView.
        /// </summary>
        /// <param name="stationViewModel">O modelo StationViewModel a ser incluído.</param>
        /// <returns>O modelo StationViewModel inserido.</returns>
        Task<StationViewModel?> AddOrUpdateAsync(StationViewModel stationViewModel);

        /// <summary>
        /// Remove uma StationView pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da StationView a ser removida.</param>
        /// <returns>O modelo StationViewModel removido.</returns>
        Task<StationViewModel> DeleteAsync(int id);
    }
}
