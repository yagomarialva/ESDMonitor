using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface IMonitorEsdRepository
    {
        /// <summary>
        /// Retrieve all MonitorEsd records.
        /// </summary>
        Task<List<MonitorEsdModel>> GetAllMonitorsAsync();

        /// <summary>
        /// Retrieve a MonitorEsd record by its ID.
        /// </summary>
        /// <param name="id">The monitor ID.</param>
        Task<MonitorEsdModel?> GetMonitorByIdAsync(int id);

        /// <summary>
        /// Retrieve a MonitorEsd record by its serial number.
        /// </summary>
        /// <param name="serial">The monitor serial number.</param>
        Task<MonitorEsdModel?> GetMonitorBySerialAsync(string serial);
        //Task<MonitorEsdModel?> GetMonitorByIPAsync(string ip);

        /// <summary>
        /// Retrieve a MonitorEsd record by its logs.
        /// </summary>
        /// <param name="logs">The monitor serial number.</param>
        Task<MonitorEsdModel?> GetLogsAsync(string logs);

        /// <summary>
        /// Retrieve a MonitorEsd record by operator status.
        /// </summary>
        /// <param name="statusOperator">The operator status.</param>
        Task<MonitorEsdModel?> GetByOperatorStatusAsync(string statusOperator);

        /// <summary>
        /// Retrieve a MonitorEsd record by jig status.
        /// </summary>
        /// <param name="statusJig">The jig status.</param>
        Task<MonitorEsdModel?> GetByJigStatusAsync(string statusJig);

        /// <summary>
        /// Retrieve a MonitorEsd record by its overall status.
        /// </summary>
        /// <param name="status">The monitor status.</param>
        Task<MonitorEsdModel?> GetByStatusAsync(string status);

        /// <summary>
        /// Add or update a MonitorEsd record.
        /// </summary>
        /// <param name="monitorModel">The monitor model.</param>
        Task<MonitorEsdModel?> AddOrUpdateAsync(MonitorEsdModel monitorModel);
        /// <summary>
        /// Delete a MonitorEsd record by its ID.
        /// </summary>
        /// <param name="id">The monitor ID.</param>
        Task<MonitorEsdModel> DeleteAsync(int id);
    }
}
