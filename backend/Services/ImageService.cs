using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class ImageService
    {
        private IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository ?? throw new ArgumentNullException(nameof(imageRepository));
        }

        // Retorna todas as imagens
        public async Task<List<ImageModel?>> GetAllImagesAsync()
        {
            var result = await _imageRepository.GetAllImagesAsync();
            return result;
        }

        public async Task<List<ImageModel?>> GetListImagesAsync(int page, int pageSize)
        {
            var result = await _imageRepository.GetListImagesAsync(page, pageSize);
            return result;
        }
        // Retorna a imagem pelo ID
        public async Task<ImageModel?> GetImageByIdAsync(int idImage)
        {
            var result = await _imageRepository.GetImageByIdAsync(idImage);
            return result;
        }

        public async Task<ImageModel?> GetImageByStringAsync(string idImage)
        {
            var result = await _imageRepository.GetByImageAsync(idImage);
            return result;
        }

        public async Task<ImageModel?> GetEmbeddingAsync(string emebedding)
        {
            var result = await _imageRepository.GetByEmbeddingAsync(emebedding);
            return result;
        }

        // Retorna a imagem pelo ID do usuário
        public async Task<ImageModel?> GetImageByUserIdAsync(int userId)
        {
            var result = await _imageRepository.GetImageByUserIdAsync(userId);
            return result;
        }

        // Adiciona uma imagem
        public async Task<ImageModel?> AddImageAsync(ImageModel pictureStream)
        {
            // Tenta adicionar a imagem ao repositório
            var result = await _imageRepository.AddImageAsync(pictureStream);
            return result;
        }

        // Atualiza uma imagem existente
        public async Task<ImageModel?> UpdateImageAsync(ImageModel imageEntity)
        {
            var result = await _imageRepository.UpdateImageAsync(imageEntity);
            return result;
        }

        // Deleta uma imagem pelo ID
        public async Task<bool> DeleteImageAsync(int image)
        {
            var result = await _imageRepository.DeleteImageAsync(image);
            return result;
        }
    }


}
