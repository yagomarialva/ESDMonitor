using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class FcEmbeddingService
    {
        private IFcEmbeddingRepository _fcEmbeddingRepository;
        public FcEmbeddingService(IFcEmbeddingRepository fcEmbeddingRepository)
        {
            _fcEmbeddingRepository = fcEmbeddingRepository ?? throw new ArgumentNullException(nameof(fcEmbeddingRepository));
        }
        public async Task<FcEmbeddingModel?> GetEmbeddingByUserIdAsync(int userId)
        {
            var result = await _fcEmbeddingRepository.GetByUserIdAsync(userId);
            return result;
        }
        public async Task<FcEmbeddingModel?> AddEmbeddingAsync(FcEmbeddingModel embeddingModel)
        {
            var result = await _fcEmbeddingRepository.InsertAsync(embeddingModel);
            return result;
        }
        public async Task<FcEmbeddingModel?> UpdateEmbeddingAsync(FcEmbeddingModel embeddingModel)
        {
            var result = await _fcEmbeddingRepository.UpdateAsync(embeddingModel);
            return result;
        }
        public async Task<bool> DeleteEmbeddingAsync(FcEmbeddingModel embeddingModel)
        {
            var result = await _fcEmbeddingRepository.DeleteEmbeddingAsync(embeddingModel);
            return result;
        }

    }
}
