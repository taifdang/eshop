import { apiClient } from "@/shared/lib/api-client";
import { apiAuth } from "@/shared/lib/api-auth";

export const loginRequest = () => {
  throw new Error("Direct login not supported in BFF architecture. Redirect to /bff/login instead.");
};

// Logout redirects to BFF logout endpoint
export const logoutRequest = () => {
  const returnUrl = encodeURIComponent(window.location.origin);
  window.location.href = `${import.meta.env.VITE_BFF_URL}/logout`;
};

export const logout = () => {
  const returnUrl = encodeURIComponent(window.location.origin);
  window.location.href = `${import.meta.env.VITE_BFF_URL}/logout`;
};

export const registerNewUser = () => {
  throw new Error("Direct registration not supported in BFF architecture. Use identity provider registration.");
};

export const fetchProfile = () => apiClient.get(`/userinfo`);
 