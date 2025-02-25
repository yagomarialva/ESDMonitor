using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class RecordStatusService
    {
        private readonly IRecordStatusRepository _repository;

        public RecordStatusService(IRecordStatusRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<(object?, int)> GetAllStatusAsync()
        {
            try
            {
                var statuses = await _repository.GetAllAsync();
                if (statuses == null || !statuses.Any())
                {
                    return ("Nenhum status encontrado.", StatusCodes.Status404NotFound);
                }

                return (statuses, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                // Log the exception here (e.g., using a logging framework)
                return ($"Falha de requisição: {ex.Message}", StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> GetByRecordStatusIdAsync(int id)
        {
            if (id <= 0)
            {
                return ("ID inválido.", StatusCodes.Status400BadRequest);
            }

            try
            {
                var status = await _repository.GetByIdAsync(id);
                if (status == null)
                {
                    return ("Registro não encontrado.", StatusCodes.Status404NotFound);
                }

                return (status, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ($"Erro ao buscar status pelo ID: {ex.Message}", StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> AddOrUpdateStatusAsync(RecordStatusProduceModel model)
        {
            if (model == null)
            {
                return ("Modelo inválido.", StatusCodes.Status400BadRequest);
            }

            try
            {
                var result = await _repository.AddOrUpdateAsync(model);
                return (result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ($"Erro ao salvar o status: {ex.Message}", StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> DeleteStatusAsync(int id)
        {
            if (id <= 0)
            {
                return ("ID inválido.", StatusCodes.Status400BadRequest);
            }

            try
            {
                var recordStatus = await _repository.GetByIdAsync(id);
                if (recordStatus == null)
                {
                    return ("Registro não encontrado.", StatusCodes.Status404NotFound);
                }

                await _repository.DeleteAsync(id);
                return ($"Registro ID {id} deletado com sucesso.", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ($"Erro ao deletar o status: {ex.Message}", StatusCodes.Status400BadRequest);
            }
        }
    }
}
