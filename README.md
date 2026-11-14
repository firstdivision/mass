# Mass

Multi Author Story System

## Phantom GCP Charges

For a long time I was wondering why I was getting charged more than I thought I should be for Cloud Run which should scale to zero when no traffic comes in for a while.

I finally found how to look at requests by going to GCP Logs Explorer and found the culprit.  

Various scanners were hitting the IP address of the Global Load Balancer and making the Cloud Run App load the home page, thereby keeping it alive and running in GCP.

The solution was to:
- Create a new Cloud Storage Bucket called something like `lb-deadend-mass`
- Upload a 404 page to it
- Go to the Bucket Hamburger and select Website settings, and set the 404.html page as home and 404 page.
- Add that bucket as a backend bucket to the load balancer
- Use that bucket for any unmatched requests, and
- Add a second host filter to the load balancer that would send matching traffic to the Cloud Run Service.

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

### Environment variables

Create a `.env` file in the repository root (the folder that contains the solution file) before running the app locally. The app reads these values from the environment, and the local watch/make scripts load them from this file.

```dotenv
# Place this in .env at the repository root, for example: /mass/.env
DB_CONNECTION="Host=localhost;Port=5432;Database=mass;Username=guest;Password=guest"
GCP_CLIENT_ID="your-google-client-id"
GCP_CLIENT_SECRET="your-google-client-secret"
```

- `DB_CONNECTION` should point to your local PostgreSQL instance.
- `GCP_CLIENT_ID` and `GCP_CLIENT_SECRET` are required for Google authentication.
- If you run the UI Makefile from the UI folder, keep the same values in `UI/.env` as well.

## Building

```bash
dotnet build
```

## Running

```bash
dotnet run
```

## Expose WSL Port to Local Network

From an elevated Powershell prompt, run:

`powershell.exe -ExecutionPolicy Bypass -File "\\wsl$\Debian\home\<your user>\projects\github\mass\wsl-portproxy.ps1"`

You may also have to add a Windows Defender Firewall exception for the TCP port 7213

Now you can access the app from devices on your local network by going to `https://<windows ip>:7213`

## Database Migrations

To start a local PostgreSQL Docker container and apply all migrations, run from
the repository root:

```bash
make db-up
```

Stop the container without removing its data:

```bash
make db-down
```

This requires Docker and the EF Core tools. The container persists across runs
and defaults to database `mass`, user `guest`, password `guest`, and port `5432`
on localhost. Set `PG_DATABASE`, `PG_USER`, `PG_PASSWORD`, or `PG_PORT` when
invoking make to change these defaults before creating the container. PostgreSQL
initialization settings do not change on an existing container; use its original
settings or set `PG_CONTAINER_NAME` to create a different container. Values must
not contain semicolons, which are connection-string separators. Set
`DB_CONNECTION` in the root `.env` to match these settings when running the app;
the migration command constructs its own local connection and does not use
`DB_CONNECTION` from `.env`. Use these defaults for local development only.

To also restore the local user and Google login, run:

```bash
make db-iup
```

This runs migrations first, then applies `local-user-seed.sql` from the
repository root. That file contains personal identity data and is git-ignored;
keep a private backup if the database or working directory is removed. It is
specific to this local account, not a general-purpose seed. Existing rows are
not overwritten. Never commit or share the file.

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
docker run --network mass-network --rm -p 5000:80 \
  -e 'DB_CONNECTION=Host=localdb;Database=mass;Username=guest;Password=guest' \
  mass:latest

# Or use an env file (recommended for secrets):
# .env contents:
# DB_CONNECTION='Host=localdb;Database=mass;Username=guest;Password=guest'

docker run --rm --env-file .env -p 5000:80 mass:latest
```

If you want to use a distroless base, ensure `libicu` is present in the final image (or use a self-contained publish with the necessary native libraries copied into the image).