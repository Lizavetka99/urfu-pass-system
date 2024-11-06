FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["UrfuPassSystem.App/UrfuPassSystem.App.csproj", "UrfuPassSystem.App/"]

RUN dotnet restore "UrfuPassSystem.App/UrfuPassSystem.App.csproj"
COPY . .
WORKDIR "/src/UrfuPassSystem.App"
RUN dotnet build "UrfuPassSystem.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UrfuPassSystem.App.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "UrfuPassSystem.App.dll"]
