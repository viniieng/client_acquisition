# Client Acquisition

Monorepo para uma aplicação de gestão de clientes e pedidos com ASP.NET Core .NET 8 e Blazor WebAssembly.
## Arquitetura

A solução segue uma Clean Architecture simplificada, separada em:
- `backend/src/API` para endpoints HTTP, middleware, Swagger e composição da aplicação.
- `backend/src/Application` para DTOs, validação, serviços e orquestração dos casos de uso.
- `backend/src/Domain` para entidades e regras de negócio.
- `backend/src/Infrastructure` para EF Core, repositórios, Unit of Work e acesso PostgreSQL.
- `backend/tests` para testes unitários.
- `frontend/src` para o cliente Blazor WebAssembly.
## Stack

- ASP.NET Core Web API
- .NET 8
- Entity Framework Core
- PostgreSQL / Supabase
- FluentValidation
- AutoMapper
- Swagger / OpenAPI
- Blazor WebAssembly
- xUnit

## Como Executar Localmente

1. Instale o SDK do .NET 8.
2. Configure a connection string em `backend/src/API/appsettings.json` ou via variáveis de ambiente.
3. Restaure as dependências e execute o projeto da API.
4. Execute o projeto do frontend Blazor WebAssembly.
## Banco de Dados

O script inicial de schema está em `backend/database/scripts/001_initial_schema.sql`.
Para Supabase, copie a connection string para `SUPABASE_CONNECTION_STRING` no arquivo `.env`.

## Testes

Os testes unitários cobrem:
- Validação de CPF
- Validação de idade mínima
- Cálculo do total do pedido
- Regra de bloqueio após 24 horas

## Docker

- Dockerfile do backend: `backend/src/API/Dockerfile`
- Dockerfile do frontend: `frontend/src/Dockerfile`
- Compose: `docker-compose.yml`

## Deploy

- Backend: Render.
- Frontend: Render.
- Banco: Supabase PostgreSQL.

Monorepo for a customer and order management platform built with ASP.NET Core .NET 8 and Blazor WebAssembly.

## Architecture

The solution uses a simplified Clean Architecture split into:

- `backend/src/API` for HTTP endpoints, middleware, Swagger and startup composition.
- `backend/src/Application` for DTOs, validation, services and use-case orchestration.
- `backend/src/Domain` for entities and business rules.
- `backend/src/Infrastructure` for EF Core, repositories, Unit of Work and PostgreSQL access.
- `backend/tests` for unit tests.
- `frontend/src` for the Blazor WebAssembly client.

## Technology Stack

- ASP.NET Core Web API
- .NET 8
- Entity Framework Core
- PostgreSQL / Supabase
- FluentValidation
- AutoMapper
- Swagger / OpenAPI
- Blazor WebAssembly
- xUnit

## Current Status

The repository has been scaffolded and the core business rules are in place. The backend, frontend, Docker files and schema script are ready for the next implementation phase.

## Folder Structure

```text
/backend
  /src
    /API
    /Application
    /Domain
    /Infrastructure
  /tests
/frontend
  /src
```

## How to Run Locally

1. Install the .NET 8 SDK.
2. Configure the connection string in `backend/src/API/appsettings.json` or via environment variables.
3. Restore dependencies and run the API project.
4. Run the Blazor WebAssembly frontend project.

## Database Setup

The schema is provided in `backend/database/scripts/001_initial_schema.sql`.

For Supabase, copy the connection string into `SUPABASE_CONNECTION_STRING` inside `.env`.

## Tests

Unit tests are included for:

- CPF validation
- Minimum age validation
- Order total calculation
- Order immutability after 24 hours

## Docker

- Backend Dockerfile: `backend/src/API/Dockerfile`
- Frontend Dockerfile: `frontend/src/Dockerfile`
- Compose file: `docker-compose.yml`

## Deployment Notes

- Backend: Render, Railway or Fly.io using the API Dockerfile.
- Frontend: Vercel or a static container host using the frontend Dockerfile.
- Database: Supabase PostgreSQL.

## Next Implementation Steps

1. Generate EF Core migrations from the configured DbContext.
2. Expand the Blazor UI with create, edit, delete and detail forms.
3. Add production-grade logging, pagination metadata and better error responses.# client_t-acquisition
