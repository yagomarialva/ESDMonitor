using BiometricFaceApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface IProduceActivityRepository
    {
        /// <summary>
        /// Obtém todas as atividades de produção.
        /// </summary>
        /// <returns>Lista de todas as atividades de produção.</returns>
        Task<List<ProduceActivityModel>> GetAllAsync();

        /// <summary>
        /// Obtém uma atividade de produção por ID.
        /// </summary>
        /// <param name="id">ID da atividade de produção.</param>
        /// <returns>Modelo da atividade de produção ou null se não for encontrada.</returns>
        Task<ProduceActivityModel?> GetByIdAsync(int id);

        /// <summary>
        /// Obtém uma atividade de produção por ID do usuário.
        /// </summary>
        /// <param name="userId">ID do usuário.</param>
        /// <returns>Modelo da atividade de produção ou null se não for encontrada.</returns>
        Task<ProduceActivityModel?> GetByUserIdAsync(int userId);

        /// <summary>
        /// Obtém uma atividade de produção por ID do jig.
        /// </summary>
        /// <param name="jigId">ID do jig.</param>
        /// <returns>Modelo da atividade de produção ou null se não for encontrada.</returns>
        Task<ProduceActivityModel?> GetByJigIdAsync(int jigId);

        /// <summary>
        /// Obtém uma atividade de produção por ID do monitor.
        /// </summary>
        /// <param name="monitorId">ID do monitor.</param>
        /// <returns>Modelo da atividade de produção ou null se não for encontrada.</returns>
        Task<ProduceActivityModel?> GetByMonitorIdAsync(int monitorId);

        /// <summary>
        /// Obtém uma atividade de produção por ID de vínculo de estação e linha.
        /// </summary>
        /// <param name="linkStationAndLineId">ID do vínculo de estação e linha.</param>
        /// <returns>Modelo da atividade de produção ou null se não for encontrada.</returns>
        Task<ProduceActivityModel?> GetByLinkStationAndLineIdAsync(int linkStationAndLineId);

        /// <summary>
        /// Verifica se uma atividade de produção está bloqueada.
        /// </summary>
        /// <param name="id">ID da atividade de produção.</param>
        /// <param name="isLocked">Indica se está bloqueada.</param>
        /// <returns>Modelo da atividade de produção ou null se não for encontrada.</returns>
        Task<ProduceActivityModel?> IsLockedAsync(int id, int isLocked);

        /// <summary>
        /// Adiciona uma nova atividade de produção.
        /// </summary>
        /// <param name="produceActivity">Dados da nova atividade de produção.</param>
        /// <returns>Modelo da atividade de produção adicionada ou null em caso de falha.</returns>
        Task<ProduceActivityModel?> AddOrUpdateAsync(ProduceActivityModel produceActivity);

        /// <summary>
        /// Exclui uma atividade de produção por ID.
        /// </summary>
        /// <param name="id">ID da atividade de produção a ser excluída.</param>
        /// <returns>Modelo da atividade de produção excluída ou null se não for encontrada.</returns>
        Task<ProduceActivityModel?> DeleteAsync(int id);
    }
}
