using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class StatusJigAndUserRepository : IStatusJigAndUserRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;

        public StatusJigAndUserRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector;
        }
        public async Task<StatusJigAndUserModel?> AddOrUpdateStatusAsync(StatusJigAndUserModel status)
        {
            try
            {
                var existingStatus = await GetLatestStatusAsync(status.MonitorEsdId, status.UserId, status.JigId);

                if (existingStatus != null)
                {
                    existingStatus.Status = status.Status;
                    existingStatus.LastLogId = status.LastLogId;
                    existingStatus.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                    await _oraConnector.SaveData<StatusJigAndUserModel>(SQLScripts.StatusJigAndUserQueries.UpdateStatusJigAndUser, existingStatus);

                }
                else
                {
                    //status.Created = DateTimeHelperService.GetManausCurrentDateTime();
                    status.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                    await _oraConnector.SaveData<StatusJigAndUserModel>(SQLScripts.StatusJigAndUserQueries.InsertStatusJigAndUser, status);

                }
                if (_oraConnector.Error != null)
                    throw new Exception($"Erro ao salvar linha: {_oraConnector.Error}");

                return status;
            }
            catch (Exception)
            {

                throw new Exception($"Falha ao adicionar ou atualizar LOG");
            }
           
        }

        public async Task<StatusJigAndUserModel?> GetLatestStatusAsync(int monitorEsdId, int? userId, int? jigId)
        {
            var lastStatus = await _oraConnector.LoadData<StatusJigAndUserModel, dynamic>(SQLScripts.StatusJigAndUserQueries.GetLastSatusJigAndUser, new { monitorEsdId, userId, jigId });
            return lastStatus.FirstOrDefault();
        }
    }
}
