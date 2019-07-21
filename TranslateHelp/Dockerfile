FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["TranslateHelp/TranslateHelp.csproj", "TranslateHelp/"]
RUN dotnet restore "TranslateHelp/TranslateHelp.csproj"
COPY . .
WORKDIR "/src/TranslateHelp"
RUN dotnet build "TranslateHelp.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "TranslateHelp.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "TranslateHelp.dll"]