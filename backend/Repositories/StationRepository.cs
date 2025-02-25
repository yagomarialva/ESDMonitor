using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class StationRepository : IStationRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;

        public StationRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector;
        }

        public async Task<List<StationModel>> GetAllAsync()
        {
            var result = await _oraConnector.LoadData<StationModel, dynamic>(SQLScripts.StationQueries.GetAllLStation, new { });
            return result ?? new List<StationModel>();
        }

        public async Task<StationModel> GetByIdAsync(int id)
        {
            var result = await _oraConnector.LoadData<StationModel, dynamic>(SQLScripts.StationQueries.GetStationId, new { id });
            return result?.FirstOrDefault() ??
                throw new KeyNotFoundException($"Estação com ID {id} não encontrado.");
        }

        public async Task<StationModel> GetByNameAsync(string name)
        {
           
            var nameLower = name.Normalize().ToLower().TrimEnd();

            var result = await _oraConnector.LoadData<StationModel, dynamic>(SQLScripts.StationQueries.GetStationName, new { nameLower });
            return result?.FirstOrDefault() ?? 
                throw new KeyNotFoundException($"Estação com  Name {name} não encontrado.");
        }

        public async Task<StationModel?> AddOrUpdateAsync(StationModel stationModel)
        {
            try
            {
                
                stationModel.Name = stationModel.Name.ToLowerInvariant();

                if (stationModel.ID > 0)
                {
                    stationModel.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                   
                    await _oraConnector.SaveData(SQLScripts.StationQueries.UpdateStation, stationModel);
                    
                }
                else
                {
                    stationModel.Created = DateTimeHelperService.GetManausCurrentDateTime();
                   
                    await _oraConnector.SaveData(SQLScripts.StationQueries.InsertStation, stationModel);
                   
                }

                if (_oraConnector.Error != null)
                    throw new Exception($"Erro ao salvar linha: {_oraConnector.Error}");

                stationModel = await GetByNameAsync(stationModel.Name);

                return stationModel;
            }
            catch (Exception )
            {

                throw new Exception($"Falha ao adicionar ou atualizar POST, nome ja cadastrado. ");
            }
            
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var station = await GetByIdAsync(id);
            await _oraConnector.SaveData(SQLScripts.StationQueries.DeleteStation, new { id });
            return station != null;
        }
    }
}
