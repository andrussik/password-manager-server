ARG PASSWORD_MANAGER_DB

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS run
WORKDIR /src
COPY ["DbUp/DbUp.csproj", "DbUp/"]
RUN dotnet restore "DbUp/DbUp.csproj"
COPY . .
WORKDIR "/src/DbUp"
RUN dotnet build "DbUp.csproj"
RUN dotnet run "DbUp.csproj" $PASSWORD_MANAGER_DB
