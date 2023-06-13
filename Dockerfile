#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://*:80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FoodUsers.Infrastructure.API/FoodUsers.Infrastructure.API.csproj", "FoodUsers.Infrastructure.API/"]
COPY ["FoodUsers.Application/FoodUsers.Application.csproj", "FoodUsers.Application/"]
COPY ["FoodUsers.Domain/FoodUsers.Domain.csproj", "FoodUsers.Domain/"]
COPY ["FoodUsers.Infrastructure.Data/FoodUsers.Infrastructure.Data.csproj", "FoodUsers.Infrastructure.Data/"]
COPY ["FoodUsers.Infrastructure.Security/FoodUsers.Infrastructure.Security.csproj", "FoodUsers.Infrastructure.Security/"]
RUN dotnet restore "FoodUsers.Infrastructure.API/FoodUsers.Infrastructure.API.csproj"
COPY . .
WORKDIR "/src/FoodUsers.Infrastructure.API"
RUN dotnet build "FoodUsers.Infrastructure.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FoodUsers.Infrastructure.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodUsers.Infrastructure.API.dll"]