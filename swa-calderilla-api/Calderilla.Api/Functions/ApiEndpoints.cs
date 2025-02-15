﻿namespace Calderilla.Api.Functions.Dev
{
    public static class ApiEndpoints
    {
        public const string DevEndpointsTag = "DevEndpoints";
        public const string GetValidationError = "dev/get-bad-request-error";
        public const string GetNotFoundError = "dev/get-not-found-error";
        public const string GetInternalServerError = "dev/get-internal-server-exception";
        public const string GetMessage = "dev/get-message";

        public const string UsersEndpointsTag = "UserEndpoints";
        public const string GetUserClaims = "user/get-user-claims";

        public const string OperationsEndpointsTag = "OperationsEndpoints";
        public const string GetOperations = "operations/{currentAccount:guid}/{year:int}/{month:int}";

        public const string CurrentAccountsEndpointsTag = "CurrentAccountsEndpoints";
        public const string GetCurrentAccounts = "current-accounts";
    }
}
