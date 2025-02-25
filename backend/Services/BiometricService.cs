using BiometricFaceApi.Models;
using BiometricFaceApi.Repositories.Interfaces;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using JsonException = Newtonsoft.Json.JsonException;

namespace BiometricFaceApi.Services
{
    public class BiometricService
    {
        private readonly UserService _userService;
        private readonly ImageService _imageService;
        private readonly JigService _jigService;
        private readonly StatusJigAndUserService _statusJigAndUserService;
        private readonly ILastLogMonitorEsdRepository _lastLogMonitorEsdRepository;
        public BiometricService(UserService userService, ImageService imageService, JigService jigService, StatusJigAndUserService statusJigAndUserService, ILastLogMonitorEsdRepository lastLogMonitorEsdRepository)
        {
            _userService = userService;
            _imageService = imageService;
            _jigService = jigService;
            _statusJigAndUserService = statusJigAndUserService;
            _lastLogMonitorEsdRepository = lastLogMonitorEsdRepository;
        }
        // Gerenciar operadores (atualizar ou inserir)
        public async Task<(object?, int)> ManageOperatorAsync(BiometricModel biometric)
        {
            try
            {
                var user = new UserModel { ID = biometric.ID, Badge = biometric.Badge, Name = biometric.Name };
                var image = new ImageModel { ImageFile = biometric.Image };

                var existingUser = await _userService.GetUserByBadgeAsync(user.Badge);

                if (existingUser != null)
                {
                    return await UpdateExistingOperatorAsync(existingUser, user, image, biometric);
                }
                else
                {
                    return await InsertNewOperatorAsync(user, image, biometric);
                }
            }
            catch (Exception ex)
            {
                // Lidar com exceções, logar se necessário
                return (ex.Message, StatusCodes.Status400BadRequest);
            }
        }
        private async Task<(object?, int)> UpdateExistingOperatorAsync(UserModel existingUser, UserModel user, ImageModel image, BiometricModel biometric)
        {
            // Verifica se a imagem e seu stream estão preenchidos corretamente
            if (image == null || image.PictureStream == null)
            {
                return ("Imagem inválida ou não carregada", StatusCodes.Status400BadRequest);
            }

            var url = "http://10.58.72.188:5000/embedding/";

            image.Embedding = (await GetApiResponseAsync(url, image))?.Replace("  ", "").Replace(" ", "");

            if (string.IsNullOrEmpty(image.Embedding))
            {
                return ("Falha ao obter embedding da imagem", StatusCodes.Status400BadRequest);
            }

            image.UserId = existingUser.ID;

            var existingImage = await _imageService.GetImageByUserIdAsync(existingUser.ID);

            // Atualizar usuário e imagem existente
            await _userService.UpdateUserAsync(user, existingUser.ID);
            if (existingImage == null)
            {
                await _imageService.AddImageAsync(image);
            }
            else if (image.PictureStream != null)
            {
                await _imageService.UpdateImageAsync(image);
            }

            var updatedBiometric = new BiometricModel
            {
                ID = existingUser.ID,
                Badge = biometric.Badge,
                Name = biometric.Name,
                Stream = image.PictureStream,
                Embedding = image.Embedding
            };

            return (updatedBiometric, StatusCodes.Status200OK);
        }
        private async Task<(object?, int)> InsertNewOperatorAsync(UserModel user, ImageModel image, BiometricModel biometric)
        {
            var url = "http://10.58.72.188:5000/embedding/";

            if (image != null && image.PictureStream != null)
            {
                // Obtém o embedding da imagem
                image.Embedding = (await GetApiResponseAsync(url, image))?.Replace("  ", "").Replace(" ", "");

                // Verifica se o embedding foi gerado corretamente
                if (string.IsNullOrEmpty(image.Embedding))
                {
                    return ("Falha ao obter embedding da imagem", StatusCodes.Status400BadRequest);
                }
            }
            var newUser = await _userService.AddUserAsync(user);

            if (newUser != null)
            {
                image.UserId = newUser.ID;
                await _imageService.AddImageAsync(image);

                var newBiometric = new BiometricModel
                {
                    ID = newUser.ID,
                    Badge = biometric.Badge,
                    Name = biometric.Name,
                    Stream = image.PictureStream,
                    Embedding = image.Embedding
                };

                return (newBiometric, StatusCodes.Status201Created);
            }
            else
            {
                return ("Dados incorretos ou inválidos.", StatusCodes.Status404NotFound);
            }
        }
        // Operador de pesquisa por ID
        public async Task<BiometricModel> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado");

            var image = await _imageService.GetImageByUserIdAsync(user.ID);
            return new BiometricModel
            {
                ID = user.ID,
                Name = user.Name,
                Badge = user.Badge,
                Stream = image?.PictureStream
            };
        }
        //Pesquisa por Caracteristica
        //public async Task<BiometricModel> GetEmbedding(string embedding)
        //{
        //    if (string.IsNullOrEmpty(embedding))
        //    {
        //        throw new ArgumentNullException(nameof(embedding), "O parâmetro 'embedding' não pode ser nulo ou vazio.");
        //    }
        //    var image = await _imageService.GetEmbeddingAsync(embedding);
        //    if (image == null)
        //        throw new KeyNotFoundException("Imagem não encontrado");

        //    return new BiometricModel
        //    {

        //        Stream = image?.PictureStream,
        //        Embedding = image?.Embedding
        //    };
        //}
        public async Task<(object?, int)> GetUsersPaginatedAsync(int page, int pageSize)
        {
            var users = await _userService.GetListUsersAsync(page, pageSize); // Obtenha os usuários paginados
            var userBiometrics = new List<BiometricModel>();

            foreach (var user in users)
            {
                var image = await _imageService.GetImageByUserIdAsync(user.ID);
                var biometricModel = new BiometricModel
                {
                    ID = user.ID,
                    Name = user.Name,
                    Badge = user.Badge,
                    Stream = image?.PictureStream
                };

                userBiometrics.Add(biometricModel);
            }

            var totalRecords = await _userService.GetTotalUsers(); // Total de registros para a paginação
            return (new
            {
                Users = userBiometrics,
                TotalRecords = totalRecords,
                Page = page,
                PageSize = pageSize
            }
            , StatusCodes.Status200OK);
        }
        // Buscar todos os operadores com imagem
        public async Task<(object?, int)> GetAllUsersImageAsync(int page, int pageSize)
        {
            try
            {
                var users = await _userService.GetListUsersAsync(page, pageSize);
                var images = await _imageService.GetListImagesAsync(page, pageSize); // Paginação separada para ambos

                var biometrics = users.Select(user =>
                {
                    var image = images.Find(img => img.UserId == user.ID);
                    return new BiometricModel
                    {
                        ID = user.ID,
                        Badge = user.Badge,
                        Name = user.Name,
                        Stream = image?.PictureStream
                    };
                }).ToList();

                if (!biometrics.Any())
                {
                    return ("Nenhum usuário cadastrado!", StatusCodes.Status404NotFound);
                }

                var totalRecords = await _userService.GetTotalUsers(); // Total de registros para a paginação
                return (new
                {
                    Users = biometrics,
                    TotalRecords = totalRecords,
                    Page = page,
                    PageSize = pageSize
                }
                , StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ($"Erro ao buscar dados: {ex.Message}", StatusCodes.Status400BadRequest);
            }
        }
        public async Task<(object?, int)> GetAllUsersNoImageAsync(int page, int pageSize)
        {
            try
            {
                var users = await _userService.GetListUsersAsync(page, pageSize);

                var biometrics = users.Select(user => new BiometricModel
                {
                    ID = user.ID,
                    Badge = user.Badge,
                    Name = user.Name
                }).ToList();

                if (!biometrics.Any())
                {
                    return ("Nenhum usuário cadastrado!", StatusCodes.Status404NotFound);
                }

                var totalRecords = await _userService.GetTotalUsers(); // Total de registros para a paginação
                return (new
                {
                    Users = biometrics,
                    TotalRecords = totalRecords,
                    Page = page,
                    PageSize = pageSize
                }
                , StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ($"Erro ao buscar dados: {ex.Message}", StatusCodes.Status400BadRequest);
            }
        }
        // Buscar operador pela matrícula
        public async Task<BiometricModel> GetUserByBadgeAsync(string badge)
        {
            var user = await _userService.GetUserByBadgeAsync(badge);
            if (user == null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            var image = await _imageService.GetImageByUserIdAsync(user.ID);
            return new BiometricModel
            {
                ID = user.ID,
                Name = user.Name,
                Badge = user.Badge,
                Stream = image?.PictureStream
            };
        }
        // Deletar operador por ID
        public async Task<(object?, int)> DeleteUserByIdAsync(int userId)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                    return ("Usuário não encontrado.", StatusCodes.Status404NotFound);

                var image = await _imageService.GetImageByUserIdAsync(userId);
                if (image != null)
                    await _imageService.DeleteImageAsync(image.ID);

                await _userService.DeleteUserAsync(user.ID);

                return (new { ID = user.ID, Name = user.Name, Badge = user.Badge }, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ($"Erro ao excluir o usuário: {ex.Message}", StatusCodes.Status400BadRequest);
            }
        }
        private async Task<string?> GetApiResponseAsync(string url, ImageModel image)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        var imageContent = new ByteArrayContent(image.PictureStream);
                        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                        content.Add(imageContent, "file", "imag.jpeg");
                        var response = await client.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var contentResponse = await response.Content.ReadAsStringAsync();
                            var jobject = JObject.Parse(contentResponse);
                            var data = jobject["data"]?.ToString();


                            // Retornar o conteúdo da resposta como string
                            return data;
                        }
                        else
                        {
                            return null;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao chamar a API: {ex.Message}");
                return null;
            }
        }
        private async Task<string?> GetApiResponseFaceRecognitionAsync(string url, ImageModel image)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        var imageContent = new ByteArrayContent(image.PictureStream);
                        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                        content.Add(imageContent, "file", "imag.jpeg");
                        var response = await client.PostAsync(url, content);

