import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  test: {
    globals: true, // Enables global test functions like 'describe' and 'it'
    environment: "jsdom", // Simulates a browser-like environment
    setupFiles: "./vitest.setup.ts", // File for global test setup
    exclude: ['e2e_tests/**', 'node_modules/**', 'dist/**'],
    coverage: {
      provider: "v8", // Use the 'v8' coverage provider
      reporter: ["text", "json", "html"],
    },
  },
});
