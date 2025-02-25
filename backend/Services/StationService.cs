using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;


namespace BiometricFaceApi.Services
{
    public class StationService
    {
        private readonly IStationRepository _stationRepository;

        public StationService(IStationRepository stationRepository)
        {
            _stationRepository = stationRepository;
        }

        public async Task<(object?, int)> GetAllStationsAsync()
        {
            try
            {
                var stations = await _stationRepository.GetAllAsync();
                return stations.Any()
                    ? (stations, StatusCodes.Status200OK)
                    : ("Estação não encontrada.", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> GetStationByIdAsync(int id)
        {
            try
            {
                var station = await _stationRepository.GetByIdAsync(id);
                return (station, StatusCodes.Status200OK);
            }
            catch (KeyNotFoundException ex)
            {
                return (ex.Message, StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> GetStationByNameAsync(string name)
        {
            try
            {
                var station = await _stationRepository.GetByNameAsync(name);
                return (station, StatusCodes.Status200OK);
            }
            catch (KeyNotFoundException ex)
            {
                return (ex.Message, StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object?, int)> AddOrUpdateStationAsync(StationModel stationModel)
        {
            try
            {
                var station = await _stationRepository.AddOrUpdateAsync(stationModel);
                return (station, stationModel.ID > 0 ? StatusCodes.Status200OK : StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object? result , int statusCode)> DeleteStationAsync(int id)
        {
            try
            {
                var deleted = await _stationRepository.GetByIdAsync(id);
                if (deleted == null) 
                {
                    return ("Estação não encontrada ou já excluido.", StatusCodes.Status404NotFound);
                }
                await _stationRepository.DeleteAsync(id);
                return (deleted, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }
        
    }
}
