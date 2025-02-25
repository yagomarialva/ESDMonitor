using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class LastLogMonitorEsdRepository : ILastLogMonitorEsdRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;
        public LastLogMonitorEsdRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector;
        }
        public async Task<List<LastLogMonitorEsdModel>> GetAllLastLogsAsync()
        {
            var result = await _oraConnector.LoadData<LastLogMonitorEsdModel, dynamic>(SQLScripts.LastLogMonitorEsdQueries.GetAllLastLogMonitor, new { });
            return result ??
                throw new KeyNotFoundException($"Nenhum Log cadastrado.");
        }
        public async Task<LastLogMonitorEsdModel?> GetLastLogForCreatedAsync(LastLogMonitorEsdModel date)
        {
            var result = await _oraConnector.LoadData<LastLogMonitorEsdModel, dynamic>(SQLScripts.LastLogMonitorEsdQueries.GetLastLogForCreated,new { date });
            return result.FirstOrDefault();
        }
        public async Task<LastLogMonitorEsdModel?> GetByIdLastLogsAsync(int id)
        {
            var result = await _oraConnector.LoadData<LastLogMonitorEsdModel, dynamic>(SQLScripts.LastLogMonitorEsdQueries.GetLastLogMonitorById, new { id });
            return result.FirstOrDefault();
        }
        public async Task<LastLogMonitorEsdModel?> GetJigByIdLastLogsAsync(int jigId)
        {
            var result = await _oraConnector.LoadData<LastLogMonitorEsdModel, dynamic>(SQLScripts.LastLogMonitorEsdQueries.GetJigIdInLastLogById, new { jigId });
            return result.FirstOrDefault();
        }
        public async Task<LastLogMonitorEsdModel?> InsertLastLogAsync(LastLogMonitorEsdModel model)
        {
            var formattedDateTime = DateTimeHelperService.GetManausCurrentDateTime();

            if (model == null)
                throw new ArgumentNullException(nameof(model));


            model.MessageContent = (model.MessageContent ?? string.Empty).ToLowerInvariant();
            model.MessageType = (model.MessageType ?? string.Empty).ToLowerInvariant();
            model.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
            model.Created = DateTimeHelperService.GetManausCurrentDateTime();
            await _oraConnector.SaveData<LastLogMonitorEsdModel>(SQLScripts.LastLogMonitorEsdQueries.InsertLastLogs, model);
            HandleOraConnectorError();
            //var result = await GetByIdLastLogsAsync(model.ID);
            var result = await GetLastLogForCreatedAsync(model);
            return result;

        }
        public async Task<LastLogMonitorEsdModel?> UpdateLastLog(LastLogMonitorEsdModel model, string serialNumberEsp)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            LastLogMonitorEsdModel? last = await GetMonitorEsdBySnLastLogsAsync(serialNumberEsp);
            if (last == null)
                throw new KeyNotFoundException(nameof(last));



            //last.SerialNumberEsp = model.SerialNumberEsp;
            last.MessageType = model.MessageType = (model.MessageType ?? string.Empty).ToLowerInvariant();
            last.MonitorEsdId = model.MonitorEsdId;
            last.JigId = model.JigId;
            last.IP = model.IP;
            last.Status = model.Status;
            last.MessageContent = model.MessageContent = (model.MessageContent ?? string.Empty).ToLowerInvariant();
            last.Description = model.Description;
            last.LastUpdated = model.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();


            await _oraConnector.SaveData<LastLogMonitorEsdModel>(SQLScripts.LastLogMonitorEsdQueries.UpdateLastLogs, last);
            return last;
        }
        public async Task<LastLogMonitorEsdModel?> GetMonitorEsdByIdLastLogsAsync(int id)
        {
            var result = await _oraConnector.LoadData<LastLogMonitorEsdModel, dynamic>(SQLScripts.LastLogMonitorEsdQueries.GetLastLogMonitorById, new { id });
            return result.FirstOrDefault();
        }
        public async Task<LastLogMonitorEsdModel?> GetMonitorEsdBySnLastLogsAsync(string serialNumberEsp)
        {
            var result = await _oraConnector.LoadData<LastLogMonitorEsdModel, dynamic>(SQLScripts.LastLogMonitorEsdQueries.GetMonitorEsdInLastLogBySerialNumber, new { serialNumberEsp });
            return result.FirstOrDefault();
        }
        public async Task<LastLogMonitorEsdModel?> GetByIdLastLogsMonitorEsdAsync(int monitorEsd, int jigId)
        {
            var result = await _oraConnector.LoadData<LastLogMonitorEsdModel, dynamic>(SQLScripts.LastLogMonitorEsdQueries.GetMonitorEsdInLastLogById, new { monitorEsd, jigId });
            return result.FirstOrDefault();
        }

        private void HandleOraConnectorError()
        {
            if (_oraConnector.Error != null)
                throw new Exception($"Database Error: {_oraConnector.Error}");
        }

        public async Task<LastLogMonitorEsdModel?> GetMessageTypeAsync(string serialNumberEsp, string messageType)
        {
            var result = await _oraConnector.LoadData<LastLogMonitorEsdModel, dynamic>(SQLScripts.LastLogMonitorEsdQueries.GetMessageType, new { serialNumberEsp, messageType });
            return result.FirstOrDefault();
        }
    }
}
