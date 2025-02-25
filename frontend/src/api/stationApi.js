import axios from 'axios';
import TokenApi from "./TokenApi";

const API_BASE_URL = process.env.REACT_APP_API_URL_FCT;
const url = 'api/Station'
// Função auxiliar para manipulação de respostas
const handleResponse = async (request) => {
    try {
        const response = await request();
        return response.data;
    } catch (error) {
        console.error("API call failed: ", error);
        throw error;
    }
};

export const getAllStations = async () => {
    return handleResponse(() => TokenApi.get(`${url}/todosEstacoes`));
};

export const getStation = async (id) => {
    return handleResponse(() => TokenApi.get(`/Buscarestacao/${id}`));
};

export const getStationByName = async (name) => {
    try {
      // Chama a API e retorna o resultado
      const response = await TokenApi.get(`${url}/BuscarNomeEstacao/${name}`);
      console.log('response', response)
      return handleResponse(() => response);
    } catch (error) {
      // Exibe o erro no console para ajudar no debug
      console.error(`Erro ao buscar a estação com nome ${name}:`, error);
  
      // Retorna uma mensagem de erro personalizada ou lança o erro para ser tratado em outro lugar
      throw new Error(`Falha ao buscar a estação com o nome ${name}. Por favor, tente novamente mais tarde.`);
    }
  };
  
export const createStation = async (station) => {
    return handleResponse(() => TokenApi.post(`${url}/adicionarEstacao`, station));
};

export const updateStation = async (station) => {
    return handleResponse(() => TokenApi.put(`${url}/adicionarEstacao`, station));
};

export const deleteStation = async (id) => {
    return handleResponse(() => TokenApi.delete(`${url}/deleteEstação?id=${id}`));
};
