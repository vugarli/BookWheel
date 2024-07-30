FROM minio/minio AS minioconfigurer
COPY ./miniopolicies/ /
COPY ./miniopolicies/minioconfig.sh /

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore "ImageProcessor.csproj"
RUN dotnet build "ImageProcessor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImageProcessor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImageProcessor.dll"]