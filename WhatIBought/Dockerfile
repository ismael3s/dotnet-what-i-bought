#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
#TODO: this is the best way of doing this? I think not
COPY ./WhatIBought/WhatIBoughtAPI.csproj WhatIBoughtAPI.csproj
COPY ./Infra.IoC/Infra.IoC.csproj ../Infra.IoC/Infra.IoC.csproj
COPY ./Application/Application.csproj ../Application/Application.csproj
COPY ./Infra/Infra.csproj ../Infra/Infra.csproj
RUN dotnet restore "./WhatIBoughtAPI.csproj"
COPY ./Infra.IoC ../Infra.IoC
COPY ./Application ../Application
COPY ./Infra ../Infra
COPY ./WhatIBought .
WORKDIR "/src/."
RUN dotnet build "WhatIBoughtAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WhatIBoughtAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WhatIBoughtAPI.dll"]