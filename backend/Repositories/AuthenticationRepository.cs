using BiometricFaceApi.Data;
using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;

namespace BiometricFaceApi.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;
        private readonly BiometricFaceDBContex _dbContext;

        public AuthenticationRepository(IOracleDataAccessRepository oracleConnector, BiometricFaceDBContex dbContext)
        {
            _oraConnector = oracleConnector ?? throw new ArgumentNullException(nameof(oracleConnector));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<AuthenticationModel?> AuthenticateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("O nome de usuário e a senha não podem ser nulos ou vazios.");
            }

            username = username.Trim();
            password = password.Trim();

            var result = await _oraConnector.LoadData<AuthenticationModel, dynamic>(
                SQLScripts.AuthenticationQueries.AuthenticateUser, new { username, password });

            return result.FirstOrDefault();
        }
        public async Task<AuthenticationModel?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("O ID deve ser maior que 0.", nameof(id));
            }

            var result = await _oraConnector.LoadData<AuthenticationModel, dynamic>(
                SQLScripts.AuthenticationQueries.GetAuthenticationById, new { id });

            return result.FirstOrDefault();
        }
        public async Task<AuthenticationModel?> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("O nome de usuário não pode ser nulo ou vazio.", nameof(username));
            }

            var result = await _oraConnector.LoadData<AuthenticationModel, dynamic>(
                SQLScripts.AuthenticationQueries.GetAuthenticationByUserName, new { username });

            return result.FirstOrDefault();
        }
        public async Task<AuthenticationModel?> GetByBadgeAsync(string? badge)
        {
            var result = await _oraConnector.LoadData<AuthenticationModel, dynamic>(
                SQLScripts.AuthenticationQueries.GetAuthenticationByBadge, new { badge });

            return result.FirstOrDefault();
        }
        public async Task<AuthenticationModel?> AddAsync(AuthenticationModel userAuth)
        {
            if (userAuth == null)
            {
                throw new ArgumentNullException(nameof(userAuth));
            }
            userAuth.Created = DateTimeHelperService.GetManausCurrentDateTime();
            userAuth.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
            await _oraConnector.SaveData<AuthenticationModel>(SQLScripts.AuthenticationQueries.InsertAuthentication, userAuth);
            CheckForErrors();

            return await GetByUsernameAsync(userAuth.Username);
        }
        public async Task<AuthenticationModel?> UpdateAsync(AuthenticationModel userAuth, int id)
        {
            if (userAuth == null)
            {
                throw new ArgumentNullException(nameof(userAuth));
            }

            var existingAuth = await GetByIdAsync(id);
            if (existingAuth == null)
            {
                throw new KeyNotFoundException($"Usuário com ID: {id} não foi encontrado no banco de dados.");
            }
            userAuth.ID = existingAuth.ID;
            userAuth.LastUpdated = DateTimeHelperService.GetManausCurrentDateTime();
            await _oraConnector.SaveData<AuthenticationModel>(SQLScripts.AuthenticationQueries.UpdateAuthentication, userAuth);
            CheckForErrors();

             // Guarda o ID original
            return userAuth;
        }
        public async Task<AuthenticationModel?> DeleteAsync(int id)
        {
            var authToDelete = await GetByIdAsync(id);
            if (authToDelete == null)
            {
                throw new KeyNotFoundException($"Usuário com ID: {id} não foi encontrado no banco de dados.");
            }

            await _oraConnector.SaveData<dynamic>(SQLScripts.AuthenticationQueries.DeleteAuthentication, new { id });
            CheckForErrors();

            return authToDelete;
        }
        private void CheckForErrors()
        {
            if (_oraConnector.Error != null)
            {
                throw new Exception($"Falha na operação do banco de dados: {_oraConnector.Error}");
            }
        }
    }
}
