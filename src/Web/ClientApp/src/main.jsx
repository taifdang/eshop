import React, { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./styles/global.css";
import "./styles/index.css";
import { Providers } from "./app/providers";
import Root from "./app/root";

createRoot(document.getElementById("main")).render(
  <React.StrictMode>
    <Providers>
      <Root />
    </Providers>
  </React.StrictMode>
);
