FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services/AuthService/AuthService.WebAPI/AuthService.WebAPI.csproj", "Services/AuthService/AuthService.WebAPI/"]
COPY ["Services/AuthService/AuthService.Application/AuthService.Application.csproj", "Services/AuthService/AuthService.Application/"]
COPY ["Services/AuthService/AuthService.Domain/AuthService.Domain.csproj", "Services/AuthService/AuthService.Domain/"]
COPY ["Services/AuthService/AuthService.Infrastructure/AuthService.Infrastructure.csproj", "Services/AuthService/AuthService.Infrastructure/"]
RUN dotnet restore "Services/AuthService/AuthService.WebAPI/AuthService.WebAPI.csproj"
COPY . .
WORKDIR "/src/Services/AuthService/AuthService.WebAPI"
RUN dotnet build "AuthService.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthService.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AuthService.WebAPI.dll"]
