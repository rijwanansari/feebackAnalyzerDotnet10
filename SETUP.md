# Setup Guide for Feedback Analyzer

This guide will walk you through setting up the Feedback Analyzer application.

## Prerequisites

Before starting, ensure you have:
- .NET 10 SDK installed
- SQL Server or SQL Server LocalDB (for development)
- An Azure account with Cognitive Services access

## Step 1: Clone the Repository

```bash
git clone https://github.com/rijwanansari/feebackAnalyzerDotnet10.git
cd feebackAnalyzerDotnet10
```

## Step 2: Azure Cognitive Services Setup

### Create Text Analytics Resource

1. Log in to the [Azure Portal](https://portal.azure.com)
2. Click "Create a resource"
3. Search for "Text Analytics" or "Language Service"
4. Click "Create"
5. Fill in the required details:
   - Subscription: Select your subscription
   - Resource Group: Create new or use existing
   - Region: Choose a region near you
   - Name: Give it a unique name (e.g., "feedback-analyzer-textanalytics")
   - Pricing Tier: Choose Free (F0) for development or Standard (S) for production

6. Click "Review + Create" and then "Create"
7. Once deployed, navigate to the resource
8. Go to "Keys and Endpoint" section
9. Copy the **Endpoint URL** and **Key 1**

### Update Configuration

Open `src/FeedbackAnalyzer.API/appsettings.json` and update:

```json
{
  "AzureCognitiveServices": {
    "Endpoint": "YOUR_ENDPOINT_URL_HERE",
    "ApiKey": "YOUR_API_KEY_HERE"
  }
}
```

**Important**: For production, use Azure Key Vault or environment variables instead of storing keys in appsettings.json

## Step 3: Database Setup

### Option A: SQL Server LocalDB (Development - Recommended for Testing)

LocalDB is installed with Visual Studio and is ideal for development.

1. Verify LocalDB is installed:
```bash
sqllocaldb info
```

2. The default connection string in `appsettings.Development.json` should work:
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=FeedbackAnalyzerDb;Trusted_Connection=True;MultipleActiveResultSets=true"
```

3. Create/update the database:
```bash
cd src/FeedbackAnalyzer.API
dotnet ef database update
```

### Option B: SQL Server (Local or Remote)

1. Update the connection string in `src/FeedbackAnalyzer.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=FeedbackAnalyzerDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True"
  }
}
```

2. Create/update the database:
```bash
cd src/FeedbackAnalyzer.API
dotnet ef database update
```

### Option C: Azure SQL Database (Production)

1. Create Azure SQL Database:
   - Go to Azure Portal
   - Create a new SQL Database
   - Note the connection string

2. Update `appsettings.json` with Azure SQL connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=tcp:YOUR_SERVER.database.windows.net,1433;Initial Catalog=FeedbackAnalyzerDb;Persist Security Info=False;User ID=YOUR_USER;Password=YOUR_PASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
  }
}
```

3. Run migrations:
```bash
cd src/FeedbackAnalyzer.API
dotnet ef database update
```

## Step 4: Configure CORS (if needed)

If your Web application runs on a different port or domain, update CORS settings in `src/FeedbackAnalyzer.API/appsettings.json`:

```json
{
  "CorsOrigins": "https://localhost:5001,http://localhost:5001"
}
```

## Step 5: Build the Solution

```bash
dotnet build
```

## Step 6: Run the Application

### Method 1: Run with dotnet CLI (Two Terminals)

**Terminal 1 - API:**
```bash
cd src/FeedbackAnalyzer.API
dotnet run
```
The API will start on https://localhost:7000

**Terminal 2 - Web App:**
```bash
cd src/FeedbackAnalyzer.Web
dotnet run
```
The Web app will start on https://localhost:5001

### Method 2: Using Visual Studio

1. Open `FeedbackAnalyzer.sln` in Visual Studio
2. Right-click on the Solution in Solution Explorer
3. Select "Configure Startup Projects"
4. Choose "Multiple startup projects"
5. Set both `FeedbackAnalyzer.API` and `FeedbackAnalyzer.Web` to "Start"
6. Click OK
7. Press F5 to run

### Method 3: Using VS Code

1. Open the project folder in VS Code
2. Use the integrated terminal to run both projects as shown in Method 1

## Step 7: Access the Application

1. Open your browser and navigate to: `https://localhost:5001`
2. You should see the Feedback Analyzer home page
3. Click "Submit Feedback" to submit your first feedback
4. Open "Dashboard" in another tab to see real-time updates

## Step 8: Test the Application

1. **Submit Feedback:**
   - Navigate to "Submit Feedback"
   - Enter some text (e.g., "This is an amazing product! I love it!")
   - Optionally add a category
   - Click "Submit Feedback"
   - You should see the sentiment analysis result

2. **View Dashboard:**
   - Navigate to "Dashboard"
   - You should see the sentiment trends and recent feedback
   - Keep this page open

3. **Test Real-time Updates:**
   - In a new tab, go to "Submit Feedback"
   - Submit new feedback
   - Watch the dashboard update in real-time!

## Troubleshooting

### Issue: Azure Cognitive Services Error

**Problem:** "Azure Cognitive Services endpoint not configured" or 401 Unauthorized

**Solution:**
- Verify your endpoint URL and API key are correct
- Ensure the key is from "Keys and Endpoint" section, not "Access Keys"
- Check that your Azure subscription is active

### Issue: Database Connection Error

**Problem:** Cannot connect to SQL Server

**Solution:**
- For LocalDB: Ensure LocalDB is installed and running
- For SQL Server: Verify the connection string is correct
- For Azure SQL: Ensure firewall rules allow your IP address

### Issue: CORS Error

**Problem:** Browser console shows CORS errors

**Solution:**
- Verify the Web app URL is in the `CorsOrigins` setting in the API's appsettings.json
- Restart the API after changing CORS settings

### Issue: SignalR Connection Failed

**Problem:** Dashboard shows "Connecting..." but never connects

**Solution:**
- Ensure the API is running
- Check the `HubUrl` in Web app's appsettings.json matches the API URL
- Verify no firewall is blocking WebSocket connections

### Issue: Port Already in Use

**Problem:** "Address already in use" error

**Solution:**
- Change the port in `Properties/launchSettings.json` for the affected project
- Update corresponding URLs in appsettings.json files

## Environment Variables (Alternative to appsettings.json)

For production or CI/CD, you can use environment variables:

**API:**
```bash
export ConnectionStrings__DefaultConnection="YOUR_CONNECTION_STRING"
export AzureCognitiveServices__Endpoint="YOUR_ENDPOINT"
export AzureCognitiveServices__ApiKey="YOUR_API_KEY"
```

**Web:**
```bash
export ApiSettings__BaseUrl="https://your-api-url.com"
export ApiSettings__HubUrl="https://your-api-url.com/feedbackhub"
```

## Next Steps

- Deploy to Azure App Service
- Set up CI/CD with GitHub Actions
- Configure Application Insights for monitoring
- Implement authentication and authorization
- Add more sentiment analysis features

## Support

For issues or questions, please open an issue in the GitHub repository.
