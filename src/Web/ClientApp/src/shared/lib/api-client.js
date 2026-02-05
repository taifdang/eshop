import axios, { AxiosError } from "axios";

export const api = axios.create({
  baseURL: "/bff",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
    "X-CSRF": "1",
  },
});