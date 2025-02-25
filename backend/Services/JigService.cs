using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class JigService
    {
        private readonly IJigRepository _repository;

        public JigService(IJigRepository repository)
        {
            _repository = repository;
        }

        public async Task<(object? result, int statusCode)> GetAllJigsAsync()
        {
            try
            {
                var jigs = await _repository.GetAllAsync();
                if (jigs == null || !jigs.Any())
                {
                    return ("Nenhum Jig cadastrado.", StatusCodes.Status404NotFound);
                }

                return (jigs, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return HandleError(exception);
            }
        }

        public async Task<(object? result, int statusCode)> GetJigByIdAsync(int id)
        {
            try
            {
                var jig = await _repository.GetByIdAsync(id);
                if (jig == null)
                {
                    return ("Jig Id não encontrado.", StatusCodes.Status404NotFound);
                }

                return (jig, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return HandleError(exception);
            }
        }
        public async Task<(object? result, int statusCode)> GetJigBySnAsync(string serialNumber)
        {
            try
            {
                var jig = await _repository.GetJigBySnAsync(serialNumber);
                if (jig == null)
                {
                    return ($"Jig com serial number: {serialNumber} não encontrado.", StatusCodes.Status404NotFound);
                }

                return (jig, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return HandleError(exception);
            }
        }
        public async Task<(object? result, int statusCode)> GetJigByNameAsync(string name)
        {
            try
            {
                var jig = await _repository.GetByNameAsync(name);
                if (jig == null)
                {
                    return ("Jig não encontrado.", StatusCodes.Status404NotFound);
                }

                return (jig, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return HandleError(exception);
            }
        }
        public async Task<(object? result, int statusCode)> AddOrUpdateJigAsync(JigModel model)
        {
            try
            {
                var existingJig = await _repository.GetByIdAsync(model.ID);
                model.ID = existingJig?.ID ?? model.ID;

                var savedJig = await _repository.AddOrUpdateAsync(model);

                var statusCode = model.ID > 0 ? StatusCodes.Status200OK : StatusCodes.Status201Created;
                return (savedJig, statusCode);
            }
            catch (Exception exception)
            {
                return HandleError(exception, "Verifique se os dados estão corretos.");
            }
        }
        public async Task<(object? result, int statusCode)> DeleteJigAsync(int id)
        {
            try
            {
                var jig = await _repository.GetByIdAsync(id);
                if (jig == null)
                {
                    return ("Jig não encontrado ou já excluído.", StatusCodes.Status404NotFound);
                }

                await _repository.DeleteAsync(id);
                return (jig, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return HandleError(exception);
            }
        }
        // Método auxiliar para padronizar o tratamento de exceções
        private (object? result, int statusCode) HandleError(Exception exception, string? customMessage = null)
        {
            var errorMessage = customMessage ?? exception.Message;
            // Adicionar um log aqui, se necessário
            return (errorMessage, StatusCodes.Status400BadRequest);
        }
        public async Task<JigModel?> GetSerialNumberAsync(string serial)
        {
            var result = await _repository.GetJigSerialNumberAsync(serial);
            return result;
        }
    }
}
