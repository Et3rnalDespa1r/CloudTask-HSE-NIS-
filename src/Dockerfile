FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Обновляем пакеты и устанавливаем libncursesw (возможно, достаточно libncursesw6)
RUN apt-get update && apt-get install -y libncursesw6

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Project3_1/Project3_1.csproj", "Project3_1/"]
COPY ["BookOfHours/BookOfHours.csproj", "BookOfHours/"]
RUN dotnet restore "Project3_1/Project3_1.csproj"
COPY . .
WORKDIR "/src/Project3_1"
RUN dotnet build "Project3_1.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Project3_1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
COPY data /app/data
ENTRYPOINT ["dotnet", "Project3_1.dll"]