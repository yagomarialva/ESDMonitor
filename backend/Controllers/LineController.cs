using BiometricFaceApi.Models;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineController : ControllerBase
    {
        private readonly LineService _service;
        public LineController(LineService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Busca todas as linhas
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("TodasLinhas")]
        public async Task<ActionResult> BuscarTodaLinha()
        {
            var (result, statusCode) = await _service.GetAllLinesAsync();
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar linha por ID
        /// </summary>
        /// <param name="id">ID da linha</param>
        [HttpGet("BuscarLinha/{id}")]
        public async Task<ActionResult> BuscarLinha(int id)
        {
            var (result, statusCode) = await _service.GetLineByIdAsync(id);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar linha por nome
        /// </summary>
        /// <param name="name">Nome da linha</param>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("BuscarNome/{name}")]
        public async Task<ActionResult> BuscarNome(string name)
        {
            var (result, statusCode) = await _service.GetLineByNameAsync(name);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Criar ou atualizar uma linha
        /// </summary>
        /// <param name="linha">Informações da linha</param>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpPost("adicionarLinha")]
        public async Task<ActionResult> Include([FromBody] LineModel linha)
        {
            var (result, statusCode) = await _service.AddOrUpdateLineAsync(linha);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Deletar linha
        /// </summary>
        /// <param name="id">ID da linha a ser deletada</param>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete("DeleteLinha/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var (result, statusCode) = await _service.DeleteLineAsync(id);
            return StatusCode(statusCode, result);
        }
    }
}
