# Configuration Guide

This document describes all configuration settings used in the Feedback Analyzer application.

## API Configuration (FeedbackAnalyzer.API)

### appsettings.json

Located at: `src/FeedbackAnalyzer.API/appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FeedbackAnalyzerDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "AzureCognitiveServices": {
    "Endpoint": "https://your-resource-name.cognitiveservices.azure.com/",
    "ApiKey": "your-api-key-here"
  },
  "CorsOrigins": "https://localhost:5001"
}
```

#### Configuration Properties

| Setting | Description | Default Value | Required |
|---------|-------------|---------------|----------|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string | LocalDB connection | Yes |
| `AzureCognitiveServices:Endpoint` | Azure Cognitive Services endpoint URL | Placeholder | Yes |
| `AzureCognitiveServices:ApiKey` | Azure Cognitive Services API key | Placeholder | Yes |
| `CorsOrigins` | Comma-separated allowed CORS origins | https://localhost:5001 | Yes |

### appsettings.Development.json

Located at: `src/FeedbackAnalyzer.API/appsettings.Development.json`

Development-specific overrides:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Information",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FeedbackAnalyzerDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "CorsOrigins": "https://localhost:5001,http://localhost:5001"
}
```

## Web Application Configuration (FeedbackAnalyzer.Web)

### appsettings.json

Located at: `src/FeedbackAnalyzer.Web/appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ApiSettings": {
    "BaseUrl": "https://localhost:7000",
    "HubUrl": "https://localhost:7000/feedbackhub"
  }
}
```

#### Configuration Properties

| Setting | Description | Default Value | Required |
|---------|-------------|---------------|----------|
| `ApiSettings:BaseUrl` | Base URL of the API | https://localhost:7000 | Yes |
| `ApiSettings:HubUrl` | SignalR hub URL | https://localhost:7000/feedbackhub | Yes |

## Environment-Specific Configuration

### Development

Use `appsettings.Development.json` for local development settings.

Set the environment:
```bash
export ASPNETCORE_ENVIRONMENT=Development
```

### Staging

Create `appsettings.Staging.json` for staging environment.

Set the environment:
```bash
export ASPNETCORE_ENVIRONMENT=Staging
```

### Production

Create `appsettings.Production.json` for production settings.

**Important:** Never include secrets in production config files. Use Azure Key Vault or environment variables.

Set the environment:
```bash
export ASPNETCORE_ENVIRONMENT=Production
```

## Using Environment Variables

All configuration values can be overridden using environment variables.

### Format

Use double underscores `__` to represent nested configuration:

```bash
# API
export ConnectionStrings__DefaultConnection="Server=..."
export AzureCognitiveServices__Endpoint="https://..."
export AzureCognitiveServices__ApiKey="..."
export CorsOrigins="https://myapp.com"

# Web
export ApiSettings__BaseUrl="https://api.myapp.com"
export ApiSettings__HubUrl="https://api.myapp.com/feedbackhub"
```

### Azure App Service

In Azure App Service, set these as Application Settings:

1. Go to your App Service
2. Navigate to Configuration > Application settings
3. Add new settings:
   - Name: `ConnectionStrings__DefaultConnection`
   - Value: Your connection string
   - Type: Connection string (for connection strings) or Setting (for others)

## Azure Key Vault Integration

For production, integrate with Azure Key Vault:

### 1. Install Package

```bash
dotnet add package Azure.Extensions.AspNetCore.Configuration.Secrets
dotnet add package Azure.Identity
```

### 2. Update Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    var keyVaultUrl = builder.Configuration["KeyVaultUrl"];
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl),
        new DefaultAzureCredential());
}
```

### 3. Store Secrets in Key Vault

Secret names in Key Vault should use dashes instead of colons:
- `AzureCognitiveServices--Endpoint`
- `AzureCognitiveServices--ApiKey`
- `ConnectionStrings--DefaultConnection`

## Port Configuration

Default ports are configured in `Properties/launchSettings.json`:

### API (FeedbackAnalyzer.API)
- HTTPS: 7000
- HTTP: 5000

### Web (FeedbackAnalyzer.Web)
- HTTPS: 5001
- HTTP: 5000

To change ports, edit `Properties/launchSettings.json` in each project.

## Connection String Formats

### SQL Server LocalDB
```
Server=(localdb)\\mssqllocaldb;Database=FeedbackAnalyzerDb;Trusted_Connection=True;MultipleActiveResultSets=true
```

### SQL Server (Windows Auth)
```
Server=SERVERNAME;Database=FeedbackAnalyzerDb;Trusted_Connection=True;MultipleActiveResultSets=true
```

### SQL Server (SQL Auth)
```
Server=SERVERNAME;Database=FeedbackAnalyzerDb;User Id=USERNAME;Password=PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true
```

### Azure SQL Database
```
Server=tcp:SERVERNAME.database.windows.net,1433;Initial Catalog=FeedbackAnalyzerDb;Persist Security Info=False;User ID=USERNAME;Password=PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

## Security Best Practices

1. **Never commit secrets to source control**
   - Add `appsettings.*.json` files with secrets to `.gitignore`
   - Use user secrets for local development

2. **Use Azure Key Vault in production**
   - Store all sensitive configuration in Key Vault
   - Use Managed Identity for authentication

3. **Rotate keys regularly**
   - Regenerate API keys periodically
   - Update connection strings when passwords change

4. **Use separate configurations per environment**
   - Different databases for dev/staging/production
   - Different API keys for each environment

## User Secrets (Development)

For local development, use .NET User Secrets:

```bash
# Navigate to API project
cd src/FeedbackAnalyzer.API

# Initialize user secrets
dotnet user-secrets init

# Set secrets
dotnet user-secrets set "AzureCognitiveServices:ApiKey" "your-actual-key"
dotnet user-secrets set "AzureCognitiveServices:Endpoint" "https://your-resource.cognitiveservices.azure.com/"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
```

User secrets are stored outside the project directory and are not committed to source control.

## Validating Configuration

The application validates critical configuration at startup. If required settings are missing, the application will fail to start with a clear error message indicating which configuration is missing.
