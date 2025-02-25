using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface IFcAreaRepository
    {
        Task<FcAreaModel?> GetByUserIdAsync(int userId);
        Task<FcAreaModel> InsertAsync(FcAreaModel entity);
        Task<FcAreaModel?> UpdateAsync(FcAreaModel entity);
        Task<bool> DeleteAsync(FcAreaModel userId);
    }
}
