# Password Manager server

## Prerequisites
- [.NET 6.0](https://dotnet.microsoft.com/en-us/download)
- [Docker](https://docs.docker.com/get-docker/)
- [Entity Framework tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

## Local development
**Run Azure SQL Edge on docker**
```shell
docker run -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=secret_password' -p 1433:1433 --name password-manager-db -d mcr.microsoft.com/azure-sql-edge 
```

**Run web application**
```shell
dotnet restore
dotnet build
dotnet run
```

## Database migrations
**List migrations**
```shell
dotnet ef migrations list --startup-project WebApp --project Db
```

**Create new migration**
```shell
dotnet ef migrations add Y --startup-project WebApp --project Db -o Migrations
```

**Generate SQL script from migration X to migration Y**
```shell
dotnet ef migrations script X Y -i --startup-project WebApp --project Db -o DbUp/Scripts
```

**Run script that does migration and generates SQL script.**
```shell
./migrate migration_name
```