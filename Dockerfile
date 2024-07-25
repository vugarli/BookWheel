FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore "BookWheel.sln"
WORKDIR "/app/src/BookWheel.Api"
RUN dotnet build "BookWheel.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookWheel.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookWheel.Api.dll"]