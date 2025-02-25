using BiometricFaceApi.Repositories.Interfaces;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace BiometricFaceApi.Repositories
{
    public class OracleDataAccessRepository : IOracleDataAccessRepository
    {

        public string? Error => error;
        private string? error;
        public string? ConnectionString { get; set; }
        public OracleDataAccessRepository(string? connectionstring = null)
        {
            ConnectionString = connectionstring;

        }
        public async Task<List<T>> LoadData<T, U>(string SqlCommand, U parameters, string? connectionString = null, int? timeout = null)
        {
            using (IDbConnection connection = new OracleConnection(connectionString ?? this.ConnectionString))
            {
                try
                {
                    error = null;
                    var RetreivedList = await connection.QueryAsync<T>(SqlCommand, parameters, null, commandTimeout: timeout ?? 5000, null);
                    
                    return  RetreivedList.AsList();
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    return new List<T>();
                }
            }
        }
        public async Task SaveData<T>(string SqlCommand, T parameters, string? connectionString = null)
        {
            try
            {
                error = null;
                using (IDbConnection connection = new OracleConnection(connectionString ?? this.ConnectionString))
                {
                    await connection.ExecuteAsync(SqlCommand, parameters);
                    
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

        }
        public async Task SaveDataAdmin<T1, T2>(string insertAdminUserQuery, T2 value, string? ConnectionString = null)
        {
            try
            {
                error = null;
                using (IDbConnection connection = new OracleConnection(this.ConnectionString))
                {
                    connection.Open();
                    await connection.ExecuteAsync(insertAdminUserQuery, value);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }
    }
}
