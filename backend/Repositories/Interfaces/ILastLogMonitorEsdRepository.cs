using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface ILastLogMonitorEsdRepository
    {
        Task<List<LastLogMonitorEsdModel>> GetAllLastLogsAsync();
        Task<LastLogMonitorEsdModel?> GetLastLogForCreatedAsync( LastLogMonitorEsdModel date);
        Task<LastLogMonitorEsdModel?> GetByIdLastLogsAsync(int id);
        Task<LastLogMonitorEsdModel?> GetByIdLastLogsMonitorEsdAsync(int monitorEsd, int jig);
        Task<LastLogMonitorEsdModel?> GetMessageTypeAsync(string serialNumberEsp, string messageType);
        Task<LastLogMonitorEsdModel?> GetMonitorEsdByIdLastLogsAsync(int id);
        Task<LastLogMonitorEsdModel?> GetJigByIdLastLogsAsync(int jigId);
        Task<LastLogMonitorEsdModel?> GetMonitorEsdBySnLastLogsAsync(string serialNumber);
        Task<LastLogMonitorEsdModel?> InsertLastLogAsync(LastLogMonitorEsdModel model);
        Task<LastLogMonitorEsdModel?> UpdateLastLog(LastLogMonitorEsdModel model, string serialNumber);
    }
}
