#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["pagamento/QuickOrderPagamento.Adapters.Driving.Api.csproj", "pagamento/"]
COPY ["QuickOrderPagamento.Core.IoC/QuickOrderPagamento.Core.IoC.csproj", "QuickOrderPagamento.Core.IoC/"]
COPY ["QuickOrderPagamento.Adapters.Driven.MongoDB/QuickOrderPagamento.Adapters.Driven.MongoDB.csproj", "QuickOrderPagamento.Adapters.Driven.MongoDB/"]
COPY ["QuickOrderPagamento.Core.Domain/QuickOrderPagamento.Core.Domain.csproj", "QuickOrderPagamento.Core.Domain/"]
COPY ["QuickOrderPagamento.Core.Application/QuickOrderPagamento.Core.Application.csproj", "QuickOrderPagamento.Core.Application/"]
COPY ["QuickOrderPagamento.Adapters.Driven.MercadoPago/QuickOrderPagamento.Adapters.Driven.MercadoPago.csproj", "QuickOrderPagamento.Adapters.Driven.MercadoPago/"]
RUN dotnet restore "./pagamento/./QuickOrderPagamento.Adapters.Driving.Api.csproj"
COPY . .
WORKDIR "/src/pagamento"
RUN dotnet build "./QuickOrderPagamento.Adapters.Driving.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QuickOrderPagamento.Adapters.Driving.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuickOrderPagamento.Adapters.Driving.Api.dll"]