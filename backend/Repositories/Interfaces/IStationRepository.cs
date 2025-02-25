using BiometricFaceApi.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface IStationRepository
    {
        /// <summary>
        /// Retrieves all stations.
        /// </summary>
        Task<List<StationModel>> GetAllAsync();

        /// <summary>
        /// Retrieves a station by its unique identifier.
        /// </summary>
        Task<StationModel> GetByIdAsync(int id);

        /// <summary>
        /// Retrieves a station by its name.
        /// </summary>
        Task<StationModel> GetByNameAsync(string name);

        /// <summary>
        /// Adds a new station.
        /// </summary>
        Task<StationModel?> AddOrUpdateAsync(StationModel stationModel);

        /// <summary>
        /// Deletes a station by its unique identifier.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