                        if (response.IsSuccessStatusCode)
                        {
                            var contentResponse = await response.Content.ReadAsStringAsync();
                            return contentResponse;
                        }
                        else
                        {
                            return null;
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao chamar a API: {ex.Message}");
                return null;
            }
        }
        public async Task<(int personId, JigModel? jig, UserModel? user, int? statusJig, int? statusUser)> Verify(IFormFile imageFile, string serial)
        {
            // Valida se o número de série é válido
            var jig = await _jigService.GetSerialNumberAsync(serial);
            if (jig == null || string.IsNullOrEmpty(jig.SerialNumberJig))
            {
                return (0, null, null, 0, 0);
            }

            var lasLogMonitor = await _lastLogMonitorEsdRepository.GetJigByIdLastLogsAsync(jig.ID);
            if (lasLogMonitor == null)
            {
                return (0, null, null, 0, 0);
            }

            // Converte a imagem para byte array
            byte[] imageBytes;
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            var image = new ImageModel { PictureStream = imageBytes };

            // URL da API de reconhecimento facial
            var url = "http://10.58.72.188:5000/verify/";

            // Chama a API de reconhecimento facial
            var responseData = await GetApiResponseFaceRecognitionAsync(url, image);

            if (string.IsNullOrEmpty(responseData))
            {
                Console.WriteLine("Erro ao obter resposta da API.");
                return (0, null, null, 0, 0);
            }

            // Processa a resposta da API para extrair o ID da pessoa e a foto
            var personId = ExtractPersonId(responseData); // Obtém o personId da resposta da API
            if (personId == 0)
            {
                return (0, null, null, 0, 0); // Retorna null se o reconhecimento falhou
            }

            // Busca o usuário pelo personId
            var user = await _userService.GetUserByIdAsync(personId);
            if (user == null)
            {
                return (0, null, null, 0, 0);
            }
            var monitor = 0;
            var status = await _statusJigAndUserService.GetCurrentStatusAsync(monitor, personId, jig.ID);
            var statusJig = status?.MessageType == "jig" ? status.Status : 0;
            var statusUser = status?.MessageType == "operador" ? status.Status : 0;

            // Retorna o resultado com o personId, foto, jig e o usuário
            return (personId, jig, user, statusJig, statusUser);
        }
        private int ExtractPersonId(string responseData)
        {
            try
            {
                using (JsonDocument jsonDocument = JsonDocument.Parse(responseData))
                {
                    var root = jsonDocument.RootElement;

                    // Verifica se a propriedade "userId" existe
                    if (root.TryGetProperty("userId", out JsonElement userIdElement))
                    {
                        // Se o "userId" for numérico, retorna seu valor
                        if (userIdElement.ValueKind == JsonValueKind.Number)
                        {
                            return userIdElement.GetInt32();
                        }
                        // Caso o "userId" seja uma string, tenta convertê-la para int
                        else if (userIdElement.ValueKind == JsonValueKind.String &&
                                 int.TryParse(userIdElement.GetString(), out int personId))
                        {
                            return personId;
                        }
                    }

                    Console.WriteLine("Propriedade 'userId' ausente ou inválida.");
                    return 0; // Retorna 0 se não encontrar o userId ou se for inválido
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao processar JSON: {ex.Message}");
                return 0;
            }
        }
        private object ExtractPhoto(string responseData)
        {
            try
            {

                JObject converteStringParaObjtJson = JObject.Parse(responseData);

                string acessaValorDaPropriedadePhotoDentroDeData = (string)converteStringParaObjtJson["data"]["photo"];

                byte[] bytesDaFotoBase64 = Convert.FromBase64String(acessaValorDaPropriedadePhotoDentroDeData);

                return bytesDaFotoBase64;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Erro ao processar JSON: {ex.Message}");
                return null;
            }
        }
    }
}
