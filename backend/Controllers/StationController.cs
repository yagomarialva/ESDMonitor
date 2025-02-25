using BiometricFaceApi.Models;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly StationService _service;

        // Injetando o StationService diretamente
        public StationController(StationService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>
        /// Buscar todos
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("todosEstacoes")]
        public async Task<IActionResult> GetAllStations()
        {
            var (result, statusCode) = await _service.GetAllStationsAsync();
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar id 
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("BuscarEstacao/{id}")]
        public async Task<IActionResult> GetStationById(int id)
        {
            var (result, statusCode) = await _service.GetStationByIdAsync(id);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Buscar nome 
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpGet]
        [Route("BuscarNomeEstacao/{name}")]
        public async Task<IActionResult> GetStationByName(string name)
        {
            var (result, statusCode) = await _service.GetStationByNameAsync(name);
            return StatusCode(statusCode, result);
        }
        /// <summary>
        /// Criar ou atualizar Estação.
        /// </summary>
        [Authorize(Roles = "administrador,tecnico")]
        [HttpPost]
        [Route("adicionarEstacao")]
        public async Task<IActionResult> AddOrUpdateStation([FromBody] StationModel model)
        {
            var (result, statusCode) = await _service.AddOrUpdateStationAsync(model);
            return StatusCode(statusCode, result);
        }

        /// <summary>
        /// Deletar Estação
        /// </summary>
       [Authorize(Roles = "administrador,tecnico")]
        [HttpDelete]
        [Route("deleteEstação")]
        public async Task<IActionResult> DeleteStation(int id)
        {
            var (result, statusCode) = await _service.DeleteStationAsync(id);
            return StatusCode(statusCode, result);
        }
    }
}
