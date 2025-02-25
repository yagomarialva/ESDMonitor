using BiometricFaceApi.Auth;
using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Security;

namespace BiometricFaceApi.Services
{
    public class AuthenticationService
    {
        private IAuthenticationRepository _authRepository;
        private readonly SecurityService _securityService;
        private readonly IRolesRepository _rolesRepository;
        


        public AuthenticationService(
            IAuthenticationRepository authRepository,
            SecurityService securityService,
            IRolesRepository rolesRepository,
            JwtAuthentication twt)
        {
            _authRepository = authRepository;
            _securityService = securityService;
            _rolesRepository = rolesRepository;
            
        }

        // Autenticar usuário
        public async Task<AuthenticationModel?> AuthenticateUserAsync(LoginModel login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
            {
                return null; // Retorna null se as credenciais estiverem vazias ou inválidas
            }

            var encryptedPassword = _securityService.EncryptAES(login.Password);
            return await _authRepository.AuthenticateUserAsync(login.Username.Trim(), encryptedPassword);
        }

        // Gerenciar autenticação: atualizar ou inserir usuário
        public async Task<(object? Content, int StatusCode)> ManageAuthAsync(AuthenticationModel auth)
        {
            if (auth == null)
            {
                return ("Objeto de autenticação inválido!", StatusCodes.Status400BadRequest);
            }

            try
            {
                // Verificar se o perfil (role) é válido
                var roles = await _rolesRepository.GetAllRolesAsync();
                if (roles != null && roles.All(r => r.RolesName?.ToLower() != auth.RolesName?.ToLower()))
                {
                    throw new Exception("Perfil selecionado é inválido!");
                }

                auth.Password = _securityService.EncryptAES(auth.Password);

                // Verificar se o usuário já existe
                var existingAuth = await _authRepository.GetByBadgeAsync(auth.Badge);
                if (existingAuth != null && existingAuth.ID > 0)
                {
                    // Atualizar usuário existente
                    await _authRepository.UpdateAsync(auth, existingAuth.ID);
                    return (new { existingAuth.ID, auth.Badge, auth.Username, auth.RolesName }, StatusCodes.Status200OK);
                }
                else
                {
                    // Inserir novo usuário
                    auth.RolesName = auth.RolesName?.ToLower();
                    var addedAuth = await _authRepository.AddAsync(auth);
                    if (addedAuth == null)
                    {
                        throw new Exception("Dados incorretos ou inválidos.");
                    }

                    return (addedAuth, StatusCodes.Status201Created);
                }
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        // Buscar usuário autenticado por ID
        public async Task<(object? Content, int StatusCode)> GetAuthByIdAsync(int authId)
        {
            try
            {
                var auth = await _authRepository.GetByIdAsync(authId);
                if (auth == null || auth.ID <= 0)
                {
                    return ("Usuário não encontrado.", StatusCodes.Status404NotFound);
                }

                return (new
                {
                    auth.ID,
                    auth.Username,
                    auth.Badge,
                    auth.RolesName
                }, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        // Excluir usuário autenticado por ID
        public async Task<(object? Content, int StatusCode)> DeleteAuthAsync(int id)
        {
            try
            {
                var auth = await _authRepository.GetByIdAsync(id);
                if (auth == null || auth.ID <= 0)
                {
                    return ("Usuário não encontrado.", StatusCodes.Status404NotFound);
                }

                await _authRepository.DeleteAsync(auth.ID);

                return (new
                {
                    auth.ID,
                    auth.Username,
                    auth.Badge
                }, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }
    }
}
