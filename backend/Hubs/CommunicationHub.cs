using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace BiometricFaceApi.Hubs
{
    public class CommunicationHub : Hub
    {
        private readonly LogMonitorEsdService _logMonitorEsdService;
        private readonly ILogger<CommunicationHub> _logger;
        private static Queue<LogMonitorEsdModel> _logQueue = new Queue<LogMonitorEsdModel>();
        private static bool _serverIsOnline = false;

        // Armazenar o status dos monitores e o tempo de envio do último log.
        private static ConcurrentDictionary<string, DateTime> _monitorStatusTimes = new ConcurrentDictionary<string, DateTime>();

        public CommunicationHub(IMonitorEsdRepository monitorEsdRepository, LogMonitorEsdService logMonitorEsdRepository, ILogger<CommunicationHub> logger)
        {
            _logMonitorEsdService = logMonitorEsdRepository;
            _logger = logger;
        }
        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"Cliente conectado: {Context.ConnectionId}");

            // Servidor está online novamente
            _serverIsOnline = true;

            // Enviar todos os logs pendentes na fila
            await ProcessQueueLogsAsync();

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation($"Cliente desconectado: {Context.ConnectionId}");

            // Servidor está offline
            _serverIsOnline = false;

            if (exception != null)
            {
                _logger.LogError($"Erro de desconexão: {exception.Message}");
            }
            await base.OnDisconnectedAsync(exception);
        }
        // Recebe logs de um monitor ESD
        public async Task ReceiveLogFromMonitor(string logData)
        {
            try
            {
                var logObject = DeserializeLogData(logData);
                if (logObject != null)
                {
                    if (_serverIsOnline)
                    {
                        // Armazena o log no banco e transmite ao front-end
                        await SaveLogData(logObject);
                        await Clients.All.SendAsync("ReceiveLog", logObject);

                        // Atualiza o tempo do monitor
                        _monitorStatusTimes[logObject.SerialNumberEsp] = DateTime.Now;

                        // Verifica o status do monitor, se for 0 e passou 1 segundo, envia alerta
                        if (logObject.Status == 0)
                        {
                            _logger.LogInformation($"Monitor {logObject.SerialNumberEsp} está com status 0.");
                            Parallel.Invoke(async () => await CheckMonitorStatus(logObject.SerialNumberEsp,logData));
                        }
                    }
                    else
                    {
                        // Se o servidor estiver offline, adicionar o log na fila
                        _logQueue.Enqueue(logObject);
                        _logger.LogInformation("Servidor offline. Log armazenado na fila.");
                    }
                }
                else
                {
                    await Clients.Caller.SendAsync("ReceiveLogError", "400 Bad Request: Dados inválidos!");
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Erro ao deserializar logData: {ex.Message}", ex);
                await Clients.Caller.SendAsync("ReceiveLogError", "400 Bad Request: Erro ao processar os dados recebidos.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao salvar os dados: {ex.Message}", ex);
                await Clients.Caller.SendAsync("ReceiveLogError", "Erro ao salvar os dados no servidor.");
            }
        }
        private LogMonitorEsdModel? DeserializeLogData(string logData)
        {
            try
            {
                return JsonConvert.DeserializeObject<LogMonitorEsdModel>(logData);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Erro ao deserializar JSON: {ex.Message}", ex);
                return null;
            }
        }
        private async Task SaveLogData(LogMonitorEsdModel logObject)
        {
            await _logMonitorEsdService.InsertSocketLogs(logObject);
        }
        private async Task ProcessQueueLogsAsync()
        {
            while (_logQueue.Count > 0)
            {
                var log = _logQueue.Dequeue();
                await SaveLogData(log);
                await Clients.All.SendAsync("ReceiveLog", log);
            }
        }

        // Verifica se o monitor não atualizou o status para 1 após 1 segundo
        private async Task CheckMonitorStatus(string serialNumber,object payload)
        {
            // Verifica o tempo que passou desde a última atualização
            var currentTime = DateTime.Now;
            if (_monitorStatusTimes.ContainsKey(serialNumber))
            {
                var lastUpdate = _monitorStatusTimes[serialNumber];
                var elapsed = currentTime - lastUpdate;

                if (elapsed.TotalSeconds > 1)
                {
                    // Se passaram mais de 1 segundo e o status é 0, envia uma notificação de erro para o cliente
                    await Clients.All.SendAsync("ReceiveAlert",payload);
                }
            }
        }
    }
}
