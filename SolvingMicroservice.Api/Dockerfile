FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_URLS = http://+:2000/

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SolvingMicroservice.Api/SolvingMicroservice.Api.csproj", "SolvingMicroservice.Api/"]
COPY ["HeadCode.Core/HeadCode.Core.csproj", "HeadCode.Core/"]
RUN dotnet restore "SolvingMicroservice.Api/SolvingMicroservice.Api.csproj"
COPY . .
WORKDIR "/src/SolvingMicroservice.Api"
RUN dotnet build "SolvingMicroservice.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SolvingMicroservice.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SolvingMicroservice.Api.dll"]
