using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class MonitorEsdService
    {
        private readonly IMonitorEsdRepository _repository;
        private readonly IStationViewRepository _stationViewRepository;

        public MonitorEsdService(IMonitorEsdRepository repository, IStationViewRepository stationViewRepository)
        {
            _repository = repository;
            _stationViewRepository = stationViewRepository;
        }

        public async Task<(object?, int)> GetAllMonitorEsds()
        {
            try
            {
                var monitors = await _repository.GetAllMonitorsAsync();
                if (!monitors.Any())
                {
                    return ("Nenhum monitor cadastrado.", StatusCodes.Status404NotFound);
                }

                return (monitors, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetMonitorId(int id)
        {
            try
            {
                var monitor = await _repository.GetMonitorByIdAsync(id);
                if (monitor == null)
                {
                    return ("Monitor Id não encontrado.", StatusCodes.Status404NotFound);
                }

                return (monitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetMonitorBySerial(string serial)
        {
            try
            {
                var monitor = await _repository.GetMonitorBySerialAsync(serial);
                if (monitor == null)
                {
                    return ("Monitor com Serial Number não encontrado.", StatusCodes.Status404NotFound);
                }

                return (monitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetLogs(string logs)
        {
            try
            {
                var monitor = await _repository.GetLogsAsync(logs);
                if (monitor == null)
                {
                    return ("Nenhum monitor encontrado com os logs fornecidos.", StatusCodes.Status404NotFound);
                }

                return (monitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return ("Status não pode ser nulo ou vazio.", StatusCodes.Status400BadRequest);

            try
            {
                var monitor = await _repository.GetByStatusAsync(status);
                if (monitor == null)
                {
                    return ("Nenhum monitor encontrado com o status fornecido.", StatusCodes.Status404NotFound);
                }

                return (monitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetByOperatorStatus(string statusOperador)
        {
            if (string.IsNullOrWhiteSpace(statusOperador))
                return ("Status do operador não pode ser nulo ou vazio.", StatusCodes.Status400BadRequest);

            try
            {
                var monitor = await _repository.GetByOperatorStatusAsync(statusOperador);
                if (monitor == null)
                {
                    return ("Nenhum monitor encontrado com o status do operador fornecido.", StatusCodes.Status404NotFound);
                }

                return (monitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetByJigStatus(string statusJig)
        {
            if (string.IsNullOrWhiteSpace(statusJig))
                return ("Status do jig não pode ser nulo ou vazio.", StatusCodes.Status400BadRequest);

            try
            {
                var monitor = await _repository.GetByJigStatusAsync(statusJig);
                if (monitor == null)
                {
                    return ("Nenhum monitor encontrado com o status do jig fornecido.", StatusCodes.Status404NotFound);
                }

                return (monitor, StatusCodes.Status200OK);
            }
            catch (Exception exception)
            {
                return (exception.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> Include(MonitorEsdModel monitorModel)
        {
            try
            {
                MonitorEsdModel? existingMonitor = await _repository.GetMonitorByIdAsync(monitorModel.ID);
                bool isNew = existingMonitor == null;

                var response = await _repository.AddOrUpdateAsync(monitorModel);
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
                var monitor = await _repository.GetMonitorByIdAsync(id);
                if (monitor == null)
                {
                    return ($"{id} não encontrado.", StatusCodes.Status404NotFound);
                }
                await _stationViewRepository.DeleteMonitorEsdByStationView(id);
                await _repository.DeleteAsync(monitor.ID);

                var content = new
                {
                    id = monitor.ID,
                    serialNumberEsp = monitor.SerialNumberEsp,
                    description = monitor.Description
                };

                return (content, StatusCodes.Status200OK);
            }
            catch (Exception)
            {
                return ($"{id} não encontrado.", StatusCodes.Status400BadRequest);
            }
        }







        //public async Task<(object?, int)> GetMonitorByIp(string ip)
        //{
        //    try
        //    {
        //        var monitor = await _repository.GetMonitorByIPAsync(ip);
        //        if (monitor == null)
        //        {
        //            return ($"Monitor com IP: {ip} não encontrado.", StatusCodes.Status404NotFound);
        //        }

        //        return (monitor, StatusCodes.Status200OK);
        //    }
        //    catch (Exception exception)
        //    {
        //        return (exception.Message, StatusCodes.Status400BadRequest);
        //    }
        //}
    }
}
