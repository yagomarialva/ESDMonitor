import * as signalR from "@microsoft/signalr";

const API_URL = process.env.REACT_APP_HOST;

interface LogData {
  serialNumber: string;
  status: number;
  // Adicione outros campos conforme necessário
}

class SignalRService {
  private connection: signalR.HubConnection;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(`http://${API_URL}:7080/loghub`) // Substitua pela URL do seu hub
      .withAutomaticReconnect() // Reconexão automática
      .configureLogging(signalR.LogLevel.Information) // Nível de log configurado para Information
      .build();
  }

  // Inicia a conexão com o SignalR
  public async startConnection(): Promise<void> {
    try {
      await this.connection.start();
      console.log("Conectado ao SignalR no factoryMap");
    } catch (error) {
      // console.error("Erro ao conectar ao SignalR:", error);
      // Tenta reconectar após 5 segundos
      setTimeout(() => this.startConnection(), 5000);
    }
  }

  // Registra o callback para o evento ReceiveAlert
  onReceiveAlert(callback: (message: any) => void): void {
    this.connection.on("ReceiveAlert", (message: any) => {
      // console.log("Alerta recebido:", message);
      callback(message);
    });
    console.log("Callback registrado para ReceiveAlert");
  }

  // Registra o callback para o evento ReceiveLog
  onReceiveLog(callback: (log: any) => void): void {
    this.connection.on("ReceiveLog", (log: any) => {
      // console.log("Log recebido:", log);
      callback(log);
    });
    console.log("Callback registrado para ReceiveLog");
  }

  // Para a conexão com o SignalR
  async stopConnection(): Promise<void> {
    try {
      await this.connection.stop();
    } catch (error) {
      // console.error("Erro ao parar a conexão:", error);
    }
  }
  public getConnectionState(): string {
    return this.connection.state;
  }
}



export default new SignalRService();