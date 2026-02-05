/* OLD v1
import {
  createContext,
  useContext,
  useRef,
  useLayoutEffect,
  useEffect,
} from "react";
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
*/

/* NEW v2 */
import { createContext, useMemo } from "react";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { fetchProfile } from "../services/user-service";

const AuthContext = createContext(null);

const BFF_BASE_URL =
  import.meta.env.VITE_BFF_URL ||
  import.meta.env.REACT_APP_BFF_URL ||
  "https://localhost:5002/bff";

const getProfileSafe = async () => {
  try {
    const { data } = await fetchProfile();
    return data ?? null;
  } catch (error) {
    if (error?.response?.status === 401 || error?.status === 401) {
      return null;
    }
    return null;
  }
};

export const AuthProvider = ({ children }) => {
  const queryClient = useQueryClient();

  const {
    data: user,
    isLoading,
    isFetching,
    error,
    refetch,
  } = useQuery({
    queryKey: ["auth", "profile"],
    queryFn: getProfileSafe,
    staleTime: 60_000,
    retry: false,
  });

  const login = async () => {
    const returnUrl = encodeURIComponent(
      window.location.pathname + window.location.search,
    );
    window.location.href = `${BFF_BASE_URL}/login?returnUrl=${returnUrl}`;
    return { success: true };
  };

  const logout = async () => {
    queryClient.setQueryData(["auth", "profile"], null);
    const returnUrl = encodeURIComponent(window.location.origin);
    window.location.href = `${BFF_BASE_URL}/logout?returnUrl=${returnUrl}`;
    return { success: true };
  };

  const signup = async () => {
    const returnUrl = encodeURIComponent(
      window.location.pathname + window.location.search,
    );
    window.location.href = `${BFF_BASE_URL}/register?returnUrl=${returnUrl}`;
    return { success: true };
  };

  const value = useMemo(
    () => ({
      user,
      isAuthenticated: Boolean(user),
      isLoading,
      isFetching,
      error,
      login,
      logout,
      signup,
      refetchProfile: refetch,
    }),
    [user, isLoading, isFetching, error, refetch],
  );

  /* OLD v1 */
  // /* OLD: Token-based login via direct API
  // const login = async (username, password) => {
  //   try {
  //     const res = await loginRequest(username, password);
  //     if (res.status === 200) {
  //       tokenStorage.set(res.data.token);
  //       await handleLoadProfile();
  //       return { success: true };
  //     }
  //     return { success: false, message: "Invalid input" };
  //   } catch (error) {
  //     if (error.response?.status === 400) {
  //       return { success: false, message: "Invalid input" };
  //     }
  //     return { success: false, message: "Server error" };
  //   }
  // };
  // */
  //
  // // NEW: BFF-based login via redirect
  // const login = async (username, password) => {
  //   // In BFF architecture, login is handled by redirecting to BFF login page
  //   // This function is deprecated - use window.location.href to redirect
  //   const returnUrl = encodeURIComponent(window.location.pathname + window.location.search);
  //   window.location.href = `${import.meta.env.VITE_BFF_URL || import.meta.env.REACT_APP_BFF_URL || "https://localhost:5002/bff"}/login?returnUrl=${returnUrl}`;
  // };
  //
  // /* OLD: Token-based logout via direct API
  // const logout = async () => {
  //   try {
  //     await logoutRequest();
  //     // clear storage
  //     tokenStorage.clear();
  //     profileStorage.clear();
  //     // redirect
  //     window.location.href = "/login";
  //   } catch (error) {
  //     tokenStorage.clear();
  //     profileStorage.clear();
  //     throw error;
  //   }
  // };
  // */
  //
  // // NEW: BFF-based logout via redirect
  // const logout = async () => {
  //   try {
  //     // Clear client storage
  //     tokenStorage.clear();
  //     profileStorage.clear();
  //     // Redirect to BFF logout endpoint
  //     //await logoutRequest();
  //   } catch (error) {
  //     tokenStorage.clear();
  //     profileStorage.clear();
  //     throw error;
  //   }
  // };
  //
  // /* OLD: Direct registration via API
  // const signup = async (username, email, password) => {
  //   try {
  //     var res = await registerNewUser(username, email, password);
  //     //redirect
  //     if (res.status === 200) {
  //       return { success: true };
  //     }
  //     return { success: false, message: "Invalid input" };
  //   } catch (error) {
  //     if (error.response?.status === 400) {
  //       return { success: false, message: "Invalid input" };
  //     }
  //     return { success: false, message: "Server error" };
  //   }
  // };
  // */
  //
  // // NEW: BFF architecture - signup handled by identity provider
  // const signup = async (username, email, password) => {
  //   // In BFF architecture, registration is typically handled by the identity provider
  //   // This function is deprecated
  //   throw new Error("Direct registration not supported in BFF architecture. Use identity provider registration.");
  // };
  //
  // /* OLD: Set user from token
  // const handleLoadProfile = async () => {
  //   try {
  //     const { data } = await fetchProfile();
  //     if (data) {
  //       profileStorage.set(data);
  //     }
  //   } catch {
  //     tokenStorage.clear();
  //     setUser(null);
  //   }
  // };
  // */
  //
  // // NEW: Load profile from BFF user endpoint
  // const handleLoadProfile = async () => {
  //   try {
  //     const { data } = await fetchProfile();
  //     if (data) {
  //       profileStorage.set(data);
  //     }
  //   } catch {
  //     // If profile fetch fails, clear storage
  //     // BFF will handle redirecting to login if needed
  //     tokenStorage.clear();
  //     profileStorage.clear();
  //   }
  // };
  //
  // useEffect(() => {
  //   const initAuth = async () => {
  //     if (trackuser.current) return;
  //     trackuser.current = true;
  //     try {
  //       const { data } = await fetchProfile();
  //       if (data) {
  //         profileStorage.set(data);
  //         console.log("User info:", data);
  //       } else {
  //         console.log("User not authenticated");
  //         return;
  //       }
  //     } catch {
  //       // clear storage
  //       tokenStorage.clear();
  //       profileStorage.clear();
  //       // BFF will handle authentication state
  //     }
  //   };
  //   initAuth();
  // }, []);

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export { AuthContext };

/* OLD v1
export const useAuth = () => {
  const ctx = useContext(AuthContext);
  if (!ctx) {
    throw new Error("outside scope !");
  }
  return ctx;
};
*/
