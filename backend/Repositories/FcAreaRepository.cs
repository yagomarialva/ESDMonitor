using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Repositories
{
    public class FcAreaRepository : IFcAreaRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;

        public FcAreaRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector ?? throw new ArgumentNullException(nameof(oraConnector));
        }

        // Método para buscar por USERID na tabela fc_area
        public async Task<FcAreaModel?> GetByUserIdAsync(int userId)
        {
            var result = await _oraConnector.LoadData<FcAreaModel, dynamic>(SQLScripts.FcAreaQueries.GetAreaByUserId, new { userId });
            return result.FirstOrDefault();
        }

        // Método para inserir dados na tabela fc_area
        public async Task<FcAreaModel> InsertAsync(FcAreaModel entity)
        {
            try
            {
                await _oraConnector.SaveData<FcAreaModel>(SQLScripts.FcAreaQueries.InsertArea, entity);
                if (_oraConnector.Error != null)
                {
                    throw new Exception($"Erro de banco de dados: {_oraConnector.Error}");
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao tentar adicionar Area.", ex);
            }
        }

        // Método para atualizar dados na tabela fc_area
        public async Task<FcAreaModel?> UpdateAsync(FcAreaModel entity)
        {
            // Verifica se o usuário já possui área cadastrada
            FcAreaModel? nextEntityArea = await GetByUserIdAsync(entity.UserId);
            if (nextEntityArea == null)
            {
                throw new Exception($"Área do usuário para o ID {entity.UserId} não foi encontrada no banco de dados.");
            }

            // Atualiza os campos com os novos valores
            nextEntityArea.FaceConfidence = entity.FaceConfidence;
            nextEntityArea.H = entity.H;
            nextEntityArea.W = entity.W;
            nextEntityArea.X = entity.X;
            nextEntityArea.Y = entity.Y;

            // Salva os dados atualizados na tabela
            await _oraConnector.SaveData<FcAreaModel>(SQLScripts.FcAreaQueries.UpdateArea, nextEntityArea);
            if (_oraConnector.Error != null)
                throw new Exception($"Error:{_oraConnector.Error}");

            // Retorna o objeto atualizado
            return await GetByUserIdAsync(entity.UserId);
        }

        // Método para excluir dados na tabela fc_area
        public async Task<bool> DeleteAsync(FcAreaModel userId)
        {
            var existingAreaUser = await GetByUserIdAsync(userId.UserId);
            if (existingAreaUser == null)
            {
                throw new KeyNotFoundException($"Nenhum Usuário encontrado para ID {userId.UserId}");
            }

            // Deleta os dados da tabela
            await _oraConnector.SaveData<FcAreaModel>(SQLScripts.FcAreaQueries.DeleteArea, userId);
            if (_oraConnector.Error != null)
            {
                throw new Exception($"Erro de banco de dados: {_oraConnector.Error}");
            }

            return true;
        }
    }
}
