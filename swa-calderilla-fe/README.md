# React + TypeScript + Vite

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) uses [Babel](https://babeljs.io/) for Fast Refresh
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) uses [SWC](https://swc.rs/) for Fast Refresh

## TypeScript Configuration Files

This project uses multiple TypeScript configuration files to separate concerns for different parts of the codebase:

- **tsconfig.json**  
  The root configuration file. It acts as a project reference manager and does not directly specify compiler options for building code. Instead, it references the other config files (`tsconfig.app.json` and `tsconfig.node.json`) to organize settings for different environments.

- **tsconfig.app.json**  
  Contains TypeScript options for the main application source code (everything in the `src` directory). It is tailored for browser/React development, including settings for JSX, DOM libraries, and strict type checking.

- **tsconfig.node.json**  
  Contains TypeScript options for Node.js-specific files, such as build scripts or configuration files (e.g., `vite.config.ts`). It targets a more recent version of ECMAScript and includes only the necessary libraries for Node.js, not browser code.

This separation helps ensure that the right TypeScript settings are applied to the right parts of the project, improving build reliability and developer experience.

## Vite Configuration Files

- **vite.config.ts**  
  This is the main configuration file for Vite. It defines how Vite should build and serve your project. In this project, it imports the React plugin to enable React Fast Refresh and other optimizations. You can customize this file to add more plugins or adjust Vite’s behavior.

- **vite-env.d.ts**  
  This file provides TypeScript with type definitions for Vite-specific features, such as environment variables and the Vite client API. The reference to `"vite/client"` ensures that TypeScript recognizes Vite’s global types throughout your project.

## Linting with Vite and React 19

This project uses ESLint to maintain code quality and consistency for both TypeScript and React 19 code. Linting is integrated into the development workflow and works seamlessly with Vite, ensuring that code issues are caught early during development.

- **Vite Integration:** Vite supports fast feedback and hot module replacement, so linting errors can be surfaced quickly as you code. You can run ESLint manually or configure your editor to show lint errors inline.
- **React 19 Support:** The ESLint configuration is compatible with React 19, including support for the latest hooks and React-specific linting rules. Plugins like `eslint-plugin-react-hooks` and `eslint-plugin-react-refresh` are included to enforce best practices and enable Fast Refresh compatibility.

### Purpose of `eslint.config.js`

The `eslint.config.js` file defines the linting rules and environment for the project. Its main responsibilities are:

- Extending recommended ESLint and TypeScript rules for robust static analysis.
- Enabling React-specific linting, including hooks and Fast Refresh support.
- Setting up the environment for browser globals and ECMAScript 2020.
- Specifying which files to lint (e.g., all `.ts` and `.tsx` files).
- Customizing or extending rules as needed for your codebase.

This configuration ensures that your code adheres to best practices for both TypeScript and React 19, and that you benefit from automated feedback as you develop.

## Code Formatting with Prettier

This project uses [Prettier](https://prettier.io/) to automatically format code for consistency and readability. Prettier enforces a uniform style by parsing your code and reprinting it with its own rules, making it easier to maintain clean and readable code across the team.

### Prettier Configuration (`.prettierrc`)

The `.prettierrc` file in this project specifies the following formatting rules:

- **singleQuote: false** — Use double quotes for strings instead of single quotes.
- **trailingComma: "es5"** — Add trailing commas where valid in ES5 (objects, arrays, etc., but not in function parameters).

You can run Prettier manually or configure your editor to format files automatically on save. This helps ensure that all code in the repository follows the same style guidelines.

## Project Dependency Files

- **package.json**  
  This file defines the project's metadata, scripts, dependencies, and development dependencies. It lists all the npm packages required to run and develop the project, as well as scripts for common tasks like starting the dev server (`dev`), building the project (`build`), running the linter (`lint`), and previewing the production build (`preview`).

### Scripts explained

| Script  | Command                                           | Description                                                                                                                        |
| ------- | ------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------------------- |
| dev     | vite                                              | Starts the Vite development server for local development                                                                           |
| build   | tsc -b && vite build                              | Builds TypeScript files and then creates a production build with Vite                                                              |
| lint    | eslint .                                          | Runs ESLint to check for code quality and style issues                                                                             |
| preview | vite preview                                      | vite runs the development server with hot module replacement, while vite preview serves the production build locally for testing.  |
| format  | prettier --write src/\*_/_.{ts,tsx,scss,css,json} | Formats all source files (TypeScript, SCSS, CSS, JSON) in the src directory using Prettier according to the project's style rules. |
| test | vitest | Runs all unit and component tests using Vitest. |
| test:coverage | vitest run --coverage | Runs all tests and generates a code coverage report in the coverage/ folder. |


## Testing with Vitest

This project uses [Vitest](https://vitest.dev/) for unit and component testing, providing a fast and modern testing experience that integrates seamlessly with Vite and React.

### How to Implement Tests

- **Test Files:**  
  Place your test files alongside your source files or in a dedicated `tests` folder. Use the `.test.ts`, `.test.tsx`, or similar naming convention.

- **Test Environment:**  
  The test environment is set to `jsdom`, which simulates a browser-like environment for React component testing.

### Configuration Details

#### `vite.config.ts`

- Adds the `@vitejs/plugin-react` plugin for React support.
- Configures Vitest with:
  - `globals: true` to allow using global test functions (like `test`, `expect`) without importing them.
  - `environment: 'jsdom'` for browser-like testing.

#### `vite-env.d.ts`

- References Vitest’s global types with:
  ```
  /// <reference types="vitest/globals" />
  ```
  This enables TypeScript to recognize Vitest’s global test functions and types in your test files.

---

#### Purpose of Key Testing Libraries

- **@testing-library/jest-dom**:  
  Provides custom DOM element matchers for Jest and Vitest, such as `toBeInTheDocument()` and `toHaveTextContent()`. These improve the readability and power of your assertions when testing React components.

- **@testing-library/react**:  
  Supplies utilities to render React components in a test environment and interact with them as a user would. It encourages best practices by focusing on testing components from the user's perspective.

- **@testing-library/user-event**:  
  Simulates user interactions (like clicks, typing, and more complex events) in your tests, making it possible to test how your app responds to real user behavior.

- **vitest**:  
  The main testing framework used in this project. It provides a fast, modern, and Vite-native test runner with a Jest-like API, supporting TypeScript and React out of the box.

- **@vitest/coverage-v8**:  
  Adds code coverage reporting to Vitest using the V8 engine. It helps you track which parts of your codebase are tested and identify untested code.

## Recommended VS Code Extensions

To improve your development workflow, it is recommended to install the following Visual Studio Code extensions:

- **Prettier - Code formatter**: Automatically formats your code according to the project's style rules every time you save a file. This helps maintain consistent code style across the team and reduces formatting issues in pull requests.

  - After installing, enable "Format on Save" in your VS Code settings to ensure your code is always formatted automatically.

- **Vitest**: Adds UI integration for running and debugging your Vitest tests directly from the Visual Studio Code interface. This extension makes it easy to view test results, re-run tests, and debug failures without leaving your editor.
