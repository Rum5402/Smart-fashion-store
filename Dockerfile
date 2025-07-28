# Use the official .NET 8.0 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official .NET 8.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution file and project files
COPY Fashion.sln ./
COPY Fashion.Api/Fashion.Api.csproj ./Fashion.Api/
COPY Fashion.Core/Fashion.Core.csproj ./Fashion.Core/
COPY Fashion.Contract/Fashion.Contract.csproj ./Fashion.Contract/
COPY Fashion.Infrastructure/Fashion.Infrastructure.csproj ./Fashion.Infrastructure/
COPY Fashion.Service/Fashion.Service.csproj ./Fashion.Service/

# Restore dependencies
RUN dotnet restore Fashion.sln

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build Fashion.sln -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish Fashion.Api/Fashion.Api.csproj -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fashion.Api.dll"] 