using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface IFcEyeRepository
    {
        Task<FcEyeModel?> GetByUserIdAsync(int userId);
        Task<FcEyeModel> InsertAsync(FcEyeModel entity);
        Task<FcEyeModel?> UpdateAsync(FcEyeModel entity);
        Task<bool> DeleteEyeAsync(FcEyeModel userId);
    }
}
