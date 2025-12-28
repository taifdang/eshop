import { createContext, useContext, useRef, useLayoutEffect } from "react";
import {
  fetchProfile,
  loginRequest,
  logoutRequest,
  registerNewUser,
} from "../services/user-service";
import { tokenStorage } from "@/shared/storage/token-storage";
import { profileStorage } from "@/shared/storage/profile-storage";
const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const trackuser = useRef(false);

  const login = async (username, password) => {
    try {
      const res = await loginRequest(username, password);
      if (res.status === 200) {
        tokenStorage.set(res.data.token);
        console.log(`login-token ${res.data.token}`);
        await handleLoadProfile();
        return { success: true };
      }
      return { success: false, message: "Invalid input" };
    } catch (error) {
      if (error.response?.status === 400) {
        return { success: false, message: "Invalid input" };
      }
      return { success: false, message: "Server error" };
    }
  };

  const logout = async () => {
    try {
      await logoutRequest();
      // clear storage
      tokenStorage.clear();
      profileStorage.clear();
      // redirect
      window.location.href = "/login";
    } catch (error) {
      tokenStorage.clear();
      profileStorage.clear();
      throw error;
    }
  };

  const signup = async (username, email, password) => {
    try {
      var res = await registerNewUser(username, email, password);
      //redirect
      if (res.status === 200) {
        return { success: true };
      }
      return { success: false, message: "Invalid input" };
    } catch (error) {
      if (error.response?.status === 400) {
        return { success: false, message: "Invalid input" };
      }
      return { success: false, message: "Server error" };
    }
  };

  const handleLoadProfile = async () => {
    try {
      const { data } = await fetchProfile();
      if (data) {
        profileStorage.set(data);
      }
      console.log(`user-login ${JSON.stringify(data)}`);
    } catch {
      tokenStorage.clear();
      setUser(null);
    }
  };

  useLayoutEffect(() => {
    const initAuth = async () => {
      if (trackuser.current) return;
      trackuser.current = true;
      try {
        const { data } = await fetchProfile();
        if (data) {
          profileStorage.set(data);
        }
        console.log(`user-login ${JSON.stringify(data)}`);
      } catch {
        // clear storage
        tokenStorage.clear();
        profileStorage.clear();
      }
    };
    initAuth();
  }, []);

  return (
    <AuthContext.Provider value={{ login, logout, signup }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error("outside scope !");
  }
  return ctx;
};
