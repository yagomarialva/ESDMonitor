using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;

namespace BiometricFaceApi.Services
{
    public class LineService
    {
        private readonly ILineRepository _repository;

        public LineService(ILineRepository lineRepository)
        {
            _repository = lineRepository;
        }

        public async Task<(object?, int)> GetAllLinesAsync()
        {
            object? content;
            int statusCode;

            try
            {
                List<LineModel> lines = await _repository.GetAllAsync();

                if (!lines.Any())
                {
                    content = "Nenhuma linha cadastrada.";
                    statusCode = StatusCodes.Status404NotFound;
                }
                else
                {
                    content = lines;
                    statusCode = StatusCodes.Status200OK;
                }

                return (content, statusCode);
            }
            catch (Exception ex)
            {
                content = $"Erro ao obter linhas: {ex.Message}";
                statusCode = StatusCodes.Status400BadRequest;
                return (content, statusCode);
            }
        }

        public async Task<(object?, int)> GetLineByIdAsync(int id)
        {
            object? content;
            int statusCode;

            try
            {
                var line = await _repository.GetByIdAsync(id);

                if (line == null)
                {
                    content = $"Linha com ID {id} não encontrada.";
                    statusCode = StatusCodes.Status404NotFound;
                }
                else
                {
                    content = line;
                    statusCode = StatusCodes.Status200OK;
                }

                return (content, statusCode);
            }
            catch (Exception ex)
            {
                content = $"Erro ao obter linha: {ex.Message}";
                statusCode = StatusCodes.Status400BadRequest;
                return (content, statusCode);
            }
        }

        public async Task<(object?, int)> GetLineByNameAsync(string lineName)
        {
            object? content;
            int statusCode;

            try
            {
                var line = await _repository.GetByNameAsync(lineName);

                if (line == null)
                {
                    content = $"Linha com nome '{lineName}' não encontrada.";
                    statusCode = StatusCodes.Status404NotFound;
                }
                else
                {
                    content = line;
                    statusCode = StatusCodes.Status200OK;
                }

                return (content, statusCode);
            }
            catch (Exception ex)
            {
                content = $"Erro ao obter linha: {ex.Message}";
                statusCode = StatusCodes.Status400BadRequest;
                return (content, statusCode);
            }
        }

        public async Task<(object?, int)> AddOrUpdateLineAsync(LineModel lineModel)
        {
            object? content;
            int statusCode;

            try
            {
                LineModel? existingLine = await _repository.GetByIdAsync(lineModel.ID);

                if (existingLine != null)
                    lineModel.ID = existingLine.ID;

                content = await _repository.AddOrUpdateAsync(lineModel);

                if (lineModel.ID > 0)
                {
                    statusCode = StatusCodes.Status200OK;
                }
                else
                {
                    statusCode = StatusCodes.Status201Created;
                }

                return (content, statusCode);
            }
            catch (Exception ex)
            {
                content = $"Erro ao adicionar ou atualizar linha: {ex.Message}";
                statusCode = StatusCodes.Status400BadRequest;
                return (content, statusCode);
            }
        }

        public async Task<(object?, int)> DeleteLineAsync(int id)
        {
            object? content;
            int statusCode;

            try
            {
                var line = await _repository.GetByIdAsync(id);

                if (line == null)
                {
                    content = "Linha não encontrada.";
                    statusCode = StatusCodes.Status404NotFound;
                }
                else
                {
                    await _repository.DeleteAsync(line.ID);
                    content = new { line.ID, line.Name };
                    statusCode = StatusCodes.Status200OK;
                }

                return (content, statusCode);
            }
            catch (Exception ex)
            {
                content = $"Erro ao deletar linha: {ex.Message}";
                statusCode = StatusCodes.Status400BadRequest;
                return (content, statusCode);
            }
        }
    }
}
