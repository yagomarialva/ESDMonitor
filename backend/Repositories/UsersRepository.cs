using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;

        public UsersRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector;
        }

        public async Task<List<UserModel>> GetAllAsync()
        {
            var result = await _oraConnector.LoadData<UserModel, dynamic>(SQLScripts.UserQueries.GetAllUsers, new { });
            return result ??
                throw new KeyNotFoundException($"Nenhum usuário cadastrado.");
        }
        public async Task<List<UserModel>> GetListUsersAsync(int page, int pageSize)
        {
            var result = await _oraConnector.LoadData<UserModel, dynamic>(SQLScripts.UserQueries.GetPaginatedUsers,
                new { Offset = (page - 1) * pageSize, Limit = pageSize });
            return result.ToList() ?? throw new KeyNotFoundException($"Nenhum operador cadastrado.");
        }

        public async Task<UserModel?> GetByIdAsync(int id)
        {
            var result = await _oraConnector.LoadData<UserModel, dynamic>(SQLScripts.UserQueries.GetUserById, new { id });
            return result.FirstOrDefault();
        }

        public async Task<UserModel?> GetByBadgeAsync(string badge)
        {
           
            var badgeLower = badge.Normalize().ToLower().TrimEnd();

            var result = await _oraConnector.LoadData<UserModel, dynamic>(SQLScripts.UserQueries.GetUserByBadge, new { badgeLower });
            return result.FirstOrDefault();
        }

        public async Task<UserModel?> GetByNameAsync(string name)
        {
          
            var nameLower = name.Normalize().ToLower().TrimEnd();

            var result = await _oraConnector.LoadData<UserModel, dynamic>(SQLScripts.UserQueries.GetUserByName, new { nameLower });
            return result.FirstOrDefault();
        }

        public async Task<UserModel?> AddAsync(UserModel user)
        {
           
            user.Name = user.Name.ToLowerInvariant();
            user.Badge = user.Badge.ToLowerInvariant();

            var formattedDateTime = DateTimeHelperService.GetManausCurrentDateTime();

            var created = user.Created = formattedDateTime;
            var lastUpdate = user.LastUpdated = formattedDateTime;

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.Created = created;
            user.LastUpdated = lastUpdate;
            await _oraConnector.SaveData<UserModel>(SQLScripts.UserQueries.InsertUser, user);
            return await GetByBadgeAsync(user.Badge);
        }

        public async Task<UserModel?> UpdateAsync(UserModel user, int id)
        {
            user.Name = user.Name.ToLowerInvariant();
            user.Badge = user.Badge.ToLowerInvariant();

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            UserModel userModelUp = await GetByIdAsync(id);
            if (userModelUp == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            userModelUp.Badge = user.Badge;
            userModelUp.Name = user.Name;
            userModelUp.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
            await _oraConnector.SaveData<UserModel>(SQLScripts.UserQueries.UpdateUser, userModelUp);
            return userModelUp;
        }

        public async Task<UserModel> DeleteAsync(int id)
        {

            UserModel userModelDel = await GetByIdAsync(id);

            if (userModelDel == null)
            {
                throw new KeyNotFoundException($"User with ID {userModelDel.ID} not found.");
            }

            await _oraConnector.SaveData<dynamic>(SQLScripts.UserQueries.DeleteUser, new { id = id });
            if (_oraConnector.Error != null)
            {
                throw new Exception($"Erro de banco de dados: {_oraConnector.Error}");
            }

            return userModelDel;
        }
        public async Task<int> GetTotalUserCount()
        {
            var totalRecords = await _oraConnector.LoadData<int, dynamic>(SQLScripts.UserQueries.GetCountUsers, new { });
            return totalRecords.FirstOrDefault();
        }
    }
}
