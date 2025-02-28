FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

RUN apt-get update && apt-get install -y libncursesw6

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/Project3_1/Project3_1.csproj", "Project3_1/"]
COPY ["src/BookOfHours/BookOfHours.csproj", "BookOfHours/"]

RUN dotnet restore "Project3_1/Project3_1.csproj"

COPY src/ .  # Копируем весь исходный код проекта
WORKDIR "/src/Project3_1"

RUN dotnet build "Project3_1.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Project3_1.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish ./
COPY src/data /app/data  # Копируем папку data с учетом новой структуры

ENTRYPOINT ["dotnet", "Project3_1.dll"]