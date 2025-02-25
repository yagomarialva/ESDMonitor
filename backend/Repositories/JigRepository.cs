using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class JigRepository : IJigRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;
        public JigRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector;
           
        }
        public async Task<List<JigModel>> GetAllAsync()
        {

            var result = await _oraConnector.LoadData<JigModel, dynamic>(SQLScripts.JigQueries.GetAllJig, new { });
            return result;
        }
        public async Task<JigModel?> GetByIdAsync(int id)
        {
            var result = await _oraConnector.LoadData<JigModel, dynamic>(SQLScripts.JigQueries.GetJigId, new { id });
            return result.FirstOrDefault();
        }
        public async Task<JigModel?> GetJigBySnAsync(string serialNumber)
        {
            var result = await _oraConnector.LoadData<JigModel, dynamic>(SQLScripts.JigQueries.GetjigBySerialNumber, new { serialNumber });
            return result.FirstOrDefault();
        }
        public async Task<JigModel?> GetByNameAsync(string name)
        {
            //Tranforma name para letras minusculas, verifica se existe caracters especiais e tira os espçao no final da palavra.
            var nameLower = name.Normalize().ToLower().TrimEnd();

            var result = await _oraConnector.LoadData<JigModel, dynamic>(SQLScripts.JigQueries.GetJigName, new { nameLower });
            return result.FirstOrDefault();
        }
        
        public async Task<JigModel?> AddOrUpdateAsync(JigModel jigModel)
        {
            try
            {
              
                jigModel.Name = jigModel.Name.ToLowerInvariant();

                var formattedDateTime = DateTimeHelperService.GetManausCurrentDateTime();

                var lastUpdated = jigModel.LastUpdated = formattedDateTime; 
                var created = jigModel.Created = formattedDateTime;


                if (jigModel.ID > 0)
                {
                    jigModel.LastUpdated = lastUpdated;

                   
                    await _oraConnector.SaveData(SQLScripts.JigQueries.UpdateJig, jigModel);
                    if (_oraConnector.Error != null)
                        throw new Exception($"Erro durante o update: {_oraConnector.Error}");

                    jigModel = await GetByIdAsync(jigModel.ID);
                }
                else
                {
                    jigModel.Created = created;
                    jigModel.LastUpdated = lastUpdated;

                    
                    await _oraConnector.SaveData<JigModel>(SQLScripts.JigQueries.InsertJig, jigModel);
                    if (_oraConnector.Error != null)
                        throw new Exception($"Erro durante o insert: {_oraConnector.Error}");

                    jigModel = await GetByNameAsync(jigModel.Name);
                }

                return jigModel;
            }
            catch (Exception ex)
            {
                // Pode adicionar um log aqui para monitorar o erro
                throw new Exception($"Erro durante o AddOrUpdate: {ex.Message}", ex);
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                JigModel? jigDel = await GetByIdAsync(id);
                if (jigDel == null)
                    return false; // Já deletado ou não encontrado

                await _oraConnector.SaveData<dynamic>(SQLScripts.JigQueries.DeleteJig, new { id });
                if (_oraConnector.Error != null)
                    throw new Exception($"Erro durante o delete: {_oraConnector.Error}");

                return true;
            }
            catch (Exception ex)
            {
                // Pode adicionar um log aqui para monitorar o erro
                throw new Exception($"Erro no Delete: {ex.Message}", ex);
            }
        }
        public async Task<JigModel?> GetJigSerialNumberAsync(string serialNumberJig)
        {
            var result = await _oraConnector.LoadData<JigModel, dynamic>(SQLScripts.JigQueries.GetjigBySerialNumber, new { serialNumberJig });
            return result.FirstOrDefault();
        }
    }
}
