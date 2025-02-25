using BiometricFaceApi.Models;

namespace BiometricFaceApi.Repositories.Interfaces
{
    public interface IOracleDataAccessRepository
    {
        string Error { get; }

        // Método para carregar dados do banco de dados
        Task<List<T>> LoadData<T, U>(string SqlCommand, U parameters, string? ConnectionString = null, int? timeout = null);

        // Método para salvar dados no banco de dados
        Task SaveData<T>(string SqlCommand, T parameters, string? ConnectionString = null);
        Task SaveDataAdmin<T1, T2>(string insertAdminUserQuery, T2 value, string? ConnectionString = null);
    }
}
