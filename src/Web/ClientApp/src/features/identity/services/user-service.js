import { api } from "@/shared/lib/api-client";
import { apiAuth } from "@/shared/lib/api-auth";

export const loginRequest = (username, password) =>
  api.post(`/api/v1/identity/login`, {
    UserName: username,
    Password: password,
  });

export const logoutRequest = () => apiAuth.post(`/api/v1/identity/logout`);

export const registerNewUser = (username, email, password) =>
  api.post(`/api/v1/identity/register`, {
    userName: username,
    email: email,
    password: password,
  });

export const fetchProfile = () => apiAuth.get(`/api/v1/identity/profile`);
