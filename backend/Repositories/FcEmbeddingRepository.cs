using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Repositories
{
    public class FcEmbeddingRepository : IFcEmbeddingRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;

        public FcEmbeddingRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector ?? throw new ArgumentNullException(nameof(oraConnector));
        }

        // Método para buscar Embedding por USERID na tabela fc_embedding
        public async Task<FcEmbeddingModel?> GetByUserIdAsync(int userId)
        {
            var result = await _oraConnector.LoadData<FcEmbeddingModel, dynamic>(SQLScripts.FcEmbeddingQueries.GetEmbeddingByUserId, new { userId });
            return result.FirstOrDefault();
        }

        // Método para inserir dados na tabela fc_embedding
        public async Task<FcEmbeddingModel> InsertAsync(FcEmbeddingModel entity)
        {
            try
            {
                await _oraConnector.SaveData<FcEmbeddingModel>(SQLScripts.FcEmbeddingQueries.InsertEmbedding, entity);
                if (_oraConnector.Error != null)
                {
                    throw new Exception($"Erro de banco de dados: {_oraConnector.Error}");
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao tentar adicionar Embedding.", ex);
            }
        }

        // Método para atualizar dados na tabela fc_embedding
        public async Task<FcEmbeddingModel?> UpdateAsync(FcEmbeddingModel entity)
        {
            // Verifica se o embedding do usuário já existe
            FcEmbeddingModel? nextEntityEmbedding = await GetByUserIdAsync(entity.UserId);
            if (nextEntityEmbedding == null)
            {
                throw new Exception($"Embedding do usuário para o ID {entity.UserId} não foi encontrado no banco de dados.");
            }

            // Atualiza o campo de EmbeddingValue
            nextEntityEmbedding.EmbeddingValue = entity.EmbeddingValue;

            // Salva os dados atualizados na tabela
            await _oraConnector.SaveData<FcEmbeddingModel>(SQLScripts.FcEmbeddingQueries.UpdateEmbedding, nextEntityEmbedding);
            if (_oraConnector.Error != null)
                throw new Exception($"Error:{_oraConnector.Error}");

            // Retorna o objeto atualizado
            return await GetByUserIdAsync(entity.UserId);
        }

        // Método para excluir dados da tabela fc_embedding
        public async Task<bool> DeleteEmbeddingAsync(FcEmbeddingModel entity)
        {
            var existingUser = await GetByUserIdAsync(entity.UserId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"Nenhum Usuário encontrado para ID {entity.UserId}");
            }

            // Deleta os dados da tabela
            await _oraConnector.SaveData<FcEmbeddingModel>(SQLScripts.FcEmbeddingQueries.DeleteEmbedding, entity);
            if (_oraConnector.Error != null)
            {
                throw new Exception($"Erro de banco de dados: {_oraConnector.Error}");
            }

            return true;
        }
    }
}
