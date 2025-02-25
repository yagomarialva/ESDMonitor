using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class FcAreaService
    {
        private IFcAreaRepository _fcAreaRepository;
        public FcAreaService(IFcAreaRepository fcAreaRepository)
        {
            _fcAreaRepository = fcAreaRepository ?? throw new ArgumentNullException(nameof(fcAreaRepository));
        }
        public async Task<FcAreaModel?> GetAreaByUserIdAsync(int userId)
        {
            var result = await _fcAreaRepository.GetByUserIdAsync(userId);
            return result;
        }
        public async Task<FcAreaModel?> AddAreaAsync(FcAreaModel areaModel)
        {
            var result = await _fcAreaRepository.InsertAsync(areaModel);
            return result;
        }
        public async Task<FcAreaModel?> UpdateAreaAsync(FcAreaModel areaModel)
        {
            var result = await _fcAreaRepository.UpdateAsync(areaModel);
            return result;
        }
        public async Task<bool> DeleteAreaAsync(FcAreaModel areaModel)
        {
            var result = await _fcAreaRepository.DeleteAsync(areaModel);
            return result;
        }
    }
}
