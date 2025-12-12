# Feedback Analyzer - .NET 10 with Azure Integration

A real-time feedback analysis system built with .NET 10, Blazor, and Azure Cognitive Services.

## Features

- **Real-time Feedback Submission**: Users can submit feedback through a Blazor web application
- **AI-Powered Sentiment Analysis**: Integrates with Azure Cognitive Services for sentiment analysis
- **Live Updates**: Real-time dashboard updates using SignalR
- **Persistent Storage**: Feedback and analysis results stored in Azure SQL Database
- **Modern UI**: Clean, responsive Blazor Server interface

## Architecture

- **Frontend**: Blazor Server (.NET 10)
- **Backend**: ASP.NET Core Web API (.NET 10)
- **Database**: Azure SQL Database (or SQL Server LocalDB for development)
- **AI Service**: Azure Cognitive Services - Text Analytics
- **Real-time Communication**: SignalR

## Project Structure

```
FeedbackAnalyzer/
├── src/
│   ├── FeedbackAnalyzer.API/          # Web API backend
│   │   ├── Controllers/               # API controllers
│   │   ├── Data/                      # Entity Framework DbContext
│   │   ├── Hubs/                      # SignalR hubs
│   │   ├── Services/                  # Business logic services
│   │   └── appsettings.json          # API configuration
│   ├── FeedbackAnalyzer.Web/          # Blazor frontend
│   │   ├── Components/
│   │   │   ├── Pages/                # Blazor pages
│   │   │   └── Layout/               # Layout components
│   │   └── appsettings.json          # Web app configuration
│   └── FeedbackAnalyzer.Shared/       # Shared models
│       └── Models/                    # Data models
└── FeedbackAnalyzer.sln              # Solution file
```

## Prerequisites

- .NET 10 SDK
- Azure Cognitive Services account (Text Analytics)
- SQL Server or Azure SQL Database
- Visual Studio 2022 or later / VS Code

## Configuration

### API Configuration (appsettings.json)

Update the following settings in `src/FeedbackAnalyzer.API/appsettings.json`:

```json
{
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

### Web Configuration (appsettings.json)

Update the API URL in `src/FeedbackAnalyzer.Web/appsettings.json`:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7000",
    "HubUrl": "https://localhost:7000/feedbackhub"
  }
}
```

## Setup Instructions

### 1. Azure Cognitive Services Setup

1. Create a Text Analytics resource in Azure Portal
2. Copy the endpoint URL and API key
3. Update the `AzureCognitiveServices` section in the API's appsettings.json

### 2. Database Setup

#### Using SQL Server LocalDB (Development)

```bash
cd src/FeedbackAnalyzer.API
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### Using Azure SQL Database (Production)

1. Create an Azure SQL Database
2. Update the connection string in appsettings.json
3. Run the migrations as shown above

### 3. Running the Application

#### Option 1: Run both projects simultaneously

**Terminal 1 - API:**
```bash
cd src/FeedbackAnalyzer.API
dotnet run
```

**Terminal 2 - Web:**
```bash
cd src/FeedbackAnalyzer.Web
dotnet run
```

#### Option 2: Using Visual Studio

1. Right-click on the solution
2. Select "Set Startup Projects"
3. Choose "Multiple startup projects"
4. Set both FeedbackAnalyzer.API and FeedbackAnalyzer.Web to "Start"
5. Press F5

### 4. Access the Application

- **Web Application**: https://localhost:5001
- **API**: https://localhost:7000
- **API Swagger**: https://localhost:7000/openapi/v1.json

## Usage

1. **Submit Feedback**: Navigate to the "Submit Feedback" page and enter your feedback
2. **View Dashboard**: Open the "Dashboard" page to see real-time sentiment analysis
3. **Live Updates**: Keep the dashboard open while submitting feedback to see live updates

## API Endpoints

- `POST /api/feedback` - Submit new feedback
- `GET /api/feedback/{id}` - Get specific feedback
- `GET /api/feedback/recent?count=50` - Get recent feedback
- `GET /api/feedback/trends` - Get sentiment trends

## SignalR Hub

- **Hub URL**: `/feedbackhub`
- **Events**:
  - `ReceiveFeedback` - New feedback received
  - `ReceiveSentimentTrends` - Updated sentiment trends

## Development Notes

- The API runs on port 7000 (HTTPS) by default
- The Web app runs on port 5001 (HTTPS) by default
- CORS is configured to allow the Web app to communicate with the API
- SignalR automatically reconnects if the connection is lost

## Security Considerations

- Never commit API keys or connection strings to source control
- Use Azure Key Vault for production secrets
- Configure appropriate CORS origins for production
- Use managed identities when deploying to Azure

## License

See LICENSE file for details.