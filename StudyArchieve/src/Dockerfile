FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

EXPOSE 80

ENV ASPNETCORE_URLS http://+:80
ENV ASPNETCORE_ENVIRONMENT Development

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["BusinessLogic/BusinessLogic.csproj", "BusinessLogic/"]
COPY ["StudyAchieve/StudyAchieveApi.csproj", "StudyAchieve/"]
RUN dotnet restore "StudyAchieve/StudyAchieveApi.csproj"

COPY . .
FROM build AS publish
RUN dotnet publish "StudyAchieve/StudyAchieveApi.csproj" -c Release -o /app/publish -p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudyAchieveApi.dll"]