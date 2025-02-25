import TokenApi from "./TokenApi";

const url = "api/LinkStationAndLine";
// Obtém todos os monitores
export const getAllLinks = async () => {
  try {
    const { data } = await TokenApi.get(`${url}/todosLinks`);
    return data;
  } catch (error) {
    console.error("Error fetching all monitors:", error);
    throw error;
  }
};

// Obtém um monitor específico por ID
export const getLink = async (id) => {
  try {
    const { data } = await TokenApi.get(`/BuscarLink/${id}`);
    return data;
  } catch (error) {
    console.error(`Error fetching monitor with ID ${id}:`, error);
    throw error;
  }
};

// Cria um novo monitor
export const createLink = async (link) => {
  try {
    const { data } = await TokenApi.post(`${url}/criarLink`, link);
    return data;
  } catch (error) {
    console.error("Error creating link:", error);
    throw error;
  }
};

// Atualiza um monitor existente por ID
export const updateLink = async (link) => {
  console.log("updated link", link);
  try {
    const { data } = await TokenApi.post(`${url}/criarLink`, link);
    return data;
  } catch (error) {
    console.error(`Error updating monitor with ID:`, error);
    throw error;
  }
};

// Deleta um monitor por ID
export const deleteLink = async (id) => {
  try {
    await TokenApi.delete(`${url}/${id}`);
  } catch (error) {
    console.error(`Error deleting monitor with ID ${id}:`, error);
    throw error;
  }
};
