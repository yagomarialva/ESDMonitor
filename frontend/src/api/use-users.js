import { useQuery } from "@tanstack/react-query"
import axios from "axios";

export const useUsers = () => {
  return useQuery({
    queryKey: ['users'],
    queryFn: async () => {
     const { data } = await axios.get(
      'https://jsonplaceholder.typicode.com/users'
     );
     return data;
    }
  });
}