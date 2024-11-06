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

WORKDIR /app-python

RUN apt-get update && \
    apt-get install -y python3 python3-pip && \
    rm -rf /var/lib/apt/lists/*

COPY FaceRecognizer .

RUN pip install -r requirements.txt --break-system-packages --root-user-action=ignore

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "UrfuPassSystem.App.dll"]
