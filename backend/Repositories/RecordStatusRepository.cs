using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;
using Microsoft.EntityFrameworkCore.Metadata;


namespace BiometricFaceApi.Repositories
{
    public class RecordStatusRepository : IRecordStatusRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;
        public RecordStatusRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector ?? throw new ArgumentNullException(nameof(oraConnector));
        }
        public async Task<List<RecordStatusProduceModel>> GetAllAsync()
        {
            try
            {
                return await _oraConnector.LoadData<RecordStatusProduceModel, dynamic>(SQLScripts.RecordStatusProduceQueries.GetAllRecordStatus, new { });
            }
            catch (Exception ex)
            {
               
                throw new Exception("\"Erro ao buscar todos os status registro.", ex);
            }
        }
        public async Task<RecordStatusProduceModel?> GetByIdAsync(int id)
        {
            try
            {
                var result = await _oraConnector.LoadData<RecordStatusProduceModel, dynamic>(SQLScripts.RecordStatusProduceQueries.GetRecordStatusById, new { id });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar status registro com ID {id}.", ex);
            }
        }
        public async Task<RecordStatusProduceModel?> GetByProduceActivityIdAsync(int produceActivityId)
        {
            try
            {
                var result = await _oraConnector.LoadData<RecordStatusProduceModel, dynamic>(SQLScripts.RecordStatusProduceQueries.GetRecordProduceActId, new { produceActivityId });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao  buscar status de registro para  atividade de produção com o ID {produceActivityId}.", ex);
            }
        }
        public async Task<RecordStatusProduceModel?> GetByUserIdAsync(int userId)
        {
            try
            {
                var result = await _oraConnector.LoadData<RecordStatusProduceModel, dynamic>(SQLScripts.RecordStatusProduceQueries.GetRecordProduceUserId, new { userId });
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar status do resgistro para o ID de usuário {userId}.", ex);
            }
        }
        public async Task<RecordStatusProduceModel?> AddOrUpdateAsync(RecordStatusProduceModel recordModel)
        {
            if (recordModel == null) throw new ArgumentNullException(nameof(recordModel));

            recordModel.DateEvent = DateTime.Now;
            
            try
            {
                if (recordModel.ID > 0)
                {
                   
                    recordModel.DateEvent = DateTimeHelperService.GetManausCurrentDateTime();
                    await _oraConnector.SaveData<RecordStatusProduceModel>(SQLScripts.RecordStatusProduceQueries.UpdateRecordStatusProduce, recordModel);
                }
                else
                {
                    
                    recordModel.DateEvent = DateTimeHelperService.GetManausCurrentDateTime();
                    await _oraConnector.SaveData<RecordStatusProduceModel>(SQLScripts.RecordStatusProduceQueries.InsertRecordStatusProduce, recordModel);
                }

                if (_oraConnector.Error != null)
                {
                    throw new Exception($"Erro no banco de dados: {_oraConnector.Error}");
                }

                return recordModel;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar o status do registro com ID: {recordModel.ID}", ex);
            }
        }
        public async Task<RecordStatusProduceModel> DeleteAsync(int id)
        {
            try
            {
                var recordToDelete = await GetByIdAsync(id);
                if (recordToDelete == null)
                {
                    throw new KeyNotFoundException($"Registro com ID {id} não encontrado.");
                }

                await _oraConnector.SaveData<dynamic>(SQLScripts.RecordStatusProduceQueries.DeleteRecordStatusProduce, new { id });

                return recordToDelete;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao excluir o status do registro com ID {id}.", ex);
            }
        }

    }
}
