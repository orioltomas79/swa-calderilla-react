# GitHub Copilot Custom Instructions for .NET Azure Functions

- Target .NET 8 and the Azure Functions isolated worker model.
- Use IHostApplicationBuilder
- Use FunctionsApplicationBuilder
- Integrate ASP.NET Core features into Azure Function apps using ConfigureFunctionsWebApplication.
- Use dependency injection via `IServiceCollection`.
- Follow naming conventions: PascalCase for classes and methods, camelCase for local variables and parameters.
- Implement error handling with try-catch blocks and return appropriate HTTP status codes.
- Write unit tests using xUnit and Moq for mocking dependencies.
- Document public methods with XML comments.
- When creating new functions, include OpenAPI annotations for API documentation using Microsoft.Azure.Functions.Worker.Extensions.OpenApi
- Use `async` and `await` for all asynchronous operations.
- Avoid using `async void`; prefer `async Task` for asynchronous methods.
- Ensure all functions are idempotent and stateless.
