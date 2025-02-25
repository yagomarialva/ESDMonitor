using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface IRecordStatusRepository
    {
        /// <summary>
        /// Obtém todos os registros de status de produção.
        /// </summary>
        Task<List<RecordStatusProduceModel>> GetAllAsync();

        /// <summary>
        /// Obtém um registro de status de produção pelo seu ID.
        /// </summary>
        /// <param name="id">ID do registro de status.</param>
        Task<RecordStatusProduceModel?> GetByIdAsync(int id);

        /// <summary>
        /// Obtém um registro de status de produção pelo ID da atividade de produção.
        /// </summary>
        /// <param name="produceActivityId">ID da atividade de produção.</param>
        Task<RecordStatusProduceModel?> GetByProduceActivityIdAsync(int produceActivityId);

        /// <summary>
        /// Obtém um registro de status de produção pelo ID do usuário.
        /// </summary>
        /// <param name="userId">ID do usuário.</param>
        Task<RecordStatusProduceModel?> GetByUserIdAsync(int userId);

        /// <summary>
        /// Insere ou atualiza um registro de status de produção.
        /// </summary>
        /// <param name="model">Modelo do registro de status de produção.</param>
        Task<RecordStatusProduceModel?> AddOrUpdateAsync(RecordStatusProduceModel model);

        /// <summary>
        /// Deleta um registro de status de produção pelo seu ID.
        /// </summary>
        /// <param name="id">ID do registro a ser deletado.</param>
        Task<RecordStatusProduceModel> DeleteAsync(int id);
    }
}
