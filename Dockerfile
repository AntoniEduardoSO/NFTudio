# Stage 1: Runtime base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Faz o Kestrel escutar apenas em HTTP na porta 10000
ENV ASPNETCORE_URLS="http://+:10000"
EXPOSE 10000

# Stage 2: Build e publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
WORKDIR /src/NFTudio.Api
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Stage 3: Imagem final
FROM base AS final
WORKDIR /app

# Copia artefatos publicados
COPY --from=build /app/publish ./

# Copia o seed.sql (para quando você rodar SeedSql())
COPY NFTudio.Api/Data/seed.sql ./Data/seed.sql

ENTRYPOINT ["dotnet", "NFTudio.Api.dll"]
