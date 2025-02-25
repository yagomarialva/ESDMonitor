using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    /// <summary>
    /// Serviço para gerenciar funções de usuário.
    /// </summary>
    public class RolesService
    {
        private readonly IRolesRepository _repository;

        public RolesService(IRolesRepository repository)
        {
            _repository = repository;
        }
        public async Task<(object? Result, int StatusCode)> GetAllRolesAsync()
        {
            try
            {
                var roles = await _repository.GetAllRolesAsync();
                if (!roles.Any())
                {
                    return ("Nenhuma função cadastrada.", StatusCodes.Status400BadRequest);
                }
                return (roles, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object? Result, int StatusCode)> GetRoleByIdAsync(int id)
        {
            try
            {
                var role = await _repository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return ("ID da função não encontrado.", StatusCodes.Status404NotFound);
                }
                return (role, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object? Result, int StatusCode)> GetRoleByNameAsync(string rolesName)
        {
            try
            {
                var role = await _repository.GetRoleByNameAsync(rolesName);
                if (role == null)
                {
                    return ("Nome da função não encontrado.", StatusCodes.Status404NotFound);
                }
                return (role, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object? Result, int StatusCode)> IncludeRoleAsync(RolesModel model)
        {
            try
            {
                var response = await _repository.AddOrUpdateRoleAsync(model);

                return (response, StatusCodes.Status200OK);
            }
            catch
            {
                return ("Dados já cadastrados ou incorretos.", StatusCodes.Status400BadRequest);
            }
        }

        public async Task<(object? Result, int StatusCode)> DeleteRoleAsync(int id)
        {
            try
            {
                var role = await _repository.GetRoleByIdAsync(id);
                if (role?.ID > 0)
                {
                    await _repository.DeleteRoleAsync(role.ID);
                    return (new { role.ID, role.RolesName }, StatusCodes.Status200OK);
                }
                return ("Dados incorretos ou inválidos.", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }
    }
}
