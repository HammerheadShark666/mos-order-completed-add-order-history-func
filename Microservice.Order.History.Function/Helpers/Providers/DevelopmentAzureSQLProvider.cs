using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;

namespace Microservice.Order.History.Function.Helpers.Providers;

public class DevelopmentAzureSQLProvider : SqlAuthenticationProvider
{
    private readonly TokenCredential _credential;

    public DevelopmentAzureSQLProvider()
    {
        var clientId = EnvironmentVariables.LocalDevelopmentClientId;
        var clientSecret = EnvironmentVariables.LocalDevelopmentClientSecret;
        var tenantId = EnvironmentVariables.LocalDevelopmentTenantId;

        _credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
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

    public override bool IsSupported(SqlAuthenticationMethod authenticationMethod) => authenticationMethod.Equals(SqlAuthenticationMethod.ActiveDirectoryServicePrincipal);
}