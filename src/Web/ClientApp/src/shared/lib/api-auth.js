import axios, { AxiosError } from "axios";
import { tokenStorage } from "../storage/token-storage";

/* OLD: Direct API with token-based authentication
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
*/

// NEW: BFF-based authentication using cookies
export const apiAuth = axios.create({
  baseURL: "/bff",
  withCredentials: true, // Essential for cookie-based authentication
  headers: {
    "Content-Type": "application/json",
    "X-CSRF": "1", // Anti-forgery token for BFF
  },
});

// BFF handles authentication via cookies - no need for manual token management
apiAuth.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (!error.response) {
      return Promise.reject(error);
    }

    const originRequest = error.config;

    // If unauthorized, redirect to BFF login
    if (error.response.status === 401 && !originRequest._retry) {
      originRequest._retry = true;
      console.log("[401]: Unauthorized - redirecting to BFF login");
      
      // Clear any client-side storage
      tokenStorage.clear();
      
      // Redirect to BFF login with return URL
      const returnUrl = encodeURIComponent(window.location.pathname + window.location.search);
      window.location.href = `$/bff/login?returnUrl=${returnUrl}`;
      
      return Promise.reject(error);
    }
    
    return Promise.reject(error);
  }
);
