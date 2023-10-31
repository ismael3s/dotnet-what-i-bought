#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src/api
COPY ./src/Presentation.API/Presentation.API.csproj .
COPY ./src/Infra.IoC/Infra.IoC.csproj ../Infra.IoC/
COPY ./src/Application/Application.csproj ../Application/
COPY ./src/Infra/Infra.csproj ../Infra/
RUN dotnet restore "./Presentation.API.csproj"
COPY ./src/Infra.IoC ../Infra.IoC
COPY ./src/Application ../Application
COPY ./src/Infra ../Infra
COPY ./src/Presentation.API .
WORKDIR "/src/api"
RUN dotnet build "Presentation.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Presentation.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.API.dll"]