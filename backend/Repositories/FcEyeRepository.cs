using BiometricFaceApi.Models;
using BiometricFaceApi.OraScripts;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Repositories
{
    public class FcEyeRepository : IFcEyeRepository
    {
        private readonly IOracleDataAccessRepository _oraConnector;

        public FcEyeRepository(IOracleDataAccessRepository oraConnector)
        {
            _oraConnector = oraConnector ?? throw new ArgumentNullException(nameof(oraConnector));
        }

        // Método para buscar os dados de olho por USERID na tabela fc_eye
        public async Task<FcEyeModel?> GetByUserIdAsync(int userId)
        {
            var result = await _oraConnector.LoadData<FcEyeModel, dynamic>(SQLScripts.FcEyeQueries.GetEyeUserId, new { userId });
            return result.FirstOrDefault();
        }

        // Método para inserir dados de olhos na tabela fc_eye
        public async Task<FcEyeModel> InsertAsync(FcEyeModel entity)
        {
            try
            {
                // Verifica se o dado já existe no banco
                var existingEye = await GetByUserIdAsync(entity.UserId);
                if (existingEye != null)
                {
                    throw new Exception($"Os dados de olhos para o usuário com ID {entity.UserId} já existem no banco.");
                }

                // Insere os dados de olhos no banco
                await _oraConnector.SaveData<FcEyeModel>(SQLScripts.FcEyeQueries.InsertEye, entity);
                if (_oraConnector.Error != null)
                {
                    throw new Exception($"Erro de banco de dados: {_oraConnector.Error}");
                }

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao tentar adicionar Eye.", ex);
            }
        }

        // Método para atualizar os dados de olhos na tabela fc_eye
        public async Task<FcEyeModel?> UpdateAsync(FcEyeModel entity)
        {
            FcEyeModel? nextEntityEye = await GetByUserIdAsync(entity.UserId);
            if (nextEntityEye == null)
            {
                throw new Exception($"Os dados de olhos do usuário com ID {entity.UserId} não foram encontrados no banco de dados.");
            }

            // Atualiza os valores de LeftEye e RightEye
            nextEntityEye.LeftEye = entity.LeftEye;
            nextEntityEye.RightEye = entity.RightEye;

            // Atualiza os dados no banco
            await _oraConnector.SaveData<FcEyeModel>(SQLScripts.FcEyeQueries.UpdateEye, nextEntityEye);
            if (_oraConnector.Error != null)
            {
                throw new Exception($"Erro ao atualizar dados: {_oraConnector.Error}");
            }

            // Retorna os dados atualizados
            return await GetByUserIdAsync(entity.UserId);
        }

        // Método para excluir dados de olhos na tabela fc_eye
        public async Task<bool> DeleteEyeAsync(FcEyeModel entity)
        {
            var existingEye = await GetByUserIdAsync(entity.UserId);
            if (existingEye == null)
            {
                throw new KeyNotFoundException($"Nenhum dado de olhos encontrado para o usuário com ID {entity.UserId}");
            }

            // Deleta os dados de olhos no banco
            await _oraConnector.SaveData<FcEyeModel>(SQLScripts.FcEyeQueries.DeleteEye, entity);
            if (_oraConnector.Error != null)
            {
                throw new Exception($"Erro de banco de dados: {_oraConnector.Error}");
            }

            return true;
        }
    }
}
