# Stage 1: Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Informa ao Kestrel para escutar em HTTP:80 e HTTPS:443
ENV ASPNETCORE_URLS="http://+:80;https://+:443"

COPY certs/mycert.pfx /app/mycert.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/mycert.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=Su@SenhaAqui
ENV ASPNETCORE_URLS="http://+:80;https://+:443"
# Exponha ambas as portas
EXPOSE 80
EXPOSE 443

# Stage 2: Build + publish
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
WORKDIR /src/NFTudio.Api

RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Stage 3: Runtime image
FROM base AS final
WORKDIR /app

# Copia os artefatos publicados
COPY --from=build /app/publish ./

# Seed de dados
COPY NFTudio.Api/Data/seed.sql ./Data/seed.sql



ENTRYPOINT ["dotnet", "NFTudio.Api.dll"]