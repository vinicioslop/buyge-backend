FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj e restaura as camadas
COPY *.csproj ./
RUN dotnet restore

# Copia tudo e builda
COPY . ./
RUN dotnet publish -c Release -o out

# Build com a imagem do runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

# Usa porta dinâmica do Heroku
CMD ASPNETCORE_URLS="http://*:$PORT" dotnet buyge-backend.dll