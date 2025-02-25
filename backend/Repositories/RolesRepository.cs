using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;

        public RolesRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector;
        }
        public async Task<IEnumerable<RolesModel>> GetAllRolesAsync()
        {
            var result = await _oraConnector.LoadData<RolesModel, dynamic>(SQLScripts.RolesQueries.GetAllRoles, new { });
            return result ??
                throw new KeyNotFoundException($"Roles não encontrada.");
        } 
        public async Task<RolesModel?> GetRoleByIdAsync(int id)
        {
            var result = await _oraConnector.LoadData<RolesModel, dynamic>(SQLScripts.RolesQueries.GetRolesById, new { id });
            return result.FirstOrDefault() ?? 
                throw new KeyNotFoundException($"Função com ID {id} não encontrada.");
        }
        public async Task<RolesModel?> GetRoleByNameAsync(string roleName)
        {
            //Tranforma name para letras minusculas, verifica se existe caracters especiais e tira os espçao no final da palavra.
            var roleLower = roleName.Normalize().ToLower().TrimEnd();

            var result = await _oraConnector.LoadData<RolesModel, dynamic>(SQLScripts.RolesQueries.GetRolesByRolesName, new { roleLower });
            return result.FirstOrDefault() ?? 
                throw new KeyNotFoundException($"Função com nome {roleName} não encontrada.");
        }
        public async Task<RolesModel?> AddOrUpdateRoleAsync(RolesModel roleModel)
        {
            
            RolesModel rolesUp;
           
            roleModel.RolesName = roleModel.RolesName.ToLowerInvariant();

            if (roleModel.ID > 0)
            {
                
                roleModel.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                await _oraConnector.SaveData(SQLScripts.RolesQueries.UpdateRoles, roleModel);
                rolesUp = roleModel;
            }
            else
            {
               
                roleModel.Created = DateTimeHelperService.GetManausCurrentDateTime();
                roleModel.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
                await _oraConnector.SaveData(SQLScripts.RolesQueries.InsertRoles, roleModel);
                rolesUp = roleModel;
            }

           
            if (_oraConnector.Error != null)
            {
                throw new Exception($"Erro: {_oraConnector.Error}");
            }

         
            return rolesUp;
        }
        public async Task<RolesModel?> DeleteRoleAsync(int id)
        {
            var roleToDelete = await GetRoleByIdAsync(id);
            await _oraConnector.SaveData<dynamic>(SQLScripts.RolesQueries.DeleteRoles, new { id });
            return roleToDelete;
        }
    }
}
