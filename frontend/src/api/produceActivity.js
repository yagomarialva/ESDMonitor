import TokenApi from "./TokenApi";


export const getAllProduce = async () => {
    const response = await TokenApi.get('TodaProducao');
    return response.data;
};

export const getProduce = async (id) => {
    const response = await TokenApi.get(`/BuscarProducaoId?id=${id}`);
    return response.data;
};

export const createProduce = async (Produce) => {
    const response = await TokenApi.post('/adicionarMonitor', Produce);
    return response.data;
};

export const updateProduce = async (id, Produce) => {
    const response = await TokenApi.put(`/adicionarMonitor${id}`, Produce);
    return response.data;
};

export const deleteProduce = async (id) => {
    await TokenApi.delete(`/deleteMonitor?id=${id}`);
};
