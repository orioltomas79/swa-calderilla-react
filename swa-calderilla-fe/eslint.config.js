// Import the base ESLint config for JavaScript
import js from "@eslint/js";

// Import a set of global variables for browser environments
import globals from "globals";

// Import the React Hooks ESLint plugin for enforcing hooks rules
import reactHooks from "eslint-plugin-react-hooks";

// Import the React Refresh plugin for Fast Refresh compatibility
import reactRefresh from "eslint-plugin-react-refresh";

// A new plugin offering improved and more granular rules for React components, especially around React 19 features.
// Includes rules to validate JSX, component lifecycles, and hooks.
import reactX from "eslint-plugin-react-x";

// Focuses on DOM-related React issues.
import reactDom from "eslint-plugin-react-dom";

// Import the TypeScript ESLint config utility
import tseslint from "typescript-eslint";

// Export the ESLint configuration using typescript-eslint's config helper
export default tseslint.config(
  // First argument: global config options
  {
    // Ignore the 'dist' and 'coverage' directories from linting
    ignores: ["dist", "coverage"],
  },

  // Second argument: configuration for TypeScript and React files
  {
    // Extend recommended rules from ESLint and TypeScript ESLint
    extends: [
      js.configs.recommended,
      ...tseslint.configs.recommendedTypeChecked,
    ],

    // Specify which files this config applies to
    files: ["**/*.{ts,tsx}"],

    languageOptions: {
      // Set ECMAScript version for parsing
      ecmaVersion: 2020,
      // Use browser global variables (e.g., window, document)
      globals: globals.browser,

      parserOptions: {
        // Specify the TypeScript project config files for type-aware linting
        project: ["./tsconfig.node.json", "./tsconfig.app.json"],
        // Set the root directory for resolving the tsconfig paths
        tsconfigRootDir: import.meta.dirname,
      },
    },

    plugins: {
      // Enables linting rules for React Hooks best practices
      "react-hooks": reactHooks,
      // Enables linting rules for React Fast Refresh compatibility
      "react-refresh": reactRefresh,
      // Provides improved and granular rules for React 19, including hooks, JSX, and component lifecycles
      "react-x": reactX,
      // Adds rules focused on React DOM-specific issues
      "react-dom": reactDom,
    },

    rules: {
      // Enforce the official recommended rules for React Hooks usage
      ...reactHooks.configs.recommended.rules,

      // Apply recommended rules from eslint-plugin-react-x for React 19 and TypeScript
      // These rules cover best practices for components, hooks, and JSX in React 19
      ...reactX.configs["recommended-typescript"].rules,

      // Apply recommended rules from eslint-plugin-react-dom for DOM-specific React issues
      ...reactDom.configs.recommended.rules,

      // Warn if components are not exported in a way compatible with React Fast Refresh
      // Setting allowConstantExport: true allows constant (arrow function) exports, which are common in modern React
      "react-refresh/only-export-components": [
        "warn",
        { allowConstantExport: true },
      ],
    },
  }
);
