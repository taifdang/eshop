import { useState, useContext, createContext } from "react";

const PreviewContext = createContext(null);

export const PreviewProvider = ({ children, item }) => {
  const [image, setImage] = useState(item);

  return (
    <PreviewContext.Provider value={{ image, setImage }}>
      {children}
    </PreviewContext.Provider>
  );
};

export const usePreview = () => {
  const ctx = useContext(PreviewContext)
  if(!ctx){
    throw new Error("outside scope !");
  }
  return ctx;
};
