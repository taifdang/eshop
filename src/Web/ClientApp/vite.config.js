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
      port: parseInt(env.VITE_PORT),
      proxy: {
        "/api": {
          target:
            process.env.services__apiservice__https__0 ||
            process.env.services__apiservice__http__0,
          changeOrigin: true,
          // rewrite: (path) => path.replace(/^\/api/, "api"),
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
