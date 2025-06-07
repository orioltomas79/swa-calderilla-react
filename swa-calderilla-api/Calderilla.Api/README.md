# **Calderilla.Api**

## Overview

**Calderilla.Api** is a **.NET 8.0 Azure Function** app built using the **Isolated Worker Model**.
With .NET Functions isolated worker process, you can much more easily add configurations, inject dependencies, and run your own middleware.

- **Isolated Worker Model**: Offers a decoupled hosting model for better flexibility.  
  [Learn more about the isolated process model](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=hostbuilder%2Cwindows).

- **`IHostApplicationBuilder` Support**: Simplifies application startup and configuration.  
  [Read more about `IHostApplicationBuilder`](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows#start-up-and-configuration).

## Program.cs

**Program.cs** is the entry point for the app.

- To integrate **ASP.NET Core** features into **Azure Functions** using the **isolated worker model**, we can utilize the builder.ConfigureFunctionsWebApplication() method.
  This approach allows us to **leverage familiar ASP.NET Core capabilities within our Azure Functions app**.
  [Learn more about ASP.NET Core integration](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows#aspnet-core-integration)

- To incorporate **middleware** into Azure Functions using the .NET isolated worker model, you can leverage the builder.ConfigureFunctionsWebApplication() method.
  This approach enables the integration of **ASP.NET Core middleware components into our function app**.
  [Learn more about ASP.NET Core middlewares](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows#middleware)

- In Azure Functions using the .NET isolated worker model, we can implement **Dependency Injection (DI)** to manage services and their lifetimes effectively
  [Learn more about dependency injection (DI)](https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide?tabs=ihostapplicationbuilder%2Cwindows#dependency-injection)

## OpenApi

Microsoft.Azure.Functions.Worker.Extensions.OpenApi is the recommended library for **OpenAPI support in Azure Functions**. It provides a set of attributes and classes to define and document your APIs.
https://github.com/Azure/azure-functions-openapi-extension/blob/main/docs/openapi-out-of-proc.md

**This project is in maintenance mode** and will receive no further feature updates. Longer term, Microsoft intend to integrate with **Microsoft.AspNetCore.OpenApi** for apps on the isolated worker model.
https://github.com/Azure/azure-functions-openapi-extension/issues/683

Even when using the `Microsoft.Azure.Functions.Worker.Extensions.OpenApi` package in the isolated worker model,
the OpenAPI attributes such as [OpenApiOperation] are defined in the `Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes` namespace.

This design choice stems from the fact that the azure-functions-openapi-extension library, which provides OpenAPI support for Azure Functions, maintains a single set of attribute definitions in the WebJobs namespace. **These attributes are utilized by both in-process and isolated worker models**.

Therefore, when developing Azure Functions with the isolated worker model and integrating OpenAPI documentation using the `Microsoft.Azure.Functions.Worker.Extensions.OpenApi` package, it's expected and appropriate to use the `Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes` namespace for OpenAPI attributes.

## Error handling

This API uses a **custom middleware** called: `ExceptionHandlingMiddleware`. Its purpose is to **handle unhandled exceptions** during the execution of Azure Functions, log them, and return a structured HTTP response to the client: 500 error.

`ProblemDetailsHelper.cs` and `ValidationProblemDetailsHelper.cs` are utility classes designed to simplify the creation of standardized error responses in an ASP.NET Core application. They leverage the **ProblemDetails** and **ValidationProblemDetails** classes, which are **part of the ASP.NET Core framework**, to provide structured error information in **compliance with the RFC 7807 specification** for problem details.

## Authentication and authorization

`StaticWebAppsAuth.cs` provides utilities for handling authentication in **Azure Static Web Apps**. It parses the `x-ms-client-principal` header to extract user information, such as **identity provider**, **user ID**, **user details**, and **roles**, and converts them into a `ClaimsPrincipal`. In debug mode, it supports local testing by creating a mock `ClientPrincipal` if the header is absent. The class also includes extension methods to retrieve the user's name, ID, and roles from a `ClaimsPrincipal`. This enables seamless integration of authentication data into the application.

`ValidateUserMiddleware.cs` is a **custom middleware** that validates user authentication and authorization. It uses `StaticWebAppsAuth` to extract a `ClaimsPrincipal` from the HTTP request. If the user claims are missing or the user lacks the required role, it returns an unauthorized response with appropriate problem details. The middleware logs unauthorized access attempts and ensures **only authorized users can proceed to the next middleware or function**.

## The functions folder

The **functions folder contains the Azure Functions** that are part of the API. 
**Each function** is defined in its **own class**, and the folder structure reflects the organization of the API endpoints.

## Local Development Settings

To run this project locally, you need a `local.settings.json` file in the `Calderilla.Api` directory with the following configuration:

```json
{
  "IsEncrypted": false,
  "Values": {
    "StorageAccountConnectionString": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated"
  },
  "ConnectionStrings": {}
}
```

This file is required for local development and should not be committed to source control. It configures the Azure Functions runtime and the connection string for local storage emulation.