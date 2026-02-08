import axios, { AxiosError } from "axios";

export const apiClient = axios.create({
  baseURL: "/bff",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
    "X-CSRF": "1",
  },
});