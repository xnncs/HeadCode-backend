FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS = http://+:8080/

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HeadCode.Api/HeadCode.Api.csproj", "HeadCode.Api/"]
COPY ["HeadCode.Infrastructure/HeadCode.Infrastructure.csproj", "HeadCode.Infrastructure/"]
COPY ["HeadCode.Core/HeadCode.Core.csproj", "HeadCode.Core/"]
COPY ["HeadCode.DataAccess/HeadCode.DataAccess.csproj", "HeadCode.DataAccess/"]
RUN dotnet restore "HeadCode.Api/HeadCode.Api.csproj"
COPY . .
WORKDIR "/src/HeadCode.Api"
RUN dotnet build "HeadCode.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HeadCode.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HeadCode.Api.dll"]
