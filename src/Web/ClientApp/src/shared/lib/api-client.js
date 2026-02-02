import axios, { AxiosError } from "axios";

/* OLD: Direct API calls to backend
export const api = axios.create({
  baseURL: import.meta.env.VITE_BACKEND_API,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

api.interceptors.response.use(
  (response) => response,
  (error) => Promise.reject(error)
);
*/

// NEW: API calls through BFF proxy
export const api = axios.create({
  baseURL: "/bff",
  withCredentials: true, // Essential for cookie-based authentication
  headers: {
    "Content-Type": "application/json",
    "X-CSRF": "1", // Anti-forgery token for BFF
  },
});

// api.interceptors.response.use(
//   (response) => response,
//   (error) => {
//     // If unauthorized, redirect to BFF login
//     if (error.response?.status === 401) {
//       const returnUrl = encodeURIComponent(window.location.pathname + window.location.search);
//       window.location.href = `${import.meta.env.VITE_BFF_URL || import.meta.env.REACT_APP_BFF_URL || "https://localhost:5002/bff"}/login?returnUrl=${returnUrl}`;
//     }
//     return Promise.reject(error);
//   }
// );
