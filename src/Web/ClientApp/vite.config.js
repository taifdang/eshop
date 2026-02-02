import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react";
import p from "path";
import tailwindcss from "@tailwindcss/vite";

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");

  return {
    plugins: [react(), tailwindcss()],
    server: {
      port: 3000,
      host: true,
      // proxy for dev
      // proxy: {
      //   "/api": {
      //     target:
      //       process.env.services__apiservice__https__0 ||
      //       process.env.services__apiservice__http__0  || "http://api:80",
      //     changeOrigin: true,
      //     secure: false,
      //   },
      // },
      proxy: {
        "/bff": {
          target: "https://localhost:5002",         
          changeOrigin: true,
          secure: false,
        },
      },
    },
    build: {
      outDir: "dist",
      rollupOptions: {
        input: "./index.html",
      },
    },

    resolve: {
      alias: {
        "@": p.resolve(__dirname, "./src"),
      },
    },
    css: {
      modules: {
        generateScopedName: "[hash:base64:6]",
      },
    },
  };
});
