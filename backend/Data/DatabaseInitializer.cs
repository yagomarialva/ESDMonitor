using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Security;
using BiometricFaceApi.OraScripts;
using Oracle.ManagedDataAccess.Client;
using BiometricFaceApi.Models;


namespace BiometricFaceApi.Data
{
    public class DatabaseInitializer
    {
        private readonly SecurityService securityService;
        private readonly IRolesRepository roleRepository;
        private readonly IOracleDataAccessRepository oraConnector;
        public DatabaseInitializer(SecurityService securityService, IRolesRepository rolesRepository, IOracleDataAccessRepository oracleConnector)
        {
            this.securityService = securityService;
            this.roleRepository = rolesRepository;
            this.oraConnector = oracleConnector;
        }
        public void CreateAdminUser()
        {
            string adminUsername = "admin";
            string adminPassword = securityService.EncryptAES("admcompal");
            string role = "administrator";

            string connectionString = "User Id=system;Password=oracle;Data Source=localhost:49161/xe";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string insertAdminUserQuery = @"
                INSERT INTO Users (UserId, Username, Password, Role)
                VALUES (seq_users.nextval, :username, :password, :role)";

                using (OracleCommand command = new OracleCommand(insertAdminUserQuery, connection))
                {
                    command.Parameters.Add(new OracleParameter("username", adminUsername));
                    command.Parameters.Add(new OracleParameter("password", adminPassword));
                    command.Parameters.Add(new OracleParameter("role", role));

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
