import axios, { AxiosError } from "axios";
import { tokenStorage } from "../storage/token-storage";

export const apiAuth = axios.create({
  baseURL: import.meta.env.VITE_BACKEND_API,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

apiAuth.interceptors.request.use(
  (config) => {
    const token = tokenStorage.get();

    //console.log(`current-token: ${token}`);
    //&& !config._retry
    if (token && !config._retry) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error)
);

apiAuth.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (!error.response) {
      return Promise.reject(error);
    }

    //500: Internal server error -> don't send token
    //401: Unauthorized
    //403: Forbidden
    //(options) originRequest._retry // mark, completed

    const originRequest = error.config;

    if (error.response.status == 401 && !originRequest._retry) {
      originRequest._retry = true;
      console.log("[401]: unauthoried. processing revoke");

      try {
        var res = await apiAuth.post(`/api/v1/identity/refresh-token`);
        //
        //console.log(`get refresh-token ${res.data.token}`);

        tokenStorage.set(res.data.token);

        originRequest.headers.Authorization = `Bearer ${res.data.token}`;

        return apiAuth(originRequest); // revoke api
      } catch (err) {
        console.log(`get refresh-token fail ${err}`);
        tokenStorage.clear();

        return Promise.reject(err);
      }
    }
    return Promise.reject(error);
  }
);
