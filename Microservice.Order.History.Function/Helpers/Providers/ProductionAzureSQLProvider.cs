using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Microservice.Order.History.Function.Helpers.Providers;

public class ProductionAzureSQLProvider : SqlAuthenticationProvider
{
    private readonly IConfiguration _configuration;
    private readonly TokenCredential _credential;

    public ProductionAzureSQLProvider(IConfiguration configuration)
    {
        _configuration = configuration;

        // Retrieve Managed Identity client ID from configuration
        var clientId = _configuration["AzureSettings:UserAssignedManagedIdentityClientId"];

        // Initialize TokenCredential with Managed Identity client ID
        _credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
        {
            ManagedIdentityClientId = clientId,
        });
    }

    private static readonly string[] _azureSqlScopes =
    [
        "https://database.windows.net//.default"
    ];

    public override async Task<SqlAuthenticationToken> AcquireTokenAsync(SqlAuthenticationParameters parameters)
    {
        var tokenRequestContext = new TokenRequestContext(_azureSqlScopes);
        var tokenResult = await _credential.GetTokenAsync(tokenRequestContext, default);
        return new SqlAuthenticationToken(tokenResult.Token, tokenResult.ExpiresOn);
    }

    public override bool IsSupported(SqlAuthenticationMethod authenticationMethod) => authenticationMethod.Equals(SqlAuthenticationMethod.ActiveDirectoryManagedIdentity);
}