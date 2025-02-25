using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class LinkStationAndLineRepository : ILinkStationAndLineRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;

        public LinkStationAndLineRepository(IOracleDataAccessRepository oracleDataAccessRepository)
        {
            _oraConnector = oracleDataAccessRepository;
        }
        public async Task<List<LinkStationAndLineModel>> GetAllLinksAsync()
        {
            var result = await _oraConnector.LoadData<LinkStationAndLineModel, dynamic>(SQLScripts.LinkStationAndLineQueries.GetAllLinks, new { });
            return result;
        }
        public async Task<List<LinkStationAndLineModel>?> GetByLineIdAsync(int lineId)
        {
            var result = await _oraConnector.LoadData<LinkStationAndLineModel, dynamic>(SQLScripts.LinkStationAndLineQueries.GetByLineId, new { lineId });
            return result;
        }
        public async Task<LinkStationAndLineModel?> GetByLinkIdAsync(int id)
        {
            var result = await _oraConnector.LoadData<LinkStationAndLineModel, dynamic>(SQLScripts.LinkStationAndLineQueries.GetByLinkId, new { id });
            return result.FirstOrDefault();
        }
        public async Task<List<LinkStationAndLineModel>?> GetByStationIdAsync(int stationId)
        {
            var result = await _oraConnector.LoadData<LinkStationAndLineModel, dynamic>(SQLScripts.LinkStationAndLineQueries.GetByLinkStationId, new { stationId });
            return result;
        }
        public async Task<LinkStationAndLineModel?> GetByLineIdAndStationIdAsync(int lineId, int stationId)
        {
            var result = await _oraConnector.LoadData<LinkStationAndLineModel, dynamic>(SQLScripts.LinkStationAndLineQueries.GetByLinkLineAndStationById, new { lineId, stationId });
            return result.FirstOrDefault();
        }
        public async Task<LinkStationAndLineModel?> IncludeAsync(LinkStationAndLineModel model)
        {
            if (model.ID > 0)
            {
                model.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                await _oraConnector.SaveData<LinkStationAndLineModel>(SQLScripts.LinkStationAndLineQueries.UpdateLinkAndStation, model);
                CheckForErrors();
                return model;
            }
            else
            {
                model.Created = DateTimeHelperService.GetManausCurrentDateTime();
                model.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                await _oraConnector.SaveData<LinkStationAndLineModel>(SQLScripts.LinkStationAndLineQueries.InsertLinkAndStation, model);
                CheckForErrors();
                return await GetByLineIdAndStationIdAsync(model.LineID, model.StationID);
            }
        }
        public async Task<LinkStationAndLineModel?> DeleteAsync(int id)
        {
            LinkStationAndLineModel? linkAndStationDel = await GetByLinkIdAsync(id);
            await _oraConnector.SaveData<dynamic>(SQLScripts.LinkStationAndLineQueries.DeleteLinkAndStation, new { id });
            return linkAndStationDel;
        }

        private void CheckForErrors()
        {
            if (_oraConnector.Error != null)
                throw new Exception($"Error: {_oraConnector.Error}");
        }
    }
}
