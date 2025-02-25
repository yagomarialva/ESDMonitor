using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface IFcEmbeddingRepository
    {
        Task<FcEmbeddingModel?> GetByUserIdAsync(int userId);
        Task <FcEmbeddingModel> InsertAsync(FcEmbeddingModel entity);
        Task <FcEmbeddingModel?> UpdateAsync(FcEmbeddingModel entity);
        Task <bool> DeleteEmbeddingAsync(FcEmbeddingModel userId);
    }
}
