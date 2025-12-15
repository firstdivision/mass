# Use the SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files first for caching
COPY *.sln ./
COPY *.csproj ./
RUN dotnet restore "mass.sln"

# Copy the rest and publish a self-contained single-file app for linux-x64
COPY . ./
RUN dotnet publish "mass.sln" -c Release -r linux-x64 --self-contained true \
    -p:PublishSingleFile=true -p:PublishTrimmed=true -p:IncludeNativeLibrariesForSelfExtract=true \
    -o /app/publish

# Final image: small distroless base with C runtime
FROM gcr.io/distroless/cc:nonroot
WORKDIR /app
COPY --from=build /app/publish/ ./
USER nonroot

# For a self-contained single-file publish the executable will be named 'mass'
ENTRYPOINT ["/app/mass"]
