using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiometricFaceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;
        public UserController(IUsersRepository usersRepository) {

            this.userService = new UserService(usersRepository);
        }
        [HttpGet]
        public ActionResult<List<UserModel>> SearchAllUsers()
        {

            return Ok(userService.GetUsers());
        }
        [HttpGet]
        [Route("/byid/{id}")]
        public async Task<ActionResult<UserModel>> SearchForId(int id)
        {

            return Ok(await userService.GetUser(id));
        }
    }
}
