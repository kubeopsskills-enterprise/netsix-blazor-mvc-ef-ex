FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ARG Configuration=Release

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
ARG Configuration=Release
COPY KubeExSite.sln ./
COPY ["KubeExSite/KubeExSite.csproj", "KubeExSite/"]
RUN dotnet restore "KubeExSite/KubeExSite.csproj" --no-cache
COPY . .
WORKDIR /src/KubeExSite/
RUN dotnet build "KubeExSite.csproj" -c $Configuration -o /app/build

FROM build AS publish
ARG Configuration=Release
WORKDIR /src/KubeExSite/
RUN dotnet publish "KubeExSite.csproj" -c $Configuration -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KubeExSite.dll"]
