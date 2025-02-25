using BiometricFaceApi.Models;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordStatusController : ControllerBase
    {
        private readonly RecordStatusService _service;

        public RecordStatusController(RecordStatusService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Busca todos os status.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("todosStatus")]
        public async Task<IActionResult> GetAllStatus()
        {
            var (result, statusCode) = await _service.GetAllStatusAsync();
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar status de produção por ID.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("buscaStatus/{id}")]
        public async Task<IActionResult> GetStatusById(int id)
        {
            var (result, statusCode) = await _service.GetByRecordStatusIdAsync(id);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Criar ou atualizar status de produção.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpPost("adicionarStatus")]
        public async Task<IActionResult> AddOrUpdateStatus([FromBody] RecordStatusProduceModel model)
        {
            var (result, statusCode) = await _service.AddOrUpdateStatusAsync(model);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Deleta status de produção.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete("deleteStatus/{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var (result, statusCode) = await _service.DeleteStatusAsync(id);
            return StatusCode(statusCode, result);
        }
    }
}
