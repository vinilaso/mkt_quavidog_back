# Etapa de runtime (Base)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Etapa de compilação (SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copiar projetos rigorosamente para otimização de cache de camada
COPY ["Sienna.WebApi/Sienna.WebApi.csproj", "Sienna.WebApi/"]
COPY ["Sienna.Application/Sienna.Application.csproj", "Sienna.Application/"]
COPY ["Sienna.Domain/Sienna.Domain.csproj", "Sienna.Domain/"]
COPY ["Sienna.Infrastructure/Sienna.Infrastructure.csproj", "Sienna.Infrastructure/"]
COPY ["Sienna.Shared/Sienna.Shared.csproj", "Sienna.Shared/"]

# Restaurar dependências a partir do entrypoint
RUN dotnet restore "Sienna.WebApi/Sienna.WebApi.csproj"

# Copiar o restante da base de código
COPY . .

# Compilar
WORKDIR "/src/Sienna.WebApi"
RUN dotnet build "Sienna.WebApi.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "Sienna.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final (Produção)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Sienna.WebApi.dll"]