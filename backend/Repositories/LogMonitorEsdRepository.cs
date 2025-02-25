using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class LogMonitorEsdRepository : ILogMonitorEsdRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;

        public LogMonitorEsdRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector;
        }
        public async Task<List<LogMonitorEsdModel>> GetAllAsync()
        {
            var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(SQLScripts.LogMonitorEsdQueries.GetAllLogMonitor, new { });
            return result ??
                throw new KeyNotFoundException($"Nenhum Log de monitor esd cadastrado.");
        }
        public async Task<List<LogMonitorEsdModel>> GetLogIncreasingForStationviewAsync()
        {
            // limite fixo como 250, definido no GetListLogDesc
            var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(
                SQLScripts.LogMonitorEsdQueries.GetListLogDesc, new { });
            return result.ToList();
        }
        public async Task<LogMonitorEsdModel?> GetByIdAsync(int id)
        {
            var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(SQLScripts.LogMonitorEsdQueries.GetLogMonitorById, new { id });
            return result.FirstOrDefault();
        }
        public async Task<LogMonitorEsdModel?> GetMessageContentAsync(string messageContent)
        {
            //Tranforma name para letras minusculas, verifica se existe caracters especiais e tira os espçao no final da palavra.
            //var messageContentLower = messageContent.Normalize().ToLower().TrimEnd();
            var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(SQLScripts.LogMonitorEsdQueries.GetMessageContent, new { messageContent });
            return result.FirstOrDefault();
        }
        public async Task<LogMonitorEsdModel?> GetMessageTypeAsync(string messageType)
        {
            //Tranforma name para letras minusculas, verifica se existe caracters especiais e tira os espçao no final da palavra.
            var messageTypeLower = messageType.Normalize().ToLower().TrimEnd();
            var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(SQLScripts.LogMonitorEsdQueries.GetMessageType, new { messageTypeLower });
            return result.FirstOrDefault();
        }
        public async Task<LogMonitorEsdModel?> GetMonitorEsdByIdAsync(int monitorId)
        {
            var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(SQLScripts.LogMonitorEsdQueries.GetMonitorEsdInLogById, new { monitorId });
            return result.FirstOrDefault();
        }
        public async Task<LogMonitorEsdModel?> GetMonitorEsdByIPAsync(string ip)
        {
            var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(SQLScripts.LogMonitorEsdQueries.GetMonitorEsdInLogByIP, new { ip });
            return result.FirstOrDefault();
        }
        public async Task<LogMonitorEsdModel?> GetMonitorEsdBySnAsync(string serialNumberEsp)
        {
            var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(SQLScripts.LogMonitorEsdQueries.GetMonitorEsdInLogBySerialNumber, new { serialNumberEsp });
            return result.FirstOrDefault();
        }
        public async Task<List<LogMonitorEsdModel>> GetListMonitorEsdByIdAsync(int monitorId, int page, int pageSize)
        {
            var offset = (page - 1) * pageSize;
            var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(SQLScripts.LogMonitorEsdQueries.GetListMonitorByIdWithPagination,
                new { monitorId, Offset = offset, Limit = pageSize });
            return result.ToList();
        }
        public async Task<List<LogMonitorEsdModel>> GetLogIncreasingAsync(int serialNumberEsp, int limit)
        {
            var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(SQLScripts.LogMonitorEsdQueries.GetListLogBySerialNumberIncreWithLimit,
                new { serialNumberEsp, limit });
            return result.ToList();

        }
        public async Task<List<LogMonitorEsdModel>> GetLogDecreasing(int serialNumberEsp, int limit)
        {
             var result = await _oraConnector.LoadData<LogMonitorEsdModel, dynamic>(SQLScripts.LogMonitorEsdQueries.GetListLogBySerialNumberDescWithLimit,
                new { serialNumberEsp, limit });
            return result.ToList();
        }
        public async Task<LogMonitorEsdModel?> AddOrUpdateAsync(LogMonitorEsdModel model)
        {
            model.MessageContent = (model.MessageContent ?? string.Empty.ToLowerInvariant());
            model.MessageType = model.MessageType.ToLowerInvariant();

            var formattedDateTime = DateTimeHelperService.GetManausCurrentDateTime();

            var created = model.Created = formattedDateTime;
            var lastUpdate = model.LastUpdated = formattedDateTime;

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.ID > 0)
            {
                model.LastUpdated = lastUpdate;
                await _oraConnector.SaveData<LogMonitorEsdModel>(SQLScripts.LogMonitorEsdQueries.UpdateLogMonitorEsd, model);
                HandleOraConnectorError();
                return model;
            }
            else
            {
                model.Created = created;
                model.LastUpdated = lastUpdate;
                await _oraConnector.SaveData<LogMonitorEsdModel>(SQLScripts.LogMonitorEsdQueries.InsertLogMonitorEsd, model);
                var r = await GetByIdAsync(model.ID);
                HandleOraConnectorError();
                return model;
            }
        }
        public async Task<LogMonitorEsdModel?> ChangeLogAsync(int id, int changeStatus, string? description)
        {
            // Convert boolean value para 0 ou 1
            int changeLog = (changeStatus == 1) ? 1 : 0; //change = 1 -> true.

            var result = await GetByIdAsync(id);

            if (result != null && result.MessageType == "jig")
            {
                result.Status = changeLog;
                result.Description = description;
                result.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();

                await _oraConnector.SaveData<LogMonitorEsdModel>(SQLScripts.LogMonitorEsdQueries.UpdateLogMonitorStatus, result);
            }
            else if (result != null && result.MessageType == "operator")
            {
                result.Status = changeLog;
                result.Description = description;
                result.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                await _oraConnector.SaveData<LogMonitorEsdModel>(SQLScripts.LogMonitorEsdQueries.UpdateLogMonitorStatus, result);
            }

            return result;
        }
        public async Task<LogMonitorEsdModel?> DeleteAsync(int id)
        {
            LogMonitorEsdModel? LogmonitorDel = await GetByIdAsync(id);
            if (LogmonitorDel == null)
                throw new ArgumentException($"Log de Monitor com ID {id} não existe.");

            await _oraConnector.SaveData<dynamic>(SQLScripts.LogMonitorEsdQueries.DeleteLogMonitorEsd, new { id });
            HandleOraConnectorError();
            return LogmonitorDel;
        }
        public async Task<LogMonitorEsdModel?> DeleteMonitorEsdByLogIdAsync(int id)
        {
            LogMonitorEsdModel? monitor = await GetMonitorEsdByIdAsync(id);
            await _oraConnector.SaveData<dynamic>(SQLScripts.LogMonitorEsdQueries.Delete_MonitorEsd, new { id });
            HandleOraConnectorError();
            return monitor;
        }
        public async Task<LogMonitorEsdModel?> AddSocketyLogAsync(LogMonitorEsdModel model)
        {
            model.MessageContent = model.MessageContent.ToLowerInvariant();
            model.MessageType = model.MessageType.ToLowerInvariant();

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.Created = DateTimeHelperService.GetManausCurrentDateTime();
            model.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
            await _oraConnector.SaveData<LogMonitorEsdModel>(SQLScripts.LogMonitorEsdQueries.InsertLogMonitorEsd, model);
            HandleOraConnectorError();
            return await GetByIdAsync(model.ID);
        }
        private void HandleOraConnectorError()
        {
            if (_oraConnector.Error != null)
                throw new Exception($"Database Error: {_oraConnector.Error}");
        } 
    }
}
