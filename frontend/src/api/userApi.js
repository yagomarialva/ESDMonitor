import axios from 'axios';
import TokenApi from "./TokenApi";

const url = 'api/Biometric/'
const API_URL = process.env.REACT_APP_API_URL;
const API_URL_USERS = process.env.REACT_APP_API_URL;

export const getUsers = async () => {
    const response = await TokenApi.get('api/Biometric/ListUsersPaginated?page=1&pageSize=50');
    return response.data;
};

export const getAllUsers = async () => {
    const response = await TokenApi.get(`api/Biometric/ListUsersPaginated?page=1&pageSize=50`);
    return response.data.value;
};

export const getUser = async (id) => {
    const response = await axios.get(`${API_URL}/users/${id}`);
    return response.data;
};

export const createUser = async (user) => {
    const response = await axios.post(`${API_URL}/users`, user);
    return response.data;
};

export const updateUser = async (id, user) => {
    const response = await axios.put(`${API_URL}/users/${id}`, user);
    return response.data;
};

export const deleteUser = async (id) => {
    await axios.delete(`${API_URL}/users/${id}`);
};
