using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiometricController : ControllerBase
    {
        protected readonly UserService _userService;
        protected readonly ImageService _imageService;
        protected readonly BiometricService _biometricService;
        protected readonly JigService _jigRepository;
        private readonly StatusJigAndUserService _statusJigAndUserService;
        private readonly ILastLogMonitorEsdRepository _lastLogMonitorEsdRepository;


        public BiometricController(UserService userService,
                                ImageService imageService,
                                BiometricService biometricService, JigService jigRepository, StatusJigAndUserService statusJigAndUserService, ILastLogMonitorEsdRepository lastLogMonitorEsdRepository)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _biometricService = biometricService ?? throw new ArgumentNullException(nameof(biometricService));
            _jigRepository =  jigRepository ?? throw new ArgumentNullException(nameof(jigRepository));
            _statusJigAndUserService = statusJigAndUserService ?? throw new ArgumentException(nameof(statusJigAndUserService));
            _lastLogMonitorEsdRepository = lastLogMonitorEsdRepository ?? throw new ArgumentException(nameof(lastLogMonitorEsdRepository));
        }

        /// <summary>
        /// Buscar todos operadores paginados
        /// </summary>
        /// <param name="pagina">Número da página (padrão: 1)</param>
        /// <param name="tamanhoPagina">Quantidade de itens por página (padrão: 50)</param>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        public async Task<ActionResult> GetUsersPaginated([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 50)
        {
            var (result, statusCode) = await _biometricService.GetUsersPaginatedAsync(pagina, tamanhoPagina);
            return StatusCode(statusCode, result);
            
        }

        /// <summary>
        /// Buscar todos operadores com imagem
        /// </summary>
        /// <param name="pagina">Número da página (padrão: 1)</param>
        /// <param name="tamanhoPagina">Quantidade de itens por página (padrão: 50)</param>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("ListUsersImages")]
        public async Task<ActionResult> GetAllUsersImage([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 50)
        {
            var (result, statusCode) = await _biometricService.GetAllUsersImageAsync(pagina, tamanhoPagina);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar todos operadores sem imagem
        /// </summary>
        /// <param name="pagina">Número da página (padrão: 1)</param>
        /// <param name="tamanhoPagina">Quantidade de itens por página (padrão: 50)</param>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("ListUsersNoImage")]
        public async Task<ActionResult> GetAllUsersNoImage([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 50)
        {
            var (result, statusCode) = await _biometricService.GetAllUsersNoImageAsync(pagina, tamanhoPagina);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Verificar jig e reconhecimento facial
        /// </summary>
        /// <param name="serialNumber">Número de serie do jig</param>
        /// <param name="imageFile">Imagem do operador</param>
        [HttpPost]
        public async Task<IActionResult> VerifyJigAndFace(IFormFile imageFile, string serialNumber)
        {
            // Valida o arquivo de imagem
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest(new { message = "Arquivo de imagem inválido ou não fornecido." });
            }

            // Chama o serviço de verificação de Jig e reconhecimento facial
            var (personId, jig, user, statusJig, statusUser) = await _biometricService.Verify(imageFile, serialNumber);

            // Valida os resultados da verificação
            if (jig == null)
            {
                return NotFound(new { message = "Jig não encontrado." });
            }

            if (personId == 0)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }


            // Determina o status do Jig
            string jigStatus = statusJig == 0 ? "Jig desconectado" : "Jig conectado";

            // Determina o status do Usuário
            string userStatus = statusUser == 0 ? "Usuário desconectado" : "Usuário conectado";

            // Retorna os dados formatados com o personId, jig e usuário
            return Ok(new
            {
                PersonId = personId,
                Jig = new
                {
                    jig.ID,
                    jig.Name,
                    jig.SerialNumberJig,
                    Status = jigStatus
                },
                User = new
                {
                    user.ID,
                    user.Name,
                    user.Badge,
                    user.Created,
                    user.LastUpdated,
                    Status = userStatus
                }
            });
        }

        /// <summary>
        /// Buscar operador por ID
        /// </summary>
        /// <param name="id">Id do operador no sistema.</param>
        [HttpGet("by-id/{id}")]
        [Authorize(Roles = "administrador,tecnico")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _biometricService.GetUserByIdAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Buscar operador por crachá
        /// </summary>
        /// <param name="cracha">Número do crachá</param>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("by-badge/{badge}")]
        public async Task<IActionResult> GetByBadge(string cracha)
        {
            var response = await _biometricService.GetUserByBadgeAsync(cracha);
            return Ok(response);
        }

        /// <summary>
        /// Criar ou atualizar operador
        /// </summary>
        /// <param name="operador">Dados do operador</param>
        [HttpPost("operador")]
        [Authorize(Roles = "administrador,tecnico")]
        public async Task<IActionResult> InsertBiometric([FromForm] BiometricModel operador)
        {
            var (result, statusCode) = await _biometricService.ManageOperatorAsync(operador);
            return StatusCode(statusCode, result);
        }

        //[HttpGet("embedding")]
        //public async Task<IActionResult> GetEmbedding(string embeding)
        //{
        //    var response = await _biometricService.GetEmbedding(embeding);
        //    return Ok(response);
        //}


        /// <summary>
        /// Excluir operador por ID
        /// </summary>
        /// <param name="id">ID do operador</param>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "administrador,tecnico")]
        public async Task<IActionResult> DeleteBiometric(int id)
        {
            var (result, statusCode) = await _biometricService.DeleteUserByIdAsync(id);
            return StatusCode(statusCode, result);
        }
    }
}
