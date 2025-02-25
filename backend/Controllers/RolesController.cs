using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RolesService _service;

        public RolesController(IRolesRepository repository)
        {
            _service = new RolesService(repository);
        }

        /// <summary>
        /// Recupera todas as funções.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var (result, statusCode) = await _service.GetAllRolesAsync();
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Recupera função por ID.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var (result, statusCode) = await _service.GetRoleByIdAsync(id);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Adiciona ou atualiza uma função.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpPost]
        public async Task<IActionResult> UpsertRole([FromBody] RolesModel model)
        {
            var (response, statusCode) = await _service.IncludeRoleAsync(model);
            return StatusCode(statusCode, response);
        }

        /// <summary>
        /// Exclui função por ID.
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var (result, statusCode) = await _service.DeleteRoleAsync(id);
            return StatusCode(statusCode, result);
        }
    }
}
