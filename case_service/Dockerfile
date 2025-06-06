# Byg fra en .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Kopier csproj og genskab nuget-dependencies
COPY *.sln .
COPY Application/*.csproj ./Application/
COPY Infrastructure/*.csproj ./Infrastructure/
COPY Presentation/*.csproj ./Presentation/

RUN dotnet restore

# Kopier resten og byg
COPY . .
WORKDIR /app/Presentation
RUN dotnet publish -c Release -o /app/publish

# Kør fra en runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8000

EXPOSE 8000

ENTRYPOINT ["dotnet", "Presentation.dll"]
