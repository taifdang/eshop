import axios, { AxiosError } from "axios";

export const apiAuth = axios.create({
  baseURL: "/bff",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
    "X-CSRF": "1",
  },
});

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
      //tokenStorage.clear();
      
      // Redirect to BFF login with return URL
      const returnUrl = encodeURIComponent(window.location.pathname + window.location.search);
      window.location.href = `/bff/login?returnUrl=${returnUrl}`;
      
      return Promise.reject(error);
    }
    
    return Promise.reject(error);
  }
);
