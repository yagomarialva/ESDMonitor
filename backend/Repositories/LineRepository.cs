using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class LineRepository : ILineRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;
        public LineRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector;
        }
        public async Task<List<LineModel>> GetAllAsync()
        {
            var result = await _oraConnector.LoadData<LineModel, dynamic>(SQLScripts.LineQueries.GetAllLine, new { });
            return result;
        }
        public async Task<LineModel?> GetByIdAsync(int id)
        {
            var result = await _oraConnector.LoadData<LineModel, dynamic>(SQLScripts.LineQueries.GetLineById, new { id });
            return result.FirstOrDefault();
        }
        public async Task<LineModel?> GetByNameAsync(string name)
        {
            var nameLower = name.Normalize().ToLower().TrimEnd();
            var result = await _oraConnector.LoadData<LineModel, dynamic>(SQLScripts.LineQueries.GetLineByName, new { nameLower });
            return result.FirstOrDefault();
        }
        public async Task<LineModel?> AddOrUpdateAsync(LineModel lineModel)
        {
            try
            {
                lineModel.Name = lineModel.Name.ToLowerInvariant();
                if (lineModel.ID > 0)
                {
                    lineModel.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                    await _oraConnector.SaveData(SQLScripts.LineQueries.UpdateLine, lineModel);
                }
                else
                {
                    lineModel.Created = DateTimeHelperService.GetManausCurrentDateTime();
                    lineModel.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                    await _oraConnector.SaveData(SQLScripts.LineQueries.InsertLine, lineModel);
                    lineModel = await GetByNameAsync(lineModel.Name);
                }

                if (_oraConnector.Error != null)
                    throw new Exception($"Erro ao salvar linha: {_oraConnector.Error}");

                return lineModel;
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao adicionar ou atualizar a linha: {ex.Message}", ex);
            }
        }
        public async Task<LineModel?> DeleteAsync(int id)
        {
            try
            {
                LineModel? line = await GetByIdAsync(id);
                if (line == null)
                    throw new Exception("Linha não encontrada.");

                await _oraConnector.SaveData<dynamic>(SQLScripts.LineQueries.DeleteLine, new { id });

                if (_oraConnector.Error != null)
                    throw new Exception($"Erro ao deletar linha: {_oraConnector.Error}");

                return line;
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao deletar a linha: {ex.Message}", ex);
            }
        }
    }
}
