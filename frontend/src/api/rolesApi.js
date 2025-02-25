import TokenApi from "./TokenApi";


export const getAllRoles = async () => {
    const response = await TokenApi.get('/todosRolees');
    return response.data;
};

export const getRoles = async (id) => {
    const response = await TokenApi.get(`/BuscarRolees${id}`);
    return response.data;
};

export const createRoles = async (monitors) => {
    const response = await TokenApi.post('/adicionarRole', monitors);
    return response.data;
};

export const updateRoles = async (id, monitors) => {
    const response = await TokenApi.put(`/adicionarRole${id}`, monitors);
    return response.data;
};

export const deleteRoles = async (id) => {
    await TokenApi.delete(`/deleteRole?id=${id}`);
};
