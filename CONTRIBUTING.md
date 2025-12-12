# Contributing to Feedback Analyzer

Thank you for your interest in contributing to Feedback Analyzer! This document provides guidelines and instructions for contributing to the project.

## Code of Conduct

Please be respectful and constructive in all interactions with the project and its community.

## Getting Started

1. **Fork the repository** on GitHub
2. **Clone your fork** locally:
   ```bash
   git clone https://github.com/YOUR-USERNAME/feebackAnalyzerDotnet10.git
   cd feebackAnalyzerDotnet10
   ```
3. **Add upstream remote**:
   ```bash
   git remote add upstream https://github.com/rijwanansari/feebackAnalyzerDotnet10.git
   ```

## Development Setup

Follow the [SETUP.md](SETUP.md) guide to set up your development environment.

## Development Workflow

### 1. Create a Branch

Create a new branch for your feature or bug fix:

```bash
git checkout -b feature/your-feature-name
# or
git checkout -b fix/issue-description
```

Branch naming conventions:
- `feature/` - New features
- `fix/` - Bug fixes
- `docs/` - Documentation updates
- `refactor/` - Code refactoring
- `test/` - Adding or updating tests

### 2. Make Your Changes

- Follow the existing code style and conventions
- Write clear, self-documenting code
- Add comments for complex logic
- Update documentation if needed

### 3. Test Your Changes

```bash
# Build the solution
dotnet build

# Run tests (if available)
dotnet test

# Test manually by running both projects
```

### 4. Commit Your Changes

Write clear, descriptive commit messages:

```bash
git add .
git commit -m "Add feature: brief description

Detailed explanation of what was changed and why."
```

Commit message guidelines:
- Use present tense ("Add feature" not "Added feature")
- Keep the first line under 50 characters
- Add detailed description after a blank line if needed
- Reference issue numbers when applicable

### 5. Push to Your Fork

```bash
git push origin feature/your-feature-name
```

### 6. Create a Pull Request

1. Go to the original repository on GitHub
2. Click "New Pull Request"
3. Select your fork and branch
4. Provide a clear title and description
5. Reference any related issues

## Code Style Guidelines

### C# Code Style

Follow standard C# coding conventions:

- Use PascalCase for class names, method names, and properties
- Use camelCase for local variables and parameters
- Use meaningful, descriptive names
- Keep methods focused and small
- Use async/await for asynchronous operations

Example:
```csharp
public class FeedbackService : IFeedbackService
{
    private readonly FeedbackDbContext _context;
    
    public async Task<Feedback> CreateFeedbackAsync(CreateFeedbackRequest request)
    {
        // Implementation
    }
}
```

### Blazor Components

- Use descriptive component names
- Keep components focused on a single responsibility
- Use code-behind files for complex logic
- Follow Blazor naming conventions

Example:
```razor
@page "/submit-feedback"
@using FeedbackAnalyzer.Shared.Models

<h1>Submit Feedback</h1>

@code {
    private CreateFeedbackRequest request = new();
    
    private async Task HandleSubmit()
    {
        // Implementation
    }
}
```

## Project Structure

```
FeedbackAnalyzer/
├── src/
│   ├── FeedbackAnalyzer.API/       # Web API project
│   ├── FeedbackAnalyzer.Web/       # Blazor web project
│   └── FeedbackAnalyzer.Shared/    # Shared models
├── tests/                           # Test projects (future)
├── docs/                           # Additional documentation (future)
├── .github/                        # GitHub workflows
├── README.md
├── SETUP.md
├── ARCHITECTURE.md
└── CONTRIBUTING.md
```

## Adding New Features

When adding new features:

1. **Models**: Add to `FeedbackAnalyzer.Shared/Models/`
2. **API Controllers**: Add to `FeedbackAnalyzer.API/Controllers/`
3. **Services**: Add to `FeedbackAnalyzer.API/Services/`
4. **Blazor Pages**: Add to `FeedbackAnalyzer.Web/Components/Pages/`
5. **Database Changes**: Create new EF migration

Example - Adding a new feature:

```bash
# Add new model
cd src/FeedbackAnalyzer.Shared/Models
# Create YourModel.cs

# Add service
cd src/FeedbackAnalyzer.API/Services
# Create IYourService.cs and YourService.cs

# Add controller
cd src/FeedbackAnalyzer.API/Controllers
# Create YourController.cs

# Create migration if needed
cd src/FeedbackAnalyzer.API
dotnet ef migrations add YourMigrationName
```

## Database Migrations

When making database changes:

1. Update the model in `FeedbackAnalyzer.Shared/Models/`
2. Update the DbContext configuration if needed
3. Create a migration:
   ```bash
   cd src/FeedbackAnalyzer.API
   dotnet ef migrations add DescriptiveMigrationName
   ```
4. Review the generated migration
5. Test the migration locally:
   ```bash
   dotnet ef database update
   ```

## Testing

Currently, the project doesn't have automated tests. When adding tests:

1. Create a test project in the `tests/` directory
2. Use xUnit as the testing framework
3. Follow the naming convention: `ProjectName.Tests`
4. Organize tests to mirror the source structure

Example test structure:
```
tests/
├── FeedbackAnalyzer.API.Tests/
│   ├── Controllers/
│   │   └── FeedbackControllerTests.cs
│   └── Services/
│       └── FeedbackServiceTests.cs
```

## Documentation

When updating documentation:

- Keep it clear and concise
- Use proper markdown formatting
- Include code examples where helpful
- Update all relevant docs when making changes

Documentation files:
- `README.md` - Project overview
- `SETUP.md` - Setup instructions
- `ARCHITECTURE.md` - Architecture details
- `CONFIGURATION.md` - Configuration guide
- `CONTRIBUTING.md` - This file

## Pull Request Guidelines

Good pull requests:

1. **Focus on a single concern**
   - One feature or bug fix per PR
   - Avoid mixing unrelated changes

2. **Include tests** (when test infrastructure exists)
   - Add tests for new features
   - Update tests for changed features

3. **Update documentation**
   - Update README if needed
   - Add comments for complex code
   - Update architecture docs for significant changes

4. **Follow code style**
   - Match existing code style
   - Use meaningful names
   - Keep methods small and focused

5. **Write a good description**
   - Explain what and why
   - Reference related issues
   - Include screenshots for UI changes

## Review Process

1. Automated checks will run on your PR
2. Maintainers will review your code
3. Address any feedback or requested changes
4. Once approved, your PR will be merged

## Reporting Issues

When reporting bugs:

1. Check if the issue already exists
2. Use the issue template (if available)
3. Provide a clear title and description
4. Include steps to reproduce
5. Add relevant logs or screenshots
6. Specify your environment (.NET version, OS, etc.)

## Feature Requests

When requesting features:

1. Check if it's already requested
2. Clearly describe the feature
3. Explain the use case
4. Provide examples if helpful

## Questions?

If you have questions:

1. Check existing documentation
2. Search closed issues
3. Open a new issue with the "question" label

## License

By contributing, you agree that your contributions will be licensed under the same license as the project (see LICENSE file).

## Recognition

Contributors will be recognized in the project's README and release notes.

Thank you for contributing to Feedback Analyzer! 🎉
