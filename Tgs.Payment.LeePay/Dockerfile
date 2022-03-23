#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BuildConfiguration=Release
RUN --mount=type=secret,id=nuget,dst=/pat cp /pat ~/.nuget/NuGet/NuGet.Config
WORKDIR /src
COPY ["NuGet.config", "."]
COPY ["Tgs.Payment.LeePay/Tgs.Payment.LeePay.csproj", "Tgs.Payment.LeePay/"]
COPY ["LeePay.Core/LeePay.Core.csproj", "LeePay.Core/"]
COPY ["Tgs.Payment.LeePay.Client/Tgs.Payment.LeePay.Client.csproj", "Tgs.Payment.LeePay.Client/"]
RUN dotnet restore "Tgs.Payment.LeePay/Tgs.Payment.LeePay.csproj"
COPY . .
WORKDIR "/src/Tgs.Payment.LeePay"
RUN dotnet build "Tgs.Payment.LeePay.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.Payment.LeePay.csproj" -c $BuildConfiguration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.Payment.LeePay.dll"]
