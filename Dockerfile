# Use the SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first for caching
COPY *.sln ./
COPY UI/mass.csproj UI/
RUN dotnet restore "UI/mass.csproj"

# Copy the rest and publish a framework-dependent app
COPY . ./
RUN dotnet publish "UI/mass.csproj" -c Release -o /app/publish

# Final image: use official ASP.NET runtime (includes libicu)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish/ ./

# Run as non-root (image provides 'app' user)
USER app

ENTRYPOINT ["dotnet", "mass.dll"]
