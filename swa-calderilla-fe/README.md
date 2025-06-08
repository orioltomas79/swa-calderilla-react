# React + TypeScript + Vite

This template provides a minimal setup to get React working in Vite with HMR and some ESLint rules.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) uses [Babel](https://babeljs.io/) for Fast Refresh
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) uses [SWC](https://swc.rs/) for Fast Refresh

## Material UI Integration

This project uses [Material UI (MUI)](https://mui.com/) as its main React UI component library, providing a modern, accessible, and customizable set of components that follow Google’s Material Design guidelines.

### Key Dependencies

- **@mui/material**: The core Material UI component library for React, offering a wide range of ready-to-use UI components.
- **@mui/icons-material**: Provides Material Design icons as React components for use throughout the app.
- **@emotion/react** and **@emotion/styled**: Peer dependencies required by MUI for styling components using the Emotion CSS-in-JS library.
- **@fontsource/roboto**: Supplies the Roboto font, which is the default font for Material UI components and recommended for a consistent Material Design look.

These dependencies are already included in the project’s `package.json`. You can use Material UI components and icons directly in your React code. For more information and usage examples, see the [Material UI documentation](https://mui.com/).

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

| Script             | Command                                                                                                                                                                                                                                                                         | Description                                                                                                                                                                 |
| ------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| dev                | vite                                                                                                                                                                                                                                                                            | Starts the Vite development server for local development                                                                                                                    |
| build              | tsc -b && vite build                                                                                                                                                                                                                                                            | Builds TypeScript files and then creates a production build with Vite                                                                                                       |
| lint               | eslint .                                                                                                                                                                                                                                                                        | Runs ESLint to check for code quality and style issues                                                                                                                      |
| preview            | vite preview                                                                                                                                                                                                                                                                    | vite runs the development server with hot module replacement, while vite preview serves the production build locally for testing.                                           |
| format             | prettier --write src/\*_/_.{ts,tsx,scss,css,json}                                                                                                                                                                                                                               | Formats all source files (TypeScript, SCSS, CSS, JSON) in the src directory using Prettier according to the project's style rules.                                          |
| test               | vitest                                                                                                                                                                                                                                                                          | Runs all unit and component tests using Vitest.                                                                                                                             |
| test:coverage      | vitest run --coverage                                                                                                                                                                                                                                                           | Runs all tests and generates a code coverage report in the coverage/ folder.                                                                                                |
| api:generate       | pnpm run api:generate:nswag && npm run api:postgenerate                                                                                                                                                                                                                         | Runs the full API client generation workflow: first generates the TypeScript client from the backend OpenAPI (Swagger) spec, then formats the generated file with Prettier. |
| api:generate:nswag | nswag openapi2tsclient /typeScriptVersion:4.9 /input:http://localhost:7072/api/swagger.json /output:src/api/apiClient.g.nswag.ts /template:fetch /operationGenerationMode:MultipleClientsFromFirstTagAndOperationName /typeStyle:Interface /nullValue:null /dateTimeType:string | Uses NSwag to generate the TypeScript API client from the backend's OpenAPI (Swagger) specification. The output is written to `src/api/apiClient.g.nswag.ts`.               |
| api:postgenerate   | prettier --write src/api/apiClient.g.nswag.ts                                                                                                                                                                                                                                   | Formats the autogenerated API client file with Prettier to ensure consistent code style.                                                                                    |

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

## Backend API Client Generation

To interact with the backend, this project uses an autogenerated TypeScript client based on the OpenAPI (Swagger) specification provided by the backend. The client is generated using [NSwag](https://github.com/RicoSuter/NSwag).

### How the Client is Generated

The client is generated with the following command, which is included in the `package.json` scripts:

```
nswag openapi2tsclient /typeScriptVersion:4.9 /input:http://localhost:7072/api/swagger.json /output:src/api/apiClient.g.nswag.ts /template:fetch /operationGenerationMode:MultipleClientsFromFirstTagAndOperationName /typeStyle:Interface /nullValue:null /dateTimeType:string
```

This command fetches the OpenAPI specification from the backend and generates a TypeScript client in `src/api/apiClient.g.nswag.ts`. The generated client uses the Fetch API and is tailored for our backend's endpoints and data types.

### File Structure and Usage

- **`src/api/apiClient.g.nswag.ts`**  
  This is the autogenerated file containing all the TypeScript classes and interfaces for calling the backend API.  
  **Do not edit this file manually.** It will be overwritten each time the client is regenerated.

- **`src/api/apiClient.ts`**  
  This file wraps the autogenerated client, providing a single `apiClient` instance with all the endpoint clients preconfigured.  
  The rest of the codebase should use this file to access backend endpoints, ensuring a consistent and centralized API usage.

- **`src/api/types.ts`**  
  This file re-exports the main types (`Operation`, `CurrentAccount`, etc.) from the autogenerated client.  
  Use these types throughout the codebase for type safety and consistency.

### Best Practices

- **Do not import or use `apiClient.g.nswag.ts` directly in your application code.**  
  Always use `apiClient.ts` for API calls and `types.ts` for type imports.  
  This abstraction allows us to update or regenerate the API client without affecting the rest of the codebase.

---

## Recommended VS Code Extensions

To improve your development workflow, it is recommended to install the following Visual Studio Code extensions:

- **Prettier - Code formatter**: Automatically formats your code according to the project's style rules every time you save a file. This helps maintain consistent code style across the team and reduces formatting issues in pull requests.

  - After installing, enable "Format on Save" in your VS Code settings to ensure your code is always formatted automatically.

- **Vitest**: Adds UI integration for running and debugging your Vitest tests directly from the Visual Studio Code interface. This extension makes it easy to view test results, re-run tests, and debug failures without leaving your editor.

## Authentication and Authorization in Azure Static Web Apps

This project uses [Azure Static Web Apps](https://learn.microsoft.com/en-us/azure/static-web-apps/) built-in authentication and authorization system to secure API routes and manage user login/logout flows.

### How Authentication Works

Azure Static Web Apps provides built-in authentication endpoints under the `/.auth` path. When a user visits the app, they can authenticate using supported identity providers (such as GitHub, Microsoft, Google, etc.).

- **Login:** The React app includes a link: `<a href="/.auth/login/github">Login</a>`. Clicking this link redirects the user to GitHub's OAuth login flow. Upon successful authentication, the user is redirected back to the app with a valid session.
- **Logout:** The `<a href="/.auth/logout">Log out</a>` link ends the user's session and logs them out of the app.

### Authorization and Route Protection

The `staticwebapp.config.json` file configures how authentication and authorization are enforced:

- **API Route Protection:**

  ```json
  {
    "route": "/api/*",
    "allowedRoles": ["authenticated"]
  }
  ```

  All API routes under `/api/*` require the user to be authenticated. Unauthenticated users are not allowed to access these endpoints.

- **Login Provider Restriction:**

  ```json
  {
    "route": "/.auth/login/aad",
    "statusCode": 404
  }
  ```

  The Azure Active Directory (AAD) login endpoint is disabled by returning a 404 status, so only GitHub login is available.

- **401 Response Override:**

  ```json
  "responseOverrides": {
    "401": {
      "statusCode": 302,
      "redirect": "/.auth/login/github"
    }
  }
  ```

  If an unauthenticated user tries to access a protected resource (such as an API route), they are automatically redirected to the GitHub login page.

- **SPA Navigation Fallback:**

  ```json
  "navigationFallback": {
    "rewrite": "/index.html"
  }
  ```

  Ensures that all non-API routes are served by the React single-page application, supporting client-side routing.

  ## Routing with React Router (Declarative Mode)

This application uses [React Router](https://reactrouter.com/) in [declarative mode](https://reactrouter.com/start/declarative/installation) to manage client-side navigation.

Declarative routing allows you to define your application's routes directly in your React component tree, making navigation predictable and easy to maintain.

### Routing: `<Routes>` and `<Route>`

- **`<BrowserRouter>`**: Wraps your app and enables client-side routing using the browser’s history API.
- **`<Routes>`**: Contains all your route definitions. It matches the current URL to the best `<Route>` and renders its element.
- **`<Route>`**: Defines a path and the React element to render for that path.
  - Example from your code:
    ```tsx
    <Routes>
      <Route path="/" element={<App />}>
        <Route index element={<Home />} />
        <Route path="about" element={<About />} />
      </Route>
    </Routes>
    ```
    - The root path `/` renders the `App` component.
    - The index route (just `/`) renders `Home` inside `App`.
    - The `/about` path renders `About` inside `App`.

### Navigating: `<Link>`, `<NavLink>`, and `useNavigate`

- **`<Link to="..." />`**: Renders an anchor tag that, when clicked, navigates to the specified route without reloading the page.
  - Example: `<Link to="/about">About</Link>`
- **`<NavLink to="..." />`**: Like `<Link>`, but adds styling (e.g., an `active` class) when the link matches the current URL. Useful for navigation menus.
- **`useNavigate()`**: A React hook that returns a function for programmatic navigation.
  - Example:
    ```tsx
    const navigate = useNavigate();
    // Navigate to /about on some event
    navigate("/about");
    ```

### URL Values: Route Params

- **Dynamic Segments**: You can define routes with dynamic segments using `:paramName`.
  - Example: `<Route path="user/:userId" element={<User />} />`
- **Route Params**: When a user visits `/user/42`, the `userId` param will be `"42"`.
- **Accessing Params**: Use the `useParams()` hook inside your component to get the parsed values.
  - Example:
    ```tsx
    import { useParams } from "react-router-dom";
    const { userId } = useParams(); // userId will be "42" for /user/42
    ```

**Summary:**

- `<Routes>` and `<Route>` define which components render for which URLs.
- `<Link>`, `<NavLink>`, and `useNavigate` let you navigate between routes declaratively or programmatically.
- Route params let you extract dynamic values from the URL for use in your components.

## Global Current Accounts Context

This project uses a React Context to efficiently manage and provide access to the list of current accounts throughout the application. This ensures that the list of accounts is fetched from the backend only once per app session, improving performance and reducing unnecessary API calls.

### How It Works

- The context is implemented in `src/contexts/CurrentAccountsContext.tsx`.
- The `CurrentAccountsProvider` component wraps the application at the top level (see `main.tsx`).
- The provider fetches the list of current accounts from the backend using the `getCurrentAccounts` endpoint when the app first loads.
- The result is stored in context and made available to all components via the `useCurrentAccounts` hook.
- Any component can access the list, loading, and error state by calling `useCurrentAccounts()`.
- The data is only fetched once per app session, regardless of how many times you navigate between pages.

### Usage Example

Access the accounts in any component:

```tsx
import { useCurrentAccounts } from "../contexts/CurrentAccountsContext";

const { listCurrentAccounts, loading, error } = useCurrentAccounts();
```

This pattern ensures that the list of accounts is always available, up-to-date, and only fetched once per session.

---
