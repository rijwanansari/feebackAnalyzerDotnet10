# Architecture Documentation

## System Overview

The Feedback Analyzer is a real-time sentiment analysis system built with .NET 10, consisting of three main components:

1. **Blazor Web Application** - User interface for submitting feedback and viewing dashboards
2. **ASP.NET Core Web API** - Backend service handling business logic and data processing
3. **Azure Services** - Cloud services for AI and data storage

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                         Client Browser                           │
│                                                                   │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │         Blazor Web Application (Port 5001)                │  │
│  │                                                           │  │
│  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐   │  │
│  │  │    Home      │  │   Submit     │  │  Dashboard   │   │  │
│  │  │    Page      │  │   Feedback   │  │    (Live)    │   │  │
│  │  └──────────────┘  └──────────────┘  └──────────────┘   │  │
│  │         │                  │                  │           │  │
│  │         └──────────────────┴──────────────────┘           │  │
│  │                           │                                │  │
│  │                  ┌────────▼────────┐                      │  │
│  │                  │  SignalR Client  │                      │  │
│  │                  │   (Real-time)    │                      │  │
│  │                  └─────────────────┘                      │  │
│  └───────────────────────────────────────────────────────────┘  │
└───────────────────────────┬─────────────────────────────────────┘
                            │
                            │ HTTPS + SignalR WebSocket
                            │
┌───────────────────────────▼─────────────────────────────────────┐
│              ASP.NET Core Web API (Port 7000)                    │
│                                                                   │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                    Controllers                           │    │
│  │  ┌───────────────────────────────────────────────────┐  │    │
│  │  │          FeedbackController                       │  │    │
│  │  │  - POST /api/feedback                            │  │    │
│  │  │  - GET  /api/feedback/{id}                       │  │    │
│  │  │  - GET  /api/feedback/recent                     │  │    │
│  │  │  - GET  /api/feedback/trends                     │  │    │
│  │  └───────────────────────────────────────────────────┘  │    │
│  └─────────────────────────────────────────────────────────┘    │
│                            │                                      │
│  ┌─────────────────────────▼──────────────────────────────┐     │
│  │                   Services Layer                        │     │
│  │  ┌──────────────────────┐  ┌────────────────────────┐  │     │
│  │  │  FeedbackService     │  │ SentimentAnalysis     │  │     │
│  │  │  - Create            │  │ Service               │  │     │
│  │  │  - Get Recent        │  │ - Analyze Sentiment   │  │     │
│  │  │  - Get Trends        │  │                       │  │     │
│  │  └──────────────────────┘  └────────────────────────┘  │     │
│  └─────────────────────────────────────────────────────────┘     │
│                            │                                      │
│  ┌─────────────────────────▼──────────────────────────────┐     │
│  │                    Data Layer                           │     │
│  │  ┌─────────────────────────────────────────────────┐   │     │
│  │  │         FeedbackDbContext (EF Core)              │   │     │
│  │  │         - DbSet<Feedback>                        │   │     │
│  │  └─────────────────────────────────────────────────┘   │     │
│  └─────────────────────────────────────────────────────────┘     │
│                            │                                      │
│  ┌─────────────────────────▼──────────────────────────────┐     │
│  │                  SignalR Hub                            │     │
│  │  ┌─────────────────────────────────────────────────┐   │     │
│  │  │           FeedbackHub                            │   │     │
│  │  │  - ReceiveFeedback                              │   │     │
│  │  │  - ReceiveSentimentTrends                       │   │     │
│  │  └─────────────────────────────────────────────────┘   │     │
│  └─────────────────────────────────────────────────────────┘     │
└───────────────────┬─────────────────────────┬───────────────────┘
                    │                         │
                    │                         │
        ┌───────────▼──────────┐   ┌─────────▼────────────────┐
        │  Azure Cognitive     │   │   Azure SQL Database     │
        │  Services            │   │   or SQL Server          │
        │  (Text Analytics)    │   │                          │
        │  - Sentiment         │   │  ┌────────────────────┐  │
        │    Analysis          │   │  │  Feedbacks Table   │  │
        │                      │   │  │  - Id              │  │
        │                      │   │  │  - Text            │  │
        │                      │   │  │  - Sentiment       │  │
        │                      │   │  │  - Score           │  │
        │                      │   │  │  - CreatedAt       │  │
        │                      │   │  │  - Category        │  │
        │                      │   │  └────────────────────┘  │
        └──────────────────────┘   └──────────────────────────┘
