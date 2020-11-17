FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./BrokerR.Http/BrokerR.Http.csproj ./BrokerR.Http/
COPY ./BrokerR.Http.Server/BrokerR.Http.Server.csproj ./BrokerR.Http.Server/
RUN dotnet restore BrokerR.Http.Server

# Copy project files
COPY ./BrokerR.Http ./BrokerR.Http
COPY ./BrokerR.Http.Server ./BrokerR.Http.Server

# Build
RUN dotnet publish BrokerR.Http.Server -c Release -o out

# Delete project files, retaining only build output
RUN rm -rf ./BrokerR.Http
RUN rm -rf ./BrokerR.Http.Server

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "BrokerR.Http.Server.dll"]

