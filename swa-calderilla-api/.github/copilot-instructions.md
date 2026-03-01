# Calderilla API - AI Coding Agent Instructions

## Architecture Overview

**Calderilla** is a layered financial operations API built on **Azure Functions (.NET 8 Isolated Worker Model)** with Azure Static Web Apps authentication.

### Layer Structure
- **API Layer** (`Calderilla.Api`): HTTP-triggered Azure Functions, request orchestration
- **Services Layer** (`Calderilla.Services`): Business logic, operations processing, bank parsing
- **DataAccess Layer** (`Calderilla.DataAccess`): Azure Blob Storage abstraction
- **Domain Layer** (`Calderilla.Domain`): Shared models (Operation, CurrentAccount, OperationType)

Data flows: Request → Function → Service → Repository → Blob Storage

## Critical Developer Workflows

### Build & Run
- `dotnet build` in `Calderilla.Api` folder (or use VS Code task "build (functions)")
- **Local development requires**:
  - `local.settings.json` with `StorageAccountConnectionString: UseDevelopmentStorage=true`
  - **Azurite blob storage**: Run `azurite-blob` before starting functions
  - Functions host: `func start` or task "start functions host on 7072"

### Testing
- `dotnet test` from root runs all test projects
- Unit tests use **Moq** for mocking services

## Project-Specific Patterns

### 1. Azure Function Definition
```csharp
public class GetOperationsFunction(ILogger<GetOperationsFunction> logger, IOperationsService service)
{
    [Function(nameof(GetOperations))]
    [OpenApiOperation(operationId: "GetOps", tags: [ApiEndpoints.OperationsEndpointsTag])]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, bodyType: typeof(List<Operation>))]
    public async Task<IActionResult> GetOperations(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = ApiEndpoints.GetOperations)] 
        HttpRequest req, 
        Guid accountId, int year, int month)
    {
        var userId = StaticWebAppsAuth.GetUserId(req); // Extract from x-ms-client-principal
        return new OkObjectResult(await service.GetAsync(userId, accountId, year, month));
    }
}
```

**Key conventions**:
- Route constants in `ApiEndpoints.cs` (centralized URL definitions)
- Use **C# 12 primary constructor syntax** for dependency injection
- **OpenAPI attributes** use `Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes` namespace (not `.Core.Isolated.*`)
- Extract user identity via `StaticWebAppsAuth.GetUserId(req)` from `x-ms-client-principal` header
- Each function is a separate class, organized by endpoint category (Banks/User/Operation/CurrentAccount)

### 2. Service Layer Pattern
- Implement `IXxxService` interface in constructor
- **Validate inputs inline** (null checks, range validation) before delegating to repositories
- Services coordinate multiple repositories and contain business logic
- Example: `OperationsService.EnrichOperationTypeAsync()` enriches operation types using last 12 months of data

### 3. Repository Pattern
- `IBlobRepository`: Generic read/write lists to blobs as JSON
- `IOperationsRepository`: Higher-level operations (GetOperationsAsync, SaveOperationsAsync)
- Blob naming hierarchy: `{userId}/{accountId}/{year}/{month}/operations.json`
- Containers auto-created on first write

### 4. Error Handling
- **Middleware-based**: `ExceptionHandlingMiddleware` catches unhandled exceptions globally (logs, returns 500)
- **Validation errors**: `ValidationProblemDetails` for 400 responses (RFC 7807 compliant)
- **Not found errors**: `ProblemDetails` with 404 status
- Response format: `{ "type": "...", "status": 500, "title": "...", "detail": "...", "instance": "/path" }`

### 5. Authentication & Authorization
- **StaticWebAppsAuth**: Extracts `ClaimsPrincipal` from `x-ms-client-principal` HTTP header
- **ValidateUserMiddleware**: Enforces user presence; returns 401 if unauthenticated
- Both middleware run in `Program.cs` via `builder.UseMiddleware<T>()`
- Debug mode: Creates mock principal if header absent (for local testing)

## File Organization

```
Calderilla.Api/
├── Functions/              # HTTP-triggered functions, one class per endpoint
│   ├── Banks/             # Bank extract uploads (ING, Sabadell)
│   ├── Operations/        # Operation retrieval and management
│   ├── CurrentAccount/    # Account endpoints
│   ├── User/              # User claims
│   └── Dev/               # Test endpoints (validation errors, 404, 500)
├── Auth/                  # Authentication utilities
├── ErrorHandling/         # Middleware and error formatting
└── Program.cs             # DI setup, middleware registration
```

## Key Dependencies to Know

- **Azure.Storage.Blobs**: Blob operations
- **Microsoft.Azure.Functions.Worker**: Azure Functions isolated model
- **Microsoft.Azure.Functions.Worker.Extensions.OpenApi**: API documentation
- **Async patterns**: Use `.ConfigureAwait(false)` on Task operations
- **NPOI**: Excel parsing (e.g., ING bank extracts)

## Common Tasks

**Adding a new function**:
1. Create class in `Functions/{Category}/MyFunction.cs`
2. Add route constant to `ApiEndpoints.cs`
3. Use constructor injection for dependencies
4. Decorate with `[Function]`, `[HttpTrigger]`, OpenAPI attributes
5. Extract user ID via `StaticWebAppsAuth.GetUserId(req)`
6. Return `IActionResult` (OkObjectResult, BadRequestObjectResult, UnauthorizedObjectResult)

**Adding service logic**:
1. Implement `IXxxService` interface
2. Inject repositories in constructor
3. Validate all inputs
4. Call repositories to persist/retrieve
5. Register singleton in `Program.cs` under `AddMyServices()`

**Testing**:
1. Mock dependencies with Moq
2. Create HttpRequest via `new DefaultHttpContext().Request`
3. Use `FormFile` for file uploads in tests
