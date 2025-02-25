using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class MonitorEsdRepository : IMonitorEsdRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;
        public MonitorEsdRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector;
        }
        public async Task<List<MonitorEsdModel>> GetAllMonitorsAsync()
        {
            var result = await _oraConnector.LoadData<MonitorEsdModel, dynamic>(SQLScripts.MonitoresdQueries.GetAllMonitor, new { });
            return result ??
                throw new KeyNotFoundException($"Nenhum monitor esd cadastrado.");
        }
        public async Task<MonitorEsdModel?> GetMonitorByIdAsync(int id)
        {
            var result = await _oraConnector.LoadData<MonitorEsdModel, dynamic>(SQLScripts.MonitoresdQueries.GetMonitorId, new { id });
            return result.FirstOrDefault();
        }
        public async Task<MonitorEsdModel?> GetMonitorBySerialAsync(string serial)
        {
            var result = await _oraConnector.LoadData<MonitorEsdModel, dynamic>(SQLScripts.MonitoresdQueries.GetSerialNumber, new { serial });
            return result.FirstOrDefault();
        }
        public async Task<MonitorEsdModel?> GetLogsAsync(string logs)
        {
            var result = await _oraConnector.LoadData<MonitorEsdModel, dynamic>(SQLScripts.MonitoresdQueries.GetByLogs, new { logs });
            return result.FirstOrDefault();
        }
        public async Task<MonitorEsdModel?> GetByStatusAsync(string status)
        {
            //Tranforma name para letras minusculas, verifica se existe caracters especiais e tira os espçao no final da palavra.
            var statusLower = status.Normalize().ToLower().TrimEnd();

            var result = await _oraConnector.LoadData<MonitorEsdModel, dynamic>(SQLScripts.MonitoresdQueries.GetStatus, new { statusLower });
            return result.FirstOrDefault();
        }
        public async Task<MonitorEsdModel?> GetByOperatorStatusAsync(string statusOperador)
        {
            //Tranforma name para letras minusculas, verifica se existe caracters especiais e tira os espçao no final da palavra.
            var statusOperadorLower = statusOperador.Normalize().ToLower().TrimEnd();

            var result = await _oraConnector.LoadData<MonitorEsdModel, dynamic>(SQLScripts.MonitoresdQueries.GetStatusOP, new { statusOperadorLower });
            return result.FirstOrDefault();
        }
        public async Task<MonitorEsdModel?> GetByJigStatusAsync(string statusJig)
        {
            //Tranforma name para letras minusculas, verifica se existe caracters especiais e tira os espçao no final da palavra.
            var statusJigLower = statusJig.Normalize().ToLower().TrimEnd();

            var result = await _oraConnector.LoadData<MonitorEsdModel, dynamic>(SQLScripts.MonitoresdQueries.GetStatusJig, new { statusJigLower });
            return result.FirstOrDefault();
        }
        public async Task<MonitorEsdModel?> AddOrUpdateAsync(MonitorEsdModel monitorModel)
        {
            if (monitorModel == null)
                throw new ArgumentNullException(nameof(monitorModel));

            if (string.IsNullOrEmpty(monitorModel.SerialNumberEsp))
                throw new ArgumentException("SerialNumber is required.");

            if (monitorModel.ID > 0)
            {
                monitorModel.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();

                await _oraConnector.SaveData<MonitorEsdModel>(SQLScripts.MonitoresdQueries.UpdateMonitorEsd, monitorModel);
                HandleOraConnectorError();
                return monitorModel;
            }
            else
            {
                monitorModel.Created = DateTimeHelperService.GetManausCurrentDateTime();
                monitorModel.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();

                await _oraConnector.SaveData<MonitorEsdModel>(SQLScripts.MonitoresdQueries.InsertMonitorEsd, monitorModel);
                HandleOraConnectorError();
                return await GetMonitorBySerialAsync(monitorModel.SerialNumberEsp);
            }
        }
        public async Task<MonitorEsdModel> DeleteAsync(int id)
        {
            MonitorEsdModel? monitorDel = await GetMonitorByIdAsync(id);
            if (monitorDel == null)
                throw new ArgumentException($"Monitor with ID {id} does not exist.");

            await _oraConnector.SaveData<dynamic>(SQLScripts.MonitoresdQueries.DeleteMonitor, new { id });
            HandleOraConnectorError();
            return monitorDel;
        }
        private void HandleOraConnectorError()
        {
            if (_oraConnector.Error != null)
                throw new Exception($"Database Error: {_oraConnector.Error}");
        }
    }
}
