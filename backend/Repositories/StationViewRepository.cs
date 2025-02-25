using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class StationViewRepository : IStationViewRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;
        private readonly ILinkStationAndLineRepository _linkStationAndLineRepository;
        private readonly IMonitorEsdRepository _monitorEsdRepository;
        private readonly IStationRepository _stationRepository;
        private readonly ILineRepository _lineRepository;

        public StationViewRepository(IOracleDataAccessRepository oraConnector,
            ILinkStationAndLineRepository linkStationAndLineRepository,
            IMonitorEsdRepository monitorEsdRepository,
            IStationRepository stationRepository,
            ILineRepository lineRepository)
        {
            _oraConnector = oraConnector;
            _linkStationAndLineRepository = linkStationAndLineRepository;
            _monitorEsdRepository = monitorEsdRepository;
            _stationRepository = stationRepository;
            _lineRepository = lineRepository;
        }

        public async Task<List<StationViewModel>> GetAllStationViewsAsync()
        {
            var result = await _oraConnector.LoadData<StationViewModel, dynamic>(SQLScripts.StationViewQueries.GetAllStationView, new { });
            return result;
        }
        public async Task<StationViewModel?> GetStationViewByIdAsync(int id)
        {
            
            var result = await _oraConnector.LoadData<StationViewModel, dynamic>(SQLScripts.StationViewQueries.GetStationViewById, new { id });
            return result.FirstOrDefault();
        }
        public async Task<StationViewModel?> GetByPositionSequenceIdAsync(int positionId)
        {
            var result = await _oraConnector.LoadData<StationViewModel, dynamic>(SQLScripts.StationViewQueries.GetStationViewPositionById, new { positionId });
            return result.FirstOrDefault();
        }
        public async Task<StationViewModel?> GetStationViewByLinkStAndLineByIdAsync(int linkStationAndLineId, int seguenceNumber)
        {
            var result = await _oraConnector.LoadData<StationViewModel, dynamic>(SQLScripts.StationViewQueries.GetStationViewPositionByLinkId, new { linkStationAndLineId, seguenceNumber });
            return result.FirstOrDefault();
        }
        public async Task<StationViewModel?> AddOrUpdateAsync(StationViewModel stationView)
        {
            StationViewModel? existingStationView = await GetStationViewByIdAsync(stationView.ID);
            return existingStationView == null
                ? await InsertStationViewAsync(stationView)
                : await UpdateStationViewAsync(existingStationView, stationView);
        }
        private async Task<StationViewModel?> InsertStationViewAsync(StationViewModel stationView)
        {
            // Verifique se o LinkStationAndLineId é válido
            var linkDetails = await _linkStationAndLineRepository.GetByLinkIdAsync(stationView.LinkStationAndLineId);
            if (linkDetails == null)
                throw new Exception("Id de link é inválido!");

            stationView.LinkStationAndLine = linkDetails;
            stationView.LinkStationAndLine.Station = await _stationRepository.GetByIdAsync(stationView.LinkStationAndLine.StationID);

            // Verifique se o MonitorEsdId é válido
            var monitorEsd = await _monitorEsdRepository.GetMonitorByIdAsync(stationView.MonitorEsdId);
            if (monitorEsd == null)
                throw new Exception("Monitor ESD não encontrado!");

            // Checa se a combinação de monitor ESD e estação já existe
            var stationViews = await GetAllStationViewsAsync();
            var linkWithMonitor = stationViews
                .Where(link => (link.MonitorEsdId == stationView.MonitorEsdId && link.LinkStationAndLineId == stationView.LinkStationAndLineId) || link.MonitorEsdId == stationView.MonitorEsdId)
                .ToList();

            if (linkWithMonitor.Any())
                throw new Exception("A combinação de monitor ESD e estação já existe ou monitor ESD já está vinculado com outra estação!");

            // Verifica a capacidade máxima de monitores
            var links = stationViews.Where(x => x.LinkStationAndLineId == stationView.LinkStationAndLineId).ToList();
            var counterLinks = links.Count();
            var maxQuantity = stationView.LinkStationAndLine.Station?.SizeY * stationView.LinkStationAndLine.Station?.SizeX;

            if (counterLinks >= maxQuantity)
                throw new Exception($"Quantidade de monitores ESD excedida, máximo {maxQuantity}!");

            // Chama o método para verificar a disponibilidade da posição
            await CheckPositionAvailabilityAsync(stationView, counterLinks);

            // Insere a nova StationView
            stationView.Created = DateTimeHelperService.GetManausCurrentDateTime();
            stationView.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
            //stationView.PositionSequence = counterLinks;
            await _oraConnector.SaveData(SQLScripts.StationViewQueries.InsertStationView, stationView);
            CheckForError();
            return stationView;
        }
        private async Task CheckPositionAvailabilityAsync(StationViewModel stationView, int counterLinks)
        {
            // Verifique se a posição especificada já está ocupada
            if (stationView.PositionSequence >= 0)
            {
                var existingStationViewAtPosition = await GetStationViewByLinkStAndLineByIdAsync(stationView.PositionSequence, stationView.LinkStationAndLineId);
                if (existingStationViewAtPosition != null)
                {
                    throw new Exception("Já existe uma estação cadastrada nesta posição.");
                }
            }
            else
            {
                // Se a posição não foi especificada, adiciona na próxima posição disponível
                stationView.PositionSequence = counterLinks; // ou outra lógica para determinar a posição
            }
        }
        private async Task<StationViewModel?> UpdateStationViewAsync(StationViewModel existingStationView, StationViewModel updatedStationView)
        {
            //armazena data atual 
            var lastUpdated = updatedStationView.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();

            var linkDetails = await _linkStationAndLineRepository.GetByLinkIdAsync(existingStationView.LinkStationAndLineId);
            var currentMonitorEsd = await _monitorEsdRepository.GetMonitorByIdAsync(existingStationView.MonitorEsdId);

            existingStationView.MonitorEsd = currentMonitorEsd;
            existingStationView.LinkStationAndLine = linkDetails;
            existingStationView.LinkStationAndLine.Station = await _stationRepository.GetByIdAsync(existingStationView.LinkStationAndLine.StationID);

            var maxQuantity = linkDetails?.Station?.SizeY * linkDetails?.Station?.SizeX ?? 0;

            if (updatedStationView.PositionSequence < 0 || updatedStationView.PositionSequence >= maxQuantity)
                throw new Exception("A sequência informada não é válida.");

            // Chama o método para verificar a disponibilidade da posição
            await CheckPositionAvailabilityAsync(updatedStationView, maxQuantity); // Aqui, maxQuantity é a quantidade máxima de posições disponíveis

            var newMonitorEsd = await _monitorEsdRepository.GetMonitorByIdAsync(updatedStationView.MonitorEsdId);
            var newLinkStationAndLine = await _linkStationAndLineRepository.GetByLinkIdAsync(updatedStationView.LinkStationAndLineId);

            ValidateUpdateParameters(newMonitorEsd, newLinkStationAndLine);

            newLinkStationAndLine.Line = await _lineRepository.GetByIdAsync(newLinkStationAndLine.LineID);
            newLinkStationAndLine.Station = await _stationRepository.GetByIdAsync(newLinkStationAndLine.StationID);

            updatedStationView.LastUpdated = lastUpdated;
            await HandleMonitorEsdUpdate(existingStationView, updatedStationView, newMonitorEsd, newLinkStationAndLine);
            await _oraConnector.SaveData(SQLScripts.StationViewQueries.UpdateStationView, existingStationView);
            CheckForError();
            return existingStationView;
        }
        private void ValidateUpdateParameters(MonitorEsdModel? newMonitorEsd, LinkStationAndLineModel? newLinkStationAndLine)
        {
            if (newMonitorEsd == null)
                throw new Exception($"Id {newMonitorEsd.ID} de Monitor ESD é inválido");
            if (newLinkStationAndLine == null)
                throw new Exception($"Id {newLinkStationAndLine.ID} de sessão de linha é inválido");
        }
        private async Task HandleMonitorEsdUpdate(StationViewModel existingStationView, StationViewModel updatedStationView, MonitorEsdModel newMonitorEsd, LinkStationAndLineModel newLinkStationAndLine)
        {
            var allStationViews = await GetAllStationViewsAsync();

            // Verifique se o monitor ESD não está vinculado a outra estação
            var currentLinkStationAndLineOfNewMonitorEsd = allStationViews
                .FirstOrDefault(link => link.MonitorEsdId == newMonitorEsd.ID);

            if (currentLinkStationAndLineOfNewMonitorEsd == null)
            {
                existingStationView.MonitorEsdId = newMonitorEsd.ID;
                existingStationView.MonitorEsd = newMonitorEsd;
                existingStationView.PositionSequence = updatedStationView.PositionSequence;
            }
            else if (!existingStationView.LinkStationAndLineId.Equals(currentLinkStationAndLineOfNewMonitorEsd.LinkStationAndLineId))
            {
                throw new Exception($"Não é possível alterar id de vínculo de estações. Estação de id {updatedStationView.LinkStationAndLineId} e Monitor ESD de id {updatedStationView.MonitorEsdId} " +
                    $"está sendo relacionado com a Estação de id {existingStationView.LinkStationAndLineId} e Monitor ESD de id {existingStationView.MonitorEsdId}");
            }
        }
        public async Task<StationViewModel> DeleteAsync(int id)
        {
            StationViewModel? stationViewDel = await GetStationViewByIdAsync(id);
            if (stationViewDel == null)
                throw new Exception($"StationView com ID:{id} não foi encontrado no banco de dados.");
        
            await _oraConnector.SaveData<dynamic>(SQLScripts.StationViewQueries.DeleteStationView, new { id });
            return stationViewDel;
        }
        public async Task <StationViewModel?>DeleteMonitorEsdByStationView(int id)
        {
            StationViewModel? stwMonitor = await GetStationViewByMonitorIdAsync(id);
            await _oraConnector.SaveData<dynamic>(SQLScripts.StationViewQueries.Delete_MonitorByStationView, new { id });
            return stwMonitor;
        }
        public async Task<StationViewModel?> GetStationViewByMonitorIdAsync(int id)
        {
            var result = await _oraConnector.LoadData<StationViewModel, dynamic>(SQLScripts.StationViewQueries.GetMonitorByIdInST, new { id });
            return result.FirstOrDefault();
        }
        private void CheckForError()
        {
            if (_oraConnector.Error != null)
                throw new Exception($"Error : {_oraConnector.Error}");
        }
    }
}
