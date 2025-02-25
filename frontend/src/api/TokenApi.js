import axios from "axios";

const TokenApi = axios.create({
    baseURL: process.env.REACT_APP_API_URL_FCT,
});

TokenApi.interceptors.request.use(config => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers['Authorization'] = `Bearer ${token}`;
    } else {
        console.error("Token not found in localStorage");
    }
    return config;
}, error => {
    return Promise.reject(error);
});

export default TokenApi;
