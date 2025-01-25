import js from "@eslint/js";
import globals from "globals";
import reactHooks from "eslint-plugin-react-hooks";
import reactRefresh from "eslint-plugin-react-refresh";
import tseslint from "typescript-eslint";
import react from "eslint-plugin-react";

/**
 * ESLint configuration for the project.
 *
 * @type {import("eslint").Linter.Config}
 * @property {string[]} ignores - Directories to ignore during linting.
 * @property {Object} extends - ESLint configurations to extend.
 * @property {string[]} files - File patterns to include in linting.
 * @property {Object} languageOptions - Language options for ESLint.
 * @property {number} languageOptions.ecmaVersion - ECMAScript version to use.
 * @property {Object} languageOptions.globals - Global variables to use.
 * @property {Object} languageOptions.parserOptions - Parser options for TypeScript.
 * @property {string[]} languageOptions.parserOptions.project - Paths to TypeScript configuration files.
 * @property {string} languageOptions.parserOptions.tsconfigRootDir - Root directory for TypeScript configuration.
 * @property {Object} settings - Additional settings for ESLint.
 * @property {Object} settings.react - React-specific settings.
 * @property {string} settings.react.version - React version to use.
 * @property {Object} plugins - Plugins to use with ESLint.
 * @property {Object} plugins["react-hooks"] - React hooks plugin configuration.
 * @property {Object} plugins["react-refresh"] - React refresh plugin configuration.
 * @property {Object} plugins.react - React plugin configuration.
 * @property {Object} rules - Custom rules for ESLint.
 * @property {Object} rules["react-refresh/only-export-components"] - Rule configuration for react-refresh.
 */
export default tseslint.config(
  { ignores: ["dist", "coverage"] },
  {
    extends: [
      js.configs.recommended,
      ...tseslint.configs.recommendedTypeChecked,
    ],
    files: ["**/*.{ts,tsx}"],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
      parserOptions: {
        project: ["./tsconfig.node.json", "./tsconfig.app.json"],
        tsconfigRootDir: import.meta.dirname,
      },
    },
    settings: { react: { version: "18.3" } },
    plugins: {
      "react-hooks": reactHooks,
      "react-refresh": reactRefresh,
      react,
    },
    rules: {
      ...react.configs.recommended.rules,
      ...react.configs["jsx-runtime"].rules,
      ...reactHooks.configs.recommended.rules,
      "react-refresh/only-export-components": [
        "warn",
        { allowConstantExport: true },
      ],
    },
  }
);
