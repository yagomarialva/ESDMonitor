using BiometricFaceApi.Hubs;
using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace BiometricFaceApi.Services
{
    public class LogMonitorEsdService
    {
        private readonly ILogMonitorEsdRepository _logMonitorEsd;
        private readonly ILastLogMonitorEsdRepository _lastLogMonitorEsd;
        private readonly IMonitorEsdRepository _monitorEsdRepository;
        private readonly IHubContext<CommunicationHub> _hubContext;
        public LogMonitorEsdService(ILogMonitorEsdRepository logMonitorEsdRepository, ILastLogMonitorEsdRepository lastLogMonitorEsd,IMonitorEsdRepository monitorEsdRepository, IHubContext<CommunicationHub> hubContext)
        {
            _logMonitorEsd = logMonitorEsdRepository;
            _lastLogMonitorEsd = lastLogMonitorEsd;
            _monitorEsdRepository = monitorEsdRepository;
            _hubContext = hubContext;
        }
        public async Task<(object?, int)> GetAllAsync()
        {
            try
            {
                var logmonitorEsd = await _logMonitorEsd.GetAllAsync();
                if (!logmonitorEsd.Any())
                {
                    return ("Nenhum Log cadastrado", StatusCodes.Status404NotFound);
                }
                return (logmonitorEsd, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {

                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetLogForStationviewAsync()
        {
            try
            {
                var logmonitorEsd = await _logMonitorEsd.GetLogIncreasingForStationviewAsync();
                {
                    return ("Nenhum Log cadastrado", StatusCodes.Status404NotFound);
                }
                return (logmonitorEsd, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {

                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetByIdAsync(int id)
        {
            try
            {
                var logMonitor = await _logMonitorEsd.GetByIdAsync(id);
                if (logMonitor == null)
                {
                    return ($"Log de Monitor com Id: {id} não encontrado.", StatusCodes.Status404NotFound);
                }

                return (logMonitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetMonitorEsdByIdAsync(int id)
        {
            try
            {
                var logMonitor = await _logMonitorEsd.GetMonitorEsdByIdAsync(id);
                if (logMonitor == null)
                {
                    return ($"Monitor Esd com Id: {id} não encontrado.", StatusCodes.Status404NotFound);
                }

                return (logMonitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetMonitorEsdBySnAsync(string sn)
        {
            try
            {
                var logMonitor = await _logMonitorEsd.GetMonitorEsdBySnAsync(sn);
                if (logMonitor == null)
                {
                    return ($"Monitor Esd com Serial Number: {sn} não encontrado.", StatusCodes.Status404NotFound);
                }

                return (logMonitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetMonitorEsdByIPAsync(string ip)
        {
            try
            {
                var logMonitor = await _logMonitorEsd.GetMonitorEsdByIPAsync(ip);
                if (logMonitor == null)
                {
                    return ($"Monitor Esd com IP: {ip} não encontrado.", StatusCodes.Status404NotFound);
                }

                return (logMonitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetListMonitorEsdByIdAsync(int id, int page, int pageSize)
        {
            try
            {
                var logMonitor = await _logMonitorEsd.GetListMonitorEsdByIdAsync(id, page, pageSize);
                if (logMonitor.Count() == 0)
                {
                    return ($"Monitor Esd com Id: {id} não encontrado.", StatusCodes.Status404NotFound);
                }

                return (logMonitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetLogIncreAsync(int serialNumberEsp, int limit)
        {
            try
            {
                var logMonitor = await _logMonitorEsd.GetLogIncreasingAsync(serialNumberEsp, limit);
                if (logMonitor.Count() == 0)
                {
                    return ($"Monitor Esd com {serialNumberEsp} não encontrado.", StatusCodes.Status404NotFound);
                }

                return (logMonitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetLogDecreAsync(int serialNumberEsp, int limit)
        {
            try
            {
                var logMonitor = await _logMonitorEsd.GetLogDecreasing(serialNumberEsp, limit);
                if (logMonitor.Count() == 0)
                {
                    return ($"Monitor Esd com {serialNumberEsp} não encontrado.", StatusCodes.Status404NotFound);
                }

                return (logMonitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetMessageContentAsync(string messageContent)
        {
            try
            {
                var content = await _logMonitorEsd.GetMessageTypeAsync(messageContent);
                if (content == null)
                {
                    return ($"Conteudo da Mensagem: {messageContent} não encontrado.", StatusCodes.Status404NotFound);
                }
                return (content, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {

                return (exception.Message, StatusCodes.Status400BadRequest); ;
            }
        }
        public async Task<(object?, int)> GetMessageTypeAsync(string messageType)
        {
            try
            {
                var type = await _logMonitorEsd.GetMessageTypeAsync(messageType);
                if (type == null)
                {
                    return ($"Tipo de Mensagem: {messageType} não encontrado.", StatusCodes.Status404NotFound);
                }
                return (type, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {

                return (exception.Message, StatusCodes.Status400BadRequest); ;
            }
        }
        public async Task<(object?, int)> ChangeStatusLog(int id, bool changeLog, string? description)
        {
            try
            {
                if (string.IsNullOrEmpty(description))
                {
                    throw new ArgumentNullException("Por favor informar o motivo da alteração de status");
                }

                var logMonitor = await _logMonitorEsd.GetByIdAsync(id);

                var changeContent = logMonitor.MessageContent = "Status alterado manualmente após Manutenção.";

                if (logMonitor == null)
                {
                    return ("Log não encontrado", StatusCodes.Status404NotFound);
                }
                var logMonitorModel = new LogMonitorEsdModel
                {
                    ID = id,
                    SerialNumberEsp = logMonitor.SerialNumberEsp,
                    MonitorEsdId = logMonitor.MonitorEsdId,
                    JigId = logMonitor.JigId,
                    IP = logMonitor.IP,
                    Status = changeLog ? 1 : 0,
                    MessageType = logMonitor.MessageType,
                    MessageContent = changeContent,
                    Description = description
                };
                await _logMonitorEsd.AddOrUpdateAsync(logMonitorModel);
                await _hubContext.Clients.All.SendAsync("ReceiveAlert", logMonitor);
                var updateLog = await _logMonitorEsd.ChangeLogAsync(id, changeLog ? 1 : 0, description);
                return (logMonitorModel, StatusCodes.Status200OK);
            }
            catch (Exception exeception)
            {

                return (exeception.Message, StatusCodes.Status409Conflict);
            }
        }
        public async Task<(object?, int)> AddOrUpdateAsync(LogMonitorEsdModel logMonitorModel)
        {
            try
            {
                var monitor = await _monitorEsdRepository.GetMonitorByIdAsync(logMonitorModel.MonitorEsdId);
                if(monitor == null)
                    throw new Exception("Monitor nao cadastrado!");

                // Verificar se o log já existe na tabela LastLogMonitorEsd
                var lastLogMonitor = await _lastLogMonitorEsd.GetMessageTypeAsync(logMonitorModel.SerialNumberEsp, logMonitorModel.MessageType);
                

                if (lastLogMonitor == null)
                {
                    // Se não existir, faz o insert na tabela LastLogMonitorEsd
                    var newLastLog = new LastLogMonitorEsdModel
                    {
                        ID = logMonitorModel.ID,
                        SerialNumberEsp = logMonitorModel.SerialNumberEsp,
                        MessageType = logMonitorModel.MessageType,
                        MonitorEsdId = logMonitorModel.MonitorEsdId,
                        JigId = logMonitorModel.JigId,
                        MessageContent = logMonitorModel.MessageContent,
                       
                        IP = logMonitorModel.IP,
                        Status = logMonitorModel.Status,
                        Description = logMonitorModel.Description
                    };

                    await _lastLogMonitorEsd.InsertLastLogAsync(newLastLog);
                }
                else
                {
                    // Se já existir, faz o update dos dados na tabela LastLogMonitorEsd
                    lastLogMonitor.MessageContent = logMonitorModel.MessageContent;
                    lastLogMonitor.MessageType = logMonitorModel.MessageType;
                    lastLogMonitor.MonitorEsdId = logMonitorModel.MonitorEsdId;
                    lastLogMonitor.JigId = logMonitorModel.JigId;
                    lastLogMonitor.IP = logMonitorModel.IP;
                    lastLogMonitor.Status = logMonitorModel.Status;
                    lastLogMonitor.Description = logMonitorModel.Description;
                    lastLogMonitor.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime(); // Atualizando o horário de atualização

                    await _lastLogMonitorEsd.UpdateLastLog(lastLogMonitor, lastLogMonitor.SerialNumberEsp);
                }

                // Continua o fluxo de adicionar ou atualizar na tabela LogMonitorEsd
                LogMonitorEsdModel? existingLogMonitor = await _logMonitorEsd.GetByIdAsync(logMonitorModel.ID);
                bool isNew = existingLogMonitor == null;

                var response = await _logMonitorEsd.AddOrUpdateAsync(logMonitorModel);
                await _hubContext.Clients.All.SendAsync("ReceiveAlert", logMonitorModel);
                int statusCode = isNew ? StatusCodes.Status201Created : StatusCodes.Status200OK;

                return (response, statusCode);
            }
            catch (Exception)
            {
                return ("Verifique se os dados estão corretos.", StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> InsertSocketLogs(LogMonitorEsdModel logMonitorModel)
        {
            try
            {
                // Verificar se o log já existe na tabela LastLogMonitorEsd
                var lastLogMonitor = await _lastLogMonitorEsd.GetMessageTypeAsync(logMonitorModel.SerialNumberEsp, logMonitorModel.MessageType);

                if (lastLogMonitor == null)
                {
                    // Se não existir, faz o insert na tabela LastLogMonitorEsd
                    var newLastLog = new LastLogMonitorEsdModel
                    {
                        ID = logMonitorModel.ID,
                        SerialNumberEsp = logMonitorModel.SerialNumberEsp,
                        MessageContent = logMonitorModel.MessageContent,
                        MessageType = logMonitorModel.MessageType,
                        IP = logMonitorModel.IP,
                        Status = logMonitorModel.Status,
                        Description = logMonitorModel.Description
                    };

                    await _lastLogMonitorEsd.InsertLastLogAsync(newLastLog);
                }
                else
                {
                    // Se já existir, faz o update dos dados na tabela LastLogMonitorEsd
                    lastLogMonitor.MessageContent = logMonitorModel.MessageContent;
                    lastLogMonitor.MessageType = logMonitorModel.MessageType;
                    lastLogMonitor.IP = logMonitorModel.IP;
                    lastLogMonitor.Status = logMonitorModel.Status;
                    lastLogMonitor.Description = logMonitorModel.Description;
                    lastLogMonitor.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime(); // Atualizando o horário de atualização

                    await _lastLogMonitorEsd.UpdateLastLog(lastLogMonitor, lastLogMonitor.SerialNumberEsp);
                }

                // Continua o fluxo de adicionar ou atualizar na tabela LogMonitorEsd
                LogMonitorEsdModel? existingLogMonitor = await _logMonitorEsd.GetByIdAsync(logMonitorModel.ID);
                bool isNew = existingLogMonitor == null;

                var response = await _logMonitorEsd.AddOrUpdateAsync(logMonitorModel);
                await _hubContext.Clients.All.SendAsync("ReceiveAlert", logMonitorModel);
                int statusCode = isNew ? StatusCodes.Status201Created : StatusCodes.Status200OK;

                return (response, statusCode);
            }
            catch (Exception)
            {
                return ("Verifique se os dados estão corretos.", StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> Delete(int id)
        {
            try
            {
                var log = await _logMonitorEsd.GetByIdAsync(id);
                if (log == null)
                {
                    return ($"{id} não encontrado.", StatusCodes.Status404NotFound);
                }

                await _logMonitorEsd.DeleteAsync(log.ID);

                var content = new
                {
                    id = log.ID,
                    message_Type = log.MessageType,
                    message_Content = log.MessageContent,
                    monitorEsdId = log.MonitorEsdId,
                };

                return (content, StatusCodes.Status200OK);
            }
            catch (Exception)
            {
                return ($"{id} não encontrado.", StatusCodes.Status400BadRequest);
            }
        }
    }
}
