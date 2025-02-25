import TokenApi from "./TokenApi";

export interface UserData {
  username: string;
  password: string;
  rolesName: string;
  badge: string;
}

export const createUser = async (data: UserData) => {
  try {
    await TokenApi.post("/api/Authentication/criacao", data);
    return { success: true };
  } catch (error: any) {
    return { success: false, message: error.response.data.errors.Password };
  }
};
