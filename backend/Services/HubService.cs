using BiometricFaceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace BiometricFaceApi.Services
{
    public class HubService : IDisposable
    {
        private HubConnection? _hubConnection;
        private readonly IOracleDataAccessRepository _oracleRepository;
        private readonly TimeSpan _reconnectDelay = TimeSpan.FromSeconds(1);

        public HubService(IOracleDataAccessRepository oracleRepository)
        {
            _oracleRepository = oracleRepository;
        }

        /// <summary>
        /// Envia uma mensagem para o hub se estiver conectado.
        /// </summary>
        /// <param name="message">Mensagem a ser enviada.</param>
        public async Task SendMessageAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("A mensagem não pode ser nula ou vazia.", nameof(message));

            if (_hubConnection is { State: HubConnectionState.Connected })
            {
                await _hubConnection.InvokeAsync("ReceiveLogFromMonitor", message);
            }
            else
            {
                throw new InvalidOperationException("A conexão do hub não foi estabelecida.");
            }
        }

        /// <summary>
        /// Inicia a escuta do websocket e conecta ao hub.
        /// </summary>
        /// <param name="url">URL do hub.</param>
        public async Task StartListeningAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("O URL não pode ser nulo ou vazio.", nameof(url));

            await Task.Delay(_reconnectDelay);

            _hubConnection = new HubConnectionBuilder()
                // tratamento para ignorar possieis alertas de errors.
                .WithUrl(url, options =>
                {
                    options.HttpMessageHandlerFactory = message =>
                    {
                        if (message is HttpClientHandler clientHandler)
                        {
                            clientHandler.ServerCertificateCustomValidationCallback +=
                                (sender, certificate, chain, sslPolicyErrors) => true;
                        }
                        return message;
                    };
                })
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string>("ReceiveLog", async message =>
            {
                Console.WriteLine(message); // Pode ser substituído por um método de log adequado
            });

            await _hubConnection.StartAsync();
        }

        /// <summary>
        /// Desconecta do hub e libera os recursos.
        /// </summary>
        public async Task StopListeningAsync()
        {
            if (_hubConnection is { State: HubConnectionState.Connected })
            {
                await _hubConnection.StopAsync();
            }
        }
        public async void Dispose()
        {
            await StopListeningAsync();
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
