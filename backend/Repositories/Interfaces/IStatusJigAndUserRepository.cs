using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface IStatusJigAndUserRepository
    {
        Task<StatusJigAndUserModel?> GetLatestStatusAsync(int monitorEsdId, int? userId, int? jigId);
        Task <StatusJigAndUserModel?>AddOrUpdateStatusAsync(StatusJigAndUserModel status);
    }
}
