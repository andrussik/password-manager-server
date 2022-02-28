FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DbUp/DbUp.csproj", "DbUp/"]
RUN dotnet restore "DbUp/DbUp.csproj"
COPY . .
WORKDIR "/src/DbUp"
RUN dotnet build "DbUp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DbUp.csproj" -c Release -o /app/publish

FROM base AS final
ARG PASSWORD_MANAGER_DB
WORKDIR /app
COPY --from=publish /app/publish .
RUN dotnet "DbUp.dll" $PASSWORD_MANAGER_DB