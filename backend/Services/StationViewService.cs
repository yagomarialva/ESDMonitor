using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class StationViewService
    {
        private readonly IStationViewRepository _stationViewRepository;
        private readonly IMonitorEsdRepository _monitorEsdRepository;
        private readonly ILinkStationAndLineRepository _linkRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IStationRepository _stationRepository;
        private readonly LinkStationAndLineService _linkStationAndLineService;
        private readonly ILogMonitorEsdRepository _logMonitorEsdRepository;
        private readonly ILastLogMonitorEsdRepository _lastLogMonitorRepository;

        public StationViewService(IStationViewRepository stationViewRepository,
                                  IMonitorEsdRepository monitorEsdRepository,
                                  ILinkStationAndLineRepository linkStationAndLineRepository,
                                  ILineRepository lineRepository,
                                  IStationRepository stationRepository,
                                  ILogMonitorEsdRepository logMonitorEsdRepository,
                                  ILastLogMonitorEsdRepository lastLogMonitorRepository)
        {
            _stationViewRepository = stationViewRepository;
            _monitorEsdRepository = monitorEsdRepository;
            _linkRepository = linkStationAndLineRepository;
            _lineRepository = lineRepository;
            _stationRepository = stationRepository;
            _linkStationAndLineService = new LinkStationAndLineService(linkStationAndLineRepository, stationRepository, lineRepository);
            _logMonitorEsdRepository = logMonitorEsdRepository;
            _lastLogMonitorRepository = lastLogMonitorRepository;
        }

        public async Task<(object? Result, int StatusCode)> GetAllStationView()
        {
            try
            {
                var stationViews = await LoadFactoryViewDataAsync();
                return stationViews.Any()
                    ? (stationViews, StatusCodes.Status200OK)
                    : ("Nenhuma Linha de Produção foi encontrada.", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        public async Task<(object? Result, int StatusCode)> GetStationViewById(int id)
        {
            try
            {
                var stationView = await _stationViewRepository.GetStationViewByIdAsync(id);
                return stationView != null
                    ? (stationView, StatusCodes.Status200OK)
                    : ("Linha de Produção não encontrada.", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        public async Task<(object? Result, int StatusCode)> Include(StationViewModel stationViewModel)
        {
            var allStation = await LoadAllStationViewsAsync();
            var isExistCombination = allStation.Find(
                station => station.LinkStationAndLineId ==
                stationViewModel.LinkStationAndLineId && station.MonitorEsdId ==
                stationViewModel.MonitorEsdId) != null;


            if (stationViewModel.MonitorEsdId <= 0 || stationViewModel.LinkStationAndLineId <= 0)
            {
                return ("Todos os campos são obrigatórios.", StatusCodes.Status409Conflict);
            }
            if (await IsExistCombination(stationViewModel))
            {
                return ("O monitor ESD já está vinculado a uma estação.", StatusCodes.Status409Conflict);
            }

            try
            {
                var result = await _stationViewRepository.AddOrUpdateAsync(stationViewModel);
                return (result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return HandleIncludeException(ex);
            }
        }
        public async Task<(object? Content, int StatusCode)> Delete(int id)
        {
            try
            {
                var stationView = await _stationViewRepository.GetStationViewByIdAsync(id);
                if (stationView == null)
                {
                    return ("Dados incorretos ou inválidos", StatusCodes.Status404NotFound);
                }

                var content = new
                {
                    stationView.ID,
                    stationView.MonitorEsdId,
                    stationView.LinkStationAndLineId
                };

                await _stationViewRepository.DeleteAsync(stationView.ID);
                return (content, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        public async Task<(object? Content, int StatusCode)> FactoryView()
        {
            try
            {
                var lineViews = await LoadFactoryViewDataAsync();
                if (lineViews == null || !lineViews.Any())
                {
                    return ("Nenhum Post cadastrado.", StatusCodes.Status404NotFound);
                }
                return (lineViews, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        private async Task<List<StationViewModel>> LoadAllStationViewsAsync()
        {
            var monitorEds = await _monitorEsdRepository.GetAllMonitorsAsync();
            var (linkData, _) = await _linkStationAndLineService.GetAllLinkStationAndLineAsync();
            var linkStationAndLines = linkData as List<LinkStationAndLineModel> ?? new List<LinkStationAndLineModel>();
            var stationView = await _stationViewRepository.GetAllStationViewsAsync();

            stationView.ForEach(station =>
            {
                station.LinkStationAndLine = linkStationAndLines.Find(link => link.ID == station.LinkStationAndLineId);
                station.MonitorEsd = monitorEds.Find(monitor => monitor.ID == station.MonitorEsdId);
            });

            return stationView;
        }
        private async Task<List<LineView>> LoadFactoryViewDataAsync()
        {
            // Carregar todos os dados de uma vez
            var stationViewsAll = await _stationViewRepository.GetAllStationViewsAsync();
            var linksAll = await _linkRepository.GetAllLinksAsync();
            var linesAll = await _lineRepository.GetAllAsync();
            var stationsAll = await _stationRepository.GetAllAsync();
            var monitorsAll = await _monitorEsdRepository.GetAllMonitorsAsync();
            var logMonitorAll = await _logMonitorEsdRepository.GetLogIncreasingForStationviewAsync();
            var lastLogsMonitorAll = await _lastLogMonitorRepository.GetAllLastLogsAsync();

            // Converter listas para dicionários
            var linesById = linesAll.ToDictionary(line => line.ID);
            var stationsById = stationsAll.ToDictionary(station => station.ID);
            var monitorsById = monitorsAll.ToDictionary(monitor => monitor.ID);

            // Agrupar logs por tipo de mensagem para otimizar filtragem
            var latestLogsByMonitor = lastLogsMonitorAll
                .Where(log => log.MessageType.Contains("operador") || log.MessageType.Contains("jig"))
                .GroupBy(log => new { log.MonitorEsdId, log.MessageType })
                .Select(group => group.OrderByDescending(log => log.Created).FirstOrDefault())
                .ToList();

            var logOperatorByMonitor = latestLogsByMonitor
                .Where(log => log.MessageType.Contains("operador"))
                .ToDictionary(log => log.MonitorEsdId);

            var logJigByMonitor = latestLogsByMonitor
                .Where(log => log.MessageType.Contains("jig"))
                .ToDictionary(log => log.MonitorEsdId);

            // Construir a lista de LineView
            var lineViews = linksAll
                .GroupBy(link => link.LineID)
                .Select(group =>
                {
                    var lineId = group.Key;

                    // Criar LineView com suas estações e monitores
                    return new LineView
                    {
                        Line = linesById[lineId],
                        Stations = group
                            .OrderBy(link => link.OrdersList)
                            .Select(link => new StationView
                            {
                                LinkStationAndLineID = link.ID,
                                Station = stationsById[link.StationID],
                                StationViewID = stationViewsAll.FirstOrDefault(sv => sv.LinkStationAndLineId == link.ID)?.ID ?? 0,
                                MonitorsEsd = stationViewsAll
                                    .Where(stationView => stationView.LinkStationAndLineId == link.ID)
                                    .OrderBy(stationView => stationView.PositionSequence)
                                    .Select(stationView => new MonitorEsdView
                                    {
                                        MonitorsEsd = monitorsById[stationView.MonitorEsdId],
                                        PositionSequence = stationView.PositionSequence,
                                        LogOperator = logOperatorByMonitor.GetValueOrDefault(stationView.MonitorEsdId),
                                        LogJig = logJigByMonitor.GetValueOrDefault(stationView.MonitorEsdId)
                                    })
                                    .ToList()
                            })
                            .ToList()
                    };
                })
                .OrderBy(lineView => lineView.Line.ID)
                .ToList();

            return lineViews;
        }
        private async Task<bool> IsExistCombination(StationViewModel stationViewModel)
        {
            var allStations = await LoadAllStationViewsAsync();
            return allStations.Any(st =>
                st.LinkStationAndLineId == stationViewModel.LinkStationAndLineId &&
                st.MonitorEsdId == stationViewModel.MonitorEsdId);
        }
        private (object? Content, int StatusCode) HandleException(Exception ex)
        {
            // Log the exception if necessary
            return (ex.Message ?? "Erro ao processar a solicitação.", StatusCodes.Status400BadRequest);
        }
        private (object? Content, int StatusCode) HandleIncludeException(Exception ex)
        {
            return (ex.Message ?? "Não foi possível salvar as alterações. Verifique se todos os itens estão cadastrados.", StatusCodes.Status400BadRequest);
        }
    }
}
