using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class UserService
    {
        private readonly IUsersRepository _usersRepository;

        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        // Retorna todos os usuários
        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var result = await _usersRepository.GetAllAsync();
            return result;
        }

        public async Task<List<UserModel>> GetListUsersAsync(int page, int pageSize)
        {
            var result = await _usersRepository.GetListUsersAsync(page, pageSize);
            return result;
        }
        // Retorna usuários por id
        public async Task<UserModel?> GetUserByIdAsync(int id)
        {
            var result = await _usersRepository.GetByIdAsync(id);
            return result;
        }

        // Retorna usuários por matricula
        public async Task<UserModel?> GetUserByBadgeAsync(string badge)
        {
            var result  = await _usersRepository.GetByBadgeAsync(badge);
            return result;
        }

        // Retorna usuários por nome
        public async Task<UserModel?> GetByNameAsync(string name)
        {
            var result = await _usersRepository.GetByNameAsync(name);
            return result;
        }

        // Adiciona um usuário
        public async Task<UserModel?> AddUserAsync(UserModel user)
        {
            var result = await _usersRepository.AddAsync(user);
            return result;
        }

        // Atualiza um usuário ja existente
        public async Task<UserModel?> UpdateUserAsync(UserModel user, int id)
        {
            var result = await _usersRepository.UpdateAsync(user, id);
            return result;
        }

        // Deleta um usuário existente
        public async Task<UserModel> DeleteUserAsync(int id)
        {
            var resul = await _usersRepository.DeleteAsync(id);
            return resul;   
        }
        public async Task<int> GetTotalUsers()
        {
            var result = await _usersRepository.GetTotalUserCount();
            return result;
        }
    }
}
