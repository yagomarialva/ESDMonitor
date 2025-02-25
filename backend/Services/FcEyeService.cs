using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class FcEyeService
    {
        private IFcEyeRepository _fcEyeRepository;
        public FcEyeService(IFcEyeRepository fcEyeRepository)
        {
            _fcEyeRepository = fcEyeRepository ?? throw new ArgumentNullException(nameof(fcEyeRepository));
        }
        public async Task<FcEyeModel?> GetEyeByUserIdAsync(int userId)
        {
            var result = await _fcEyeRepository.GetByUserIdAsync(userId);
            return result;
        }
        public async Task<FcEyeModel?> AddEyeAsync(FcEyeModel eyeModel)
        {
            var result = await _fcEyeRepository.InsertAsync(eyeModel);
            return result;
        }
        public async Task<FcEyeModel?> UpdateEyeAsync(FcEyeModel eyeModel)
        {
            var result = await _fcEyeRepository.UpdateAsync(eyeModel);
            return result;
        }
        public async Task<bool> DeleteEyeAsync(FcEyeModel eyeModel)
        {
            var result = await _fcEyeRepository.DeleteEyeAsync(eyeModel);
            return result;
        }
    }
}
