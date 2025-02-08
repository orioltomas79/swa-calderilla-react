using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Calderilla.Api
{

    public static class StaticWebAppsAuth
    {
        private class ClientPrincipal
        {
            public string? IdentityProvider { get; set; }
            public string? UserId { get; set; }
            public string? UserDetails { get; set; }
            public IEnumerable<string>? UserRoles { get; set; }
        }

        public static ClaimsPrincipal GetClaimsPrincipal(HttpRequest req)
        {
            var principal = new ClientPrincipal();

            if (req.Headers.TryGetValue("x-ms-client-principal", out var header))
            {
                var data = header[0];
                var decoded = Convert.FromBase64String(data!);
                var json = Encoding.UTF8.GetString(decoded);
                principal = JsonSerializer.Deserialize<ClientPrincipal>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (principal == null)
                {
                    throw new InvalidOperationException("Invalid client principal.");
                }
            }

#if DEBUG
            // For local testing only.
            // We can test the backend without the frontend by creating a ClientPrincipal manually
            // if the header x-ms-client-principal is not present.
            if (principal.UserRoles == null)
            {
                principal = new ClientPrincipal
                {
                    IdentityProvider = "github",
                    UserId = "fake-user-id",
                    UserDetails = "fake-user-name",
                    UserRoles = ["authenticated", "user"]
                };
            }
#endif

            principal.UserRoles = principal.UserRoles?.Except(new string[] { "anonymous" }, StringComparer.CurrentCultureIgnoreCase);

            if (!principal.UserRoles?.Any() ?? true)
            {
                return new ClaimsPrincipal();
            }

            var identity = new ClaimsIdentity(principal.IdentityProvider);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId!));
            identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails!));
            identity.AddClaims(principal.UserRoles!.Select(r => new Claim(ClaimTypes.Role, r)));

            return new ClaimsPrincipal(identity);
        }

        public static string GetNameIdentifier(this ClaimsPrincipal input)
        {
            return input.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
        }

        public static string GetName(this ClaimsPrincipal input)
        {
            return input.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        }

        public static List<string> GetRoles(this ClaimsPrincipal input)
        {
            return input.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        }
    }
}
