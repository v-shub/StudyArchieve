FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 80
ENV ASPNETCORE_URLS http://+:80
ENV ASPNETCORE_ENVIRONMENT Development
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["BusinessLogic/BusinessLogic.csproj", "BusinessLogic/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["StudyArchieve/StudyArchieveApi.csproj", "StudyArchieve/"]

RUN dotnet restore "StudyArchieve/StudyArchieveApi.csproj"

COPY . .

FROM build AS publish
RUN dotnet publish "StudyArchieve/StudyArchieveApi.csproj" -c Release -o /app/publish -p:UseAppHost=false  # ← ИСПРАВЛЕНО

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StudyArchieveApi.dll"]