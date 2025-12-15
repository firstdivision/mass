# Mass

Multi Author Story System

## Prerequisites

Before you begin, ensure you have the following tools installed:

- **.NET Core SDK** - [Download](https://dotnet.microsoft.com/download)
- **dotnet CLI** - Included with .NET Core SDK
- **Entity Framework Tools** - Install globally via dotnet CLI
- **DBeaver** - [Download](https://dbeaver.io/download/)

## Installation

### 1. Install .NET Core SDK

Download and install from the [official .NET website](https://dotnet.microsoft.com/download). Verify installation:

```bash
dotnet --version
```

### 2. Install Entity Framework Core Tools

```bash
dotnet tool install --global dotnet-ef
```

Update if already installed:

```bash
dotnet tool update --global dotnet-ef
```

### 3. Install DBeaver (Optional)

Download the Community Edition from [dbeaver.io](https://dbeaver.io/download/) and follow the installation instructions for your OS.

## Development Setup

Clone the repository and restore dependencies:

```bash
git clone <repository-url>
cd mass
dotnet restore
```

## Building

```bash
dotnet build
```

## Running

```bash
dotnet run
```

## Database Migrations

Create a new migration:

```bash
dotnet ef migrations add MigrationName
```

Update the database:

```bash
dotnet ef database update
```

## License

[Add your license information here]

## Docker

Build a small, production-ready image using the multi-stage Dockerfile included in the repository. The Dockerfile publishes a framework-dependent build and uses the official `mcr.microsoft.com/dotnet/aspnet:10.0` runtime image which includes the native ICU package required for globalization.

Build the image:

```bash
docker build -t mass:latest .
```

Run the container (maps container port 80 to host port 5000). Pass your database connection string as the `DB_CONNECTION` environment variable:

```bash
# Inline env var
docker run --rm -p 5000:80 \
  -e 'DB_CONNECTION=Host=db-host;Database=massdb;Username=mass;Password=secret' \
  mass:latest

# Or use an env file (recommended for secrets):
# .env contents:
# DB_CONNECTION='Host=db-host;Database=massdb;Username=mass;Password=secret'

docker run --rm --env-file .env -p 5000:80 mass:latest
```

If you want to use a distroless base, ensure `libicu` is present in the final image (or use a self-contained publish with the necessary native libraries copied into the image).