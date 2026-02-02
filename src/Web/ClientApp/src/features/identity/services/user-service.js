import { api } from "@/shared/lib/api-client";
import { apiAuth } from "@/shared/lib/api-auth";

/* OLD: Direct API calls for authentication
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
*/

// NEW: BFF-based authentication endpoints
// Login is now handled by redirecting to BFF login page
// No longer needed as loginRequest since BFF handles the login form
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

export const fetchProfile = () => api.get(`/userinfo`);
 