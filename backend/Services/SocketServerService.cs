using BiometricFaceApi.Repositories.Interfaces;
using System.Net.Sockets;
using System.Text;

namespace BiometricFaceApi.Services
{
    public class SocketService : IDisposable
    {
        private TcpClient? _tcpClient;
        private readonly IOracleDataAccessRepository _oracleRepository;
        private NetworkStream? _networkStream;
        private readonly TimeSpan _reconnectDelay = TimeSpan.FromSeconds(1);

        public SocketService(IOracleDataAccessRepository oracleRepository)
        {
            _oracleRepository = oracleRepository;
        }

        /// <summary>
        /// Envia uma mensagem para o servidor socket se estiver conectado.
        /// </summary>
        /// <param name="message">Mensagem a ser enviada.</param>
        public async Task SendMessageAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("A mensagem não pode ser nula ou vazia.", nameof(message));

            if (_networkStream != null && _tcpClient != null && _tcpClient.Connected)
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                await _networkStream.WriteAsync(data, 0, data.Length);
            }
            else
            {
                throw new InvalidOperationException("A conexão do socket não foi estabelecida.");
            }
        }

        /// <summary>
        /// Inicia a escuta do socket e conecta ao servidor.
        /// </summary>
        /// <param name="host">Endereço do servidor.</param>
        /// <param name="port">Porta do servidor.</param>
        public async Task StartListeningAsync(string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host))
                throw new ArgumentException("O host não pode ser nulo ou vazio.", nameof(host));

            await Task.Delay(_reconnectDelay);

            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(host, port);
            _networkStream = _tcpClient.GetStream();

            // Iniciar escuta de mensagens recebidas
            _ = Task.Run(async () =>
            {
                byte[] buffer = new byte[1024];
                int bytesRead;
                while ((bytesRead = await _networkStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(receivedMessage); // Pode ser substituído por um método de log adequado
                }
            });
        }

        /// <summary>
        /// Desconecta do servidor socket e libera os recursos.
        /// </summary>
        public void StopListening()
        {
            if (_tcpClient != null && _tcpClient.Connected)
            {
                _networkStream?.Close();
                _tcpClient.Close();
            }
        }

        public void Dispose()
        {
            StopListening();
            _networkStream?.Dispose();
            _tcpClient?.Dispose();
        }
    }
}
