using BiometricFaceApi.Auth;
using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Security;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IAuthenticationRepository _authRepository;
        private readonly AuthenticationService _authService;
        private readonly SecurityService _securityService;
        private readonly JwtAuthentication _jwtAuthentication;
        public AuthenticationController(IAuthenticationRepository authRepository,
                                    JwtAuthentication jwtAuthentication,
                                    SecurityService securityService,
                                    AuthenticationService authService)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            _jwtAuthentication = jwtAuthentication ?? throw new ArgumentNullException(nameof(jwtAuthentication));
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }


        /// <summary>
        /// Autenticação de usuários cadastrados no banco de dados.
        /// </summary>
        /// <param name="login">Modelo de login com Username e Password.</param>
        /// <response code="200">Retorna dados do usuário autenticado.</response>
        /// <response code="401">Usuário não cadastrado.</response>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest("Username e Password são obrigatórios.");
            }

            var user = await _authService.AuthenticateUserAsync(login);
            if (user == null)
            {
                return Unauthorized("Login ou Password incorretos ou inválido.");
            }

            var tokenString = _jwtAuthentication.GenerateJSONWebToken(user);
            return Ok(new
            {
                Token = tokenString,
                Role = user.RolesName,
                Name = user.Username
            });
        }

        /// <summary>
        /// Cadastra e Atualiza dados de usuários.
        /// </summary>
        /// <param name="authModel">Dados de cadastro do usuário.</param>
        /// <response code="200">Dados atualizados com sucesso.</response>
        /// <response code="201">Dados cadastrados com sucesso.</response>
        /// <response code="400">Dados incorretos ou inválidos.</response>
        /// <response code="500">Erro do servidor interno!</response>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpPost("criacao")]
        public async Task<IActionResult> InsertAuths([FromBody] AuthenticationModel authModel)
        {
            var (result, statusCode) = await _authService.ManageAuthAsync(authModel);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Busca usuários pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <response code="200">Retorna dados do usuário.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <response code="500">Erro do servidor interno!</response>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthsById(int id)
        {
            var (response, statusCode) = await _authService.GetAuthByIdAsync(id);
            if (statusCode == StatusCodes.Status200OK)
            {
                return Ok(response);
            }
            return StatusCode(statusCode, response);
        }

        /// <summary>
        /// Deletar usuários pelo ID.
        /// </summary>
        /// <param name="id">ID do usuário a ser deletado.</param>
        /// <response code="200">Usuário removido com sucesso.</response>
        /// <response code="404">Usuário não encontrado.</response>
        /// <response code="500">Erro do servidor interno!</response>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAtuhs(int id)
        {
            var (result, statusCode) = await _authService.DeleteAuthAsync(id);
            return StatusCode(statusCode, result);
        }
    }
}
