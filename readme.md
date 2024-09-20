# EShop

Demo .NET Core eshop application with versioned API.

## Infrastructure

In order to run the application, change OVERLAY_NAME in .env file to existing, or create new Docker overlay network using

```
docker network create DevOver
```

## Configuration

Feel free to change selected configurable values in EShop.API/appsettings.yml file.

## Run

Open solution in VS, set docker-compose as startup project and run the application.
Open in browser https://localhost:9002/swagger/index.html to interact with the API.

The database will be created automatically and will be seeded with demo data.

Note: In order to run integration tests, the project needs to be run using docker-compose first, the sql-database.local container must be running.
