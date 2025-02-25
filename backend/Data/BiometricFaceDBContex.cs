using BiometricFaceApi.Models;
using BiometricFaceApi.Security;
using Microsoft.EntityFrameworkCore;


namespace BiometricFaceApi.Data
{
    public class BiometricFaceDBContex : DbContext
    {

        private readonly SecurityService _securityService;
        private readonly IConfiguration _appSettings;
        public BiometricFaceDBContex(DbContextOptions<BiometricFaceDBContex> options, SecurityService securityService, IConfiguration appSettings) : base(options)
        {
            _securityService = securityService;
            _appSettings = appSettings;
        }
    }
}
