FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./BrokerR.Http/BrokerR.Http.csproj ./BrokerR.Http/
COPY ./BrokerR.Http.Server/BrokerR.Http.Server.csproj ./BrokerR.Http.Server/
RUN dotnet restore BrokerR.Http.Server

# Copy everything else and build
COPY ./BrokerR.Http ./BrokerR.Http
COPY ./BrokerR.Http.Server ./BrokerR.Http.Server
RUN dotnet publish BrokerR.Http.Server -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BrokerR.Http.Server.dll"]

