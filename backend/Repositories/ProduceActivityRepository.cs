using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class ProduceActivityRepository : IProduceActivityRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;

        public ProduceActivityRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector;
        }
        public async Task<List<ProduceActivityModel>> GetAllAsync()
        {
            return await _oraConnector.LoadData<ProduceActivityModel, dynamic>(SQLScripts.ProduceActivityQueries.GetAllProcuceAct, new { });
        }
        public async Task<ProduceActivityModel?> GetByIdAsync(int id)
        {
            var result = await _oraConnector.LoadData<ProduceActivityModel, dynamic>(SQLScripts.ProduceActivityQueries.GetProduceActById, new { id });
            return result.FirstOrDefault();
        }
        public async Task<ProduceActivityModel?> GetByMonitorIdAsync(int monitorEsdId)
        {
            var result = await _oraConnector.LoadData<ProduceActivityModel, dynamic>(SQLScripts.ProduceActivityQueries.GetMonitorActById, new { monitorEsdId });
            return result.FirstOrDefault();
        }
        public async Task<ProduceActivityModel?> GetByJigIdAsync(int jigId)
        {
            var result = await _oraConnector.LoadData<ProduceActivityModel, dynamic>(SQLScripts.ProduceActivityQueries.GetJigActById, new { jigId });
            return result.FirstOrDefault();
        }
        public async Task<ProduceActivityModel?> GetByUserIdAsync(int userId)
        {
            var result = await _oraConnector.LoadData<ProduceActivityModel, dynamic>(SQLScripts.ProduceActivityQueries.GetUserActById, new { userId });
            return result.FirstOrDefault();
        }
        public async Task<ProduceActivityModel?> GetByLinkStationAndLineIdAsync(int linkStationAndLineId)
        {
            var result = await _oraConnector.LoadData<ProduceActivityModel, dynamic>(SQLScripts.ProduceActivityQueries.GetLinkStationAndLineById, new { linkStationAndLineId });
            return result.FirstOrDefault();
        }
        public async Task<ProduceActivityModel?> IsLockedAsync(int id, int locked)
        {
            var result = await GetByIdAsync(id);

            // Converte boolean value para 0 ou 1 
            int lockdValue = (locked == 1) ? 1 : 0;  // locked = 1 -> true, locked != 1 -> false

            if (result != null)
            {
                result.IsLocked = lockdValue; // Atualiza o status de IsLocked (booleano)
                result.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();

                await _oraConnector.SaveData<ProduceActivityModel>(SQLScripts.ProduceActivityQueries.UpdateProduceAct, result);
                
            }
            else
                throw new Exception($"Erro ao atualizar atividade de produção: {_oraConnector.Error}");
            return result;
        }
       
        public async Task<ProduceActivityModel?> AddOrUpdateAsync(ProduceActivityModel produceActivity)
        {
            if (produceActivity.ID > 0)
            {
                // Atualização
                produceActivity.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                await _oraConnector.SaveData<ProduceActivityModel>(SQLScripts.ProduceActivityQueries.UpdateProduceAct, produceActivity);

                if (_oraConnector.Error != null)
                {
                    throw new Exception($"Erro ao atualizar atividade de produção: {_oraConnector.Error}");
                }
            }
            else
            {
                // Inclusão
                produceActivity.Created = DateTimeHelperService.GetManausCurrentDateTime();
                produceActivity.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                await _oraConnector.SaveData<ProduceActivityModel>(SQLScripts.ProduceActivityQueries.InsertProduceAct, produceActivity);

                if (_oraConnector.Error != null)
                {
                    throw new Exception($"Erro ao incluir atividade de produção: {_oraConnector.Error}");
                }
            }

            return produceActivity;
        }
        public async Task<ProduceActivityModel?> DeleteAsync(int id)
        {
            var produceActivity = await GetByIdAsync(id);

            if (produceActivity == null)
            {
                throw new ArgumentException("ID de atividade de produção inválido.");
            }

            await _oraConnector.SaveData<dynamic>(SQLScripts.ProduceActivityQueries.DeleteProduceAct, new { id });

            if (_oraConnector.Error != null)
            {
                throw new Exception($"Erro ao deletar atividade de produção: {_oraConnector.Error}");
            }

            return produceActivity;
        }
    }
}