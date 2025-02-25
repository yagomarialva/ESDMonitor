import axios from 'axios';
import TokenApi from "./TokenApi";

const API_BASE_URL = process.env.REACT_APP_API_URL_FCT;
const url = 'api/Jig'

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

export const getAllJigs = async () => {
    return handleResponse(() => TokenApi.get(`${url}/todosJigs`));
};

export const getStation = async (id) => {
    return handleResponse(() => TokenApi.get(`${API_BASE_URL}buscarJig/${id}`));
};

export const createJigs = async (station) => {
    return handleResponse(() => TokenApi.post(`${url}/gerenciarJigs`, station));
};

export const updateJigs = async (station) => {
    return handleResponse(() => TokenApi.post(`${url}/gerenciarJigs`, station));
};

export const deleteJigs = async (id) => {
    return handleResponse(() => TokenApi.delete(`${url}/deleteJigs/${id}`));
};
