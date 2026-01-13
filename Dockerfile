# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy Solution and Project files
COPY BillTrackPro.sln ./
COPY BillTrackPro.API/BillTrackPro.API.csproj BillTrackPro.API/
COPY BillTrackPro.Application/BillTrackPro.Application.csproj BillTrackPro.Application/
COPY BillTrackPro.Domain/BillTrackPro.Domain.csproj BillTrackPro.Domain/
COPY BillTrackPro.Infrastructure/BillTrackPro.Infrastructure.csproj BillTrackPro.Infrastructure/

# Restore dependencies
RUN dotnet restore BillTrackPro.API/BillTrackPro.API.csproj

# Copy everything else
COPY . .

# Build and Publish
WORKDIR /src/BillTrackPro.API
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "BillTrackPro.API.dll"]
