#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim-arm32v7 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["CarService/CarService.csproj", "CarService/"]
RUN dotnet restore "CarService/CarService.csproj" -r linux-arm
COPY . .
WORKDIR "/src/CarService"
RUN dotnet build "CarService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarService.csproj" -c Release -o /app/publish -r linux-arm --self-contained false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarService.dll"]