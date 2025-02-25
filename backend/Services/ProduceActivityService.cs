using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BiometricFaceApi.Services
{
    public class ProduceActivityService
    {
        private readonly IProduceActivityRepository _produceActivityRepository;
        private readonly IRecordStatusRepository _recordStatusRepository;
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IJigRepository _jigRepository;
        private readonly ILinkStationAndLineRepository _linkStationAndLineRepository;
        private readonly IMonitorEsdRepository _monitorEsdRepository;

        public ProduceActivityService(
            IProduceActivityRepository produceActivityRepository,
            IRecordStatusRepository recordStatusRepository,
            IAuthenticationRepository authenticationRepository,
            IUsersRepository usersRepository,
            IJigRepository jigRepository,
            ILinkStationAndLineRepository linkStationAndLineRepository,
            IMonitorEsdRepository monitorEsdRepository)
        {
            _produceActivityRepository = produceActivityRepository ?? throw new ArgumentNullException(nameof(produceActivityRepository));
            _recordStatusRepository = recordStatusRepository ?? throw new ArgumentNullException(nameof(recordStatusRepository));
            _authenticationRepository = authenticationRepository ?? throw new ArgumentNullException(nameof(authenticationRepository));
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _jigRepository = jigRepository ?? throw new ArgumentNullException(nameof(jigRepository));
            _linkStationAndLineRepository = linkStationAndLineRepository ?? throw new ArgumentNullException(nameof(linkStationAndLineRepository));
            _monitorEsdRepository = monitorEsdRepository ?? throw new ArgumentNullException(nameof(monitorEsdRepository));
        }

        public async Task<(object? content, int statusCode)> GetAllProduceActivitiesAsync()
        {
            try
            {
                var produceActivities = await _produceActivityRepository.GetAllAsync();
                if (!produceActivities.Any())
                {
                    return ("Nenhuma Produção foi encontrada.", StatusCodes.Status404NotFound);
                }

                var jigs = await _jigRepository.GetAllAsync();
                var monitors = await _monitorEsdRepository.GetAllMonitorsAsync();
                var users = await _usersRepository.GetAllAsync();
                var links = await _linkStationAndLineRepository.GetAllLinksAsync();

                foreach (var activity in produceActivities)
                {
                    activity.User = users.FirstOrDefault(u => u.ID == activity.UserId);
                    activity.Jig = jigs.FirstOrDefault(j => j.ID == activity.JigId);
                    activity.MonitorEsd = monitors.FirstOrDefault(m => m.ID == activity.MonitorEsdId);
                    activity.LinkStationAndLine = links.FirstOrDefault(l => l.ID == activity.LinkStationAndLineID);
                }

                return (produceActivities, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object? content, int statusCode)> GetProduceActivityByIdAsync(int id)
        {
            try
            {
                var produceActivity = await _produceActivityRepository.GetByIdAsync(id);
                if (produceActivity == null)
                {
                    return ("Produção não encontrada.", StatusCodes.Status404NotFound);
                }
                return (produceActivity, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object? content, int statusCode)> ChangeProduceActivityStatusAsync(int id, bool isLocked, string? description, string jwt)
        {
            try
            {
                if (isLocked && string.IsNullOrEmpty(description))
                {
                    throw new ArgumentException("Por favor informar o motivo da inativação.");
                }

                var produceActivity = await _produceActivityRepository.GetByIdAsync(id);
                if (produceActivity == null)
                {
                    return ("Produção não encontrada.", StatusCodes.Status404NotFound);
                }

                var payload = new JwtSecurityTokenHandler().ReadJwtToken(jwt);
                var usernameClaim = payload.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (usernameClaim == null)
                {
                    throw new UnauthorizedAccessException("Usuário inválido.");
                }

                var user = await _authenticationRepository.GetByUsernameAsync(usernameClaim);
                var recordStatus = new RecordStatusProduceModel
                {
                    ProduceActivityId = id,
                    UserId = user.ID,
                    Description = description,
                    Status = isLocked ? 1 : 0,
                    DateEvent = DateTime.Now
                };

                await _recordStatusRepository.AddOrUpdateAsync(recordStatus);
                var updatedProduceActivity = await _produceActivityRepository.IsLockedAsync(id, isLocked ? 1 : 0);

                return (recordStatus, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object? content, int statusCode)> AddOrUpdateProduceActivityAsync(ProduceActivityModel produceModel)
        {
            try
            {
                if (produceModel.UserId == 0 || produceModel.JigId == 0 || produceModel.MonitorEsdId == 0)
                {
                    return ("Preencha todos os campos obrigatórios.", StatusCodes.Status400BadRequest);
                }

                await _produceActivityRepository.AddOrUpdateAsync(produceModel);
                return (produceModel, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object? content, int statusCode)> DeleteProduceActivityAsync(int id)
        {
            object? content;
            int statusCode;
            try
            {
                var produceActivity = await _produceActivityRepository.GetByIdAsync(id);
                if (produceActivity == null)
                {
                    return ("Produção não encontrada.", StatusCodes.Status404NotFound);
                }

                content = new 
                {
                    ID = produceActivity.ID,
                    UserId = produceActivity.UserId,
                    MonitorEsdId = produceActivity.MonitorEsdId,
                    JigId = produceActivity.JigId,
                    LinkStationAndLineID = produceActivity.LinkStationAndLineID
                };
                await _produceActivityRepository.DeleteAsync(produceActivity.ID);
                return (produceActivity, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }
    }
}
