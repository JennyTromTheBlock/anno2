# Byg fra en .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Eksponer porten som din app kører på (f.eks. 80)
EXPOSE 8000

# Start applikationen
ENTRYPOINT ["dotnet", "Presentation.dll"]
