using BiometricFaceApi.Models;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JigController : ControllerBase
    {
        private readonly JigService _service;

        public JigController(JigService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Buscar todos.
        /// </summary>
        /// <returns>Uma Lista de Jigs.</returns>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("todosJigs")]
        public async Task<ActionResult> GetAllJigs()
        {
            var (result, statusCode) = await _service.GetAllJigsAsync();
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar por Jig por ID.
        /// </summary>
        /// <param name="id">ID Jig.</param>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("buscarJig/{id}")]
        public async Task<ActionResult> GetJigById(int id)
        {
            var (result, statusCode) = await _service.GetJigByIdAsync(id);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar Jig por SN.
        /// </summary>
        /// <param name="sn">SN do Jig.</param>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("jig-bysn/{sn}")]
        public async Task<ActionResult> GetJigBySn(string sn)
        {
            var (result, statusCode) = await _service.GetJigBySnAsync(sn);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        ///Criar ou atualizar operador Jig.
        /// </summary>
        /// <param name="model">O modelo Jig a ser adicionado ou atualizado.</param>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpPost("gerenciarJigs")]
        public async Task<ActionResult> Include([FromBody] JigModel model)
        {
            var (result, statusCode) = await _service.AddOrUpdateJigAsync(model);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Excluir Jig por ID.
        /// </summary>
        /// <param name="id">ID do Jig a ser excluído.</param>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete("deleteJigs/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var (result, statusCode) = await _service.DeleteJigAsync(id);
            return StatusCode(statusCode, result);
        }
    }
}
