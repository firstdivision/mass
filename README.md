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

Build a small, production-ready image using the multi-stage Dockerfile included in the repository. This builds a self-contained Linux-x64 publish and copies it into a distroless runtime image.

Build the image:

```bash
docker build -t mass:latest .
```

Run the container:

```bash
docker run --rm -p 5000:80 mass:latest
```
