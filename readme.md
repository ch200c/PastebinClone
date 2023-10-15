# PastebinClone
Simplified Pastebin API

PastebinClone is an API which allows to perform operations on Alias and Paste entities. Alias is the parent entity and has a required one-to-one relationship with Paste entity. 

When creating a Paste, alias can be omitted and a random available alias will be assigned. This assumes there is always an available alias (see [TODO](#TODO)).

Database is created and database migrations are applied on startup. This is for demonstration purposes only and is not recommended to be used in production.

## Technologies
- .NET 7
- PostgreSQL 15.4

## Features
- Docker support with docker-compose
- Unit of work and repository patterns inspired by [eShopOnContainers](https://github.com/dotnet-architecture/eShopOnContainers)
- REST API
- EF Core for ORM
- JSON Patch request support from [ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-7.0)
- Swagger request example support from [Swashbuckle.AspNetCore.Filters](https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters)

## Database Migrations

### .NET CLI
`dotnet ef migrations add YourMigrationName --startup-project PastebinClone.Api/PastebinClone.Api.csproj --project PastebinClone.Api/PastebinClone.Api.csproj -o Infrastructure/Persistence/Migrations`

## TODO
- Add AliasGenerator
- Add transactions
- Add unit tests