```

## Component Details

### 1. Blazor Web Application

**Technology**: Blazor Server with .NET 10

**Responsibilities**:
- Render user interface
- Handle user interactions
- Maintain SignalR connection for real-time updates
- Display feedback submission form
- Show real-time dashboard with sentiment trends

**Key Components**:
- `Home.razor` - Landing page
- `FeedbackSubmission.razor` - Form for submitting feedback
- `Dashboard.razor` - Real-time dashboard with SignalR integration

### 2. Web API

**Technology**: ASP.NET Core Web API with .NET 10

**Responsibilities**:
- Expose RESTful API endpoints
- Process feedback submissions
- Coordinate with Azure Cognitive Services
- Manage database operations
- Broadcast real-time updates via SignalR

**Layers**:

#### Controllers
- Handle HTTP requests
- Validate input
- Return appropriate responses

#### Services
- **FeedbackService**: Business logic for feedback management
- **AzureSentimentAnalysisService**: Integration with Azure Cognitive Services

#### Data Access
- **FeedbackDbContext**: Entity Framework Core DbContext
- **Migrations**: Database schema management

#### SignalR Hub
- **FeedbackHub**: Real-time communication hub

### 3. Shared Library

**Technology**: .NET 10 Class Library

**Responsibilities**:
- Define shared data models
- Ensure consistency between API and Web projects

**Models**:
- `Feedback` - Main feedback entity
- `CreateFeedbackRequest` - DTO for creating feedback
- `SentimentAnalysisResult` - Sentiment analysis response

## Data Flow

### Feedback Submission Flow

```
1. User enters feedback in Blazor form
   │
   ▼
2. Form submits HTTP POST to API
   │
   ▼
3. API receives request at FeedbackController
   │
   ▼
4. Controller calls FeedbackService.CreateFeedbackAsync()
   │
   ▼
5. FeedbackService calls SentimentAnalysisService
   │
   ▼
6. SentimentAnalysisService sends text to Azure Cognitive Services
   │
   ▼
7. Azure returns sentiment analysis (Positive/Negative/Neutral + scores)
   │
   ▼
8. FeedbackService saves feedback + sentiment to database
   │
   ▼
9. Controller broadcasts new feedback via SignalR Hub
   │
   ▼
10. All connected dashboard clients receive real-time update
    │
    ▼
11. Dashboard UI updates automatically
```

### Real-time Dashboard Flow

```
1. User opens Dashboard page
   │
   ▼
2. Dashboard loads initial data via HTTP GET
   │
   ▼
3. Dashboard establishes SignalR connection to hub
   │
   ▼
4. When new feedback arrives:
   │
   ├─▶ Hub broadcasts "ReceiveFeedback" event
   │
   └─▶ Hub broadcasts "ReceiveSentimentTrends" event
       │
       ▼
5. Dashboard receives events and updates UI
```

## Technology Stack

### Frontend
- **Blazor Server** - Interactive web UI framework
- **Bootstrap 5** - UI styling
- **SignalR Client** - Real-time communication

### Backend
- **ASP.NET Core Web API** - REST API framework
- **Entity Framework Core** - ORM for database access
- **SignalR** - Real-time communication framework

### Azure Services
- **Azure Cognitive Services (Text Analytics)** - AI-powered sentiment analysis
- **Azure SQL Database** - Cloud database (or SQL Server for local development)

### Development Tools
- **.NET 10 SDK** - Development framework
- **Entity Framework Tools** - Database migrations

## Security Considerations

1. **API Keys**: Stored in appsettings.json (development) or Azure Key Vault (production)
2. **CORS**: Configured to allow only specific origins
3. **HTTPS**: All communication encrypted
4. **Connection Strings**: Managed securely via configuration
5. **Input Validation**: API validates all incoming requests

## Scalability Considerations

1. **SignalR**: Can be scaled out using Azure SignalR Service
2. **Database**: Azure SQL Database supports elastic pools and scaling
3. **API**: Stateless design allows horizontal scaling
4. **Caching**: Can add Redis cache for frequently accessed data

## Deployment Options

### Development
- Run locally with SQL Server LocalDB
- Use Azure Cognitive Services free tier

### Production
- Deploy API to Azure App Service
- Deploy Web to Azure App Service
- Use Azure SQL Database
- Use Azure SignalR Service for scale-out
- Store secrets in Azure Key Vault

## Monitoring and Logging

- Built-in ASP.NET Core logging
- Configurable log levels in appsettings.json
- Can integrate with Application Insights for production monitoring

## Future Enhancements

1. Authentication and authorization
2. User accounts and personalized dashboards
3. Advanced analytics and reporting
4. Export feedback data
5. Webhook notifications
6. Multi-language support
7. Sentiment history and trends over time
8. Real-time alerts for negative feedback
