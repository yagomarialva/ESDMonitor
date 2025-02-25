using BiometricFaceApi.Data;
using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using BiometricFaceApi.Security;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace BiometricFaceApi.Repositories
{
    public class DbInitializerRepository : IDbInitializerRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;
        private readonly SecurityService _securityService;
        private readonly IConfiguration _configuration;
        private readonly BiometricFaceDBContex _dbContex;

        public DbInitializerRepository(
            IOracleDataAccessRepository oraConnector,
            SecurityService securityService,
            BiometricFaceDBContex biometricFaceDBContex,
            IConfiguration configuration)
        {
            _oraConnector = oraConnector;
            _securityService = securityService;
            _dbContex = biometricFaceDBContex;
            _configuration = configuration;
        }

        public async Task InitializeAsync()
        {
            try
            {
                var tables = _configuration.GetSection("SqlStructDatabase").Get<IEnumerable<SqlStructModel>>();

                if (tables != null)
                {
                    await CreateDatabaseObjectsAsync(tables);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao inicializar o banco de dados.", ex);
            }
        }

        private async Task CreateDatabaseObjectsAsync(IEnumerable<SqlStructModel> tables)
        {
            using var command = (OracleCommand)_dbContex.Database.GetDbConnection().CreateCommand();
            await command.Connection.OpenAsync();

            foreach (var table in tables)
            {
                if (await TableExistsAsync(command, table.TableName))
                {
                    continue;
                }

                await ExecuteCommandAsync(command, table.CommandToCreateTable);
                await ExecuteCommandAsync(command, table.CommandToSequence);
                await ExecuteCommandAsync(command, table.CommandToTrigger);
                await ExecuteCommandAsync(command, table.CommandToTrigger1);
                await ExecuteCommandAsync(command, table.CommandToPopulete);
            }

            await command.Connection.CloseAsync();
        }

        private async Task<bool> TableExistsAsync(OracleCommand command, string? tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                return false;
            }

            // Ajuste para verificar o schema correto
            command.CommandText = $@"SELECT COUNT(1) FROM all_tables WHERE owner = 'FCT_AUTO_TEST' AND table_name = '{tableName.ToUpper()}'";
            command.CommandType = CommandType.Text;

            //command.CommandText = $@"SELECT COUNT(1) FROM all_tables WHERE table_name = '{tableName.ToUpper()}'";
            //command.CommandType = CommandType.Text;

            // Mantendo a implementação original
            var result = command.ExecuteScalar();
            return result?.ToString().Trim() != "0";
        }

       
        private async Task ExecuteCommandAsync(OracleCommand command, string? sqlCommand)
        {
            if (!string.IsNullOrEmpty(sqlCommand))
            {
                command.CommandText = sqlCommand;
                await Task.Run(() => command.ExecuteNonQuery());
            }
        }
    }
}
