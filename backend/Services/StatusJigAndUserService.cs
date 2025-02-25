using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class StatusJigAndUserService
    {
        private readonly IStatusJigAndUserRepository _statusJigAndUserRepository;
        private readonly ILastLogMonitorEsdRepository _lastLogMonitorEsdRepository;

        public StatusJigAndUserService(IStatusJigAndUserRepository statusJigAndUserRepository, ILastLogMonitorEsdRepository lastLogMonitorEsdRepository)
        {
            _statusJigAndUserRepository = statusJigAndUserRepository;
            _lastLogMonitorEsdRepository = lastLogMonitorEsdRepository;
        }

        //serviço injetado em BiometricService para validação de status
        public async Task<StatusJigAndUserModel?> GetCurrentStatusAsync(int monitorEsdId, int userId, int jigId)
        {
            var result = await _statusJigAndUserRepository.GetLatestStatusAsync(monitorEsdId, userId, jigId);
            return result;
        }

        public async Task UpdateStatusAsync(int monitorEsdId, int? userId,  int jigId)
        {
            // Pega o último log da tabela logmonitoresd para o monitorEsd, user e jig
            var lastLog = await _lastLogMonitorEsdRepository.GetByIdLastLogsMonitorEsdAsync(monitorEsdId,jigId);
            if (lastLog == null) return;

            // Define o tipo de mensagem ("operador" ou "jig") baseado no log
            var messageType = lastLog.MessageType?.ToLower();
            var status = lastLog.Status;

            // Cria ou atualiza o status na tabela StatusJigAndUser
            var statusJigAndUser = new StatusJigAndUserModel
            {
                MonitorEsdId = monitorEsdId,
                UserId = messageType == "operador" ? userId : (int?)null,
                JigId = messageType == "jig" ? jigId : (int?)null,
                MessageType = lastLog.MessageType,
                Status = status,
                LastLogId = lastLog.ID,
                LastUpdated = DateTimeHelperService.GetManausCurrentDateTime()
            };

            await _statusJigAndUserRepository.AddOrUpdateStatusAsync(statusJigAndUser);
        }

    }
}
