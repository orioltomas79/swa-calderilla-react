namespace Calderilla.Api.Functions.Dev
{
    public static class ApiEndpoints
    {
        // Dev endpoints
        public const string DevEndpointsTag = "DevEndpoints";
        public const string GetValidationError = "dev/get-bad-request-error";
        public const string GetNotFoundError = "dev/get-not-found-error";
        public const string GetInternalServerError = "dev/get-internal-server-exception";
        public const string GetMessage = "dev/get-message";

        // User endpoints
        public const string UsersEndpointsTag = "UserEndpoints";
        public const string GetUserClaims = "user/get-user-claims";

        // Operations endpoints
        public const string OperationsEndpointsTag = "OperationsEndpoints";
        public const string GetOperations = "operations/{accountId:guid}/{year:int}/{month:int}";
        public const string GetOperationTypes = "operation-types";
        public const string PatchOperation = "operations/{accountId:guid}/{year:int}/{month:int}/{operationId:guid}";

        // Ing operations
        public const string IngEndpointsTag = "IngEndpoints";
        public const string UploadIngBankExtract = "ing/upload-bank-extract/{accountId:guid}/{year:int}/{month:int}";

        // Sabadell operations
        public const string SabadellEndpointsTag = "SabadellEndpoints";
        public const string UploadSabadellBankExtract = "sabadell/upload-bank-extract/{accountId:guid}/{year:int}/{month:int}";

        // Current accounts endpoints
        public const string CurrentAccountsEndpointsTag = "CurrentAccountsEndpoints";
        public const string GetCurrentAccounts = "accounts";

        public const string GetCurrentAccountYearlySummary = "accounts/{accountId:guid}/summary/{year:int}";
    }
}
