# Star Wars Character Explorer

## Project Overview

This is a .NET 8 microservice solution for exploring Star Wars characters, featuring:
- Web API with Swagger documentation
- Console Client for interaction
- PostgreSQL database integration
- Entity Framework Core migrations
- Docker containerization

## Project Structure

- `StarWars.WebApi`: Main API service
- `StarWars.DAL`: Data Access Layer
- `StarWars.ConsoleClient`: Interactive console application

## Prerequisites

- .NET 8 SDK
- Docker
- Docker Compose

## Setup and Installation

### 1. Install EF Core Tools
```bash
dotnet tool install --global dotnet-ef
```

### 2. Create Initial Database Migration
```bash dotnet ef migrations add Initial_Migrations  --project ./StarWars.DAL  --startup-project ./StarWars.WebApi```

### 3. Build and Run with Docker
```bash
# Build Docker images
docker-compose build

# Start services
docker-compose up -d

# Attach to console client
docker attach starwars-consoleclient
```

## Features

- List Star Wars characters
- Search characters
- Add/remove favorite characters
- View request history
- Swagger API documentation

## Technologies

- .NET 8
- Entity Framework Core
- PostgreSQL
- Docker
- SWAPI (Star Wars API)

## Endpoints

- Swagger UI: `http://localhost:8080/swagger`
- Web API Base URL: `http://localhost:8080/api/v1`

## Development Notes

- Uses in-memory caching
- Implements request logging middleware
- Supports environment-based configuration


## Unit Testing & Code Coverage

I could not implement the unit tests yet due to limited time. So "0" code coverage for now. There may be minor bugs due to this.

## Future Improvements (WORKING ON IT..)
- Generic Exception Hnadling for Web API
- Add Unit Tests, bug fixes, code coverage, 
- add mutation scores (reliability score of the tests)
- add In-Memory Intgration Testing (this one maybe later, just a fantasy)