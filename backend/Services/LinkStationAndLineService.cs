using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class LinkStationAndLineService
    {
        private readonly ILinkStationAndLineRepository _repository;
        private readonly IStationRepository _stationRepository;
        private readonly ILineRepository _lineRepository;

        public LinkStationAndLineService(ILinkStationAndLineRepository repository, IStationRepository stationRepository, ILineRepository lineRepository)
        {
            _repository = repository;
            _stationRepository = stationRepository;
            _lineRepository = lineRepository;
        }

        public async Task<(object?, int)> GetAllLinkStationAndLineAsync()
        {
            try
            {
                var links = await _repository.GetAllLinksAsync();
                if (links == null || !links.Any())
                    return ("Nenhum Link cadastrado.", StatusCodes.Status404NotFound);

                await PopulateLinkDetailsAsync(links);
                return (links, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> GetLinkStationAndLineIdAsync(int id)
        {
            try
            {
                var link = await _repository.GetByLinkIdAsync(id);
                return link == null
                    ? ($"{id} não encontrado.", StatusCodes.Status404NotFound)
                    : (link, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> GetLineIdAsync(int id)
        {
            try
            {
                var links = await _repository.GetByLineIdAsync(id);
                if (links == null || !links.Any())
                    return ($"ID {id} não encontrado.", StatusCodes.Status404NotFound);

                await PopulateLinkDetailsAsync(links);
                return (links, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> GetStationIdAsync(int id)
        {
            try
            {
                var links = await _repository.GetByStationIdAsync(id);
                if (links == null || !links.Any())
                    return ($"{id} não encontrado.", StatusCodes.Status404NotFound);

                await PopulateLinkDetailsAsync(links);
                return (links, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> IncludeAsync(LinkStationAndLineModel model)
        {
            try
            {
                await ValidateModelAsync(model);

                var existingCombination = await _repository.GetByLineIdAndStationIdAsync(model.LineID, model.StationID);
                if (existingCombination != null)
                    throw new Exception("Esta combinação já consta na base.");

                var response = await _repository.IncludeAsync(model);
                return (response, StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> DeleteAsync(int id)
        {
            try
            {
                var link = await _repository.GetByLinkIdAsync(id);
                if (link?.ID > 0)
                {
                    await _repository.DeleteAsync(link.ID);
                    return (new { link.ID, link.LineID, link.StationID }, StatusCodes.Status200OK);
                }

                throw new Exception("Dados incorretos ou inválidos.");
            }
            catch (Exception)
            {
                return ($"{id} não encontrado.", StatusCodes.Status400BadRequest);
            }
        }

        private async Task ValidateModelAsync(LinkStationAndLineModel model)
        {
            var currentStation = await _stationRepository.GetByIdAsync(model.StationID);
            var currentLine = await _lineRepository.GetByIdAsync(model.LineID);
            if (currentLine == null || currentStation == null)
                throw new Exception("Referência Id de linha ou estação não é válida.");
        }

        private async Task PopulateLinkDetailsAsync(List<LinkStationAndLineModel> links)
        {
            var stations = await _stationRepository.GetAllAsync();
            var lines = await _lineRepository.GetAllAsync();

            foreach (var link in links)
            {
                link.Station = stations.FirstOrDefault(s => s.ID == link.StationID);
                link.Line = lines.FirstOrDefault(l => l.ID == link.LineID);
            }
        }
    }
}
