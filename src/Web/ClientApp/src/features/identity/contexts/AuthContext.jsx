import { createContext, useState } from "react";

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState();
  const [accessToken, setAccessToken] = useState(null);

  return (
    <AuthProvider value = {{accessToken, setAccessToken}}>
      {children}
    </AuthProvider>
  );
};
