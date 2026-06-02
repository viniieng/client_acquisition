# Client Acquisition

Aplicação full stack para gestão de clientes e pedidos, desenvolvida como desafio técnico.

Construída com **ASP.NET Core .NET 8** no backend e **Blazor WebAssembly** no frontend, seguindo Clean Architecture e boas práticas de desenvolvimento.

🔗 **[Acesse a aplicação em produção](https://client-acquisition-frontend.onrender.com)**  
📦 **API:** https://client-acquisition-l9g5.onrender.com/api  
📖 **Swagger:** https://client-acquisition-l9g5.onrender.com/swagger

---

## Funcionalidades

- **Cadastro de Clientes** — com validação de CPF (único e válido), e-mail, endereço e idade mínima de 18 anos
- **Cadastro de Pedidos** — vinculados a clientes, com cálculo automático do valor total baseado nos itens
- **Regra de negócio:** pedidos não podem ser editados após 24 horas da criação
- **Consulta de Pedidos** — filtragem por nome do cliente ou intervalo de datas, com exibição de itens, valores e total gasto por cliente

---

## Arquitetura

Clean Architecture simplificada em camadas:

```
/backend
  /src
    /API             → endpoints HTTP, middleware, Swagger, injeção de dependências
    /Application     → DTOs, validações (FluentValidation), serviços e casos de uso
    /Domain          → entidades e regras de negócio
    /Infrastructure  → EF Core, repositórios, Unit of Work, acesso ao PostgreSQL
  /tests             → testes unitários (xUnit)
/frontend
  /src               → cliente Blazor WebAssembly
```

---

## Stack

| Camada | Tecnologia |
|---|---|
| Backend | ASP.NET Core Web API (.NET 8) |
| Frontend | Blazor WebAssembly |
| ORM | Entity Framework Core |
| Banco de Dados | PostgreSQL (Supabase) |
| Validação | FluentValidation |
| Mapeamento | AutoMapper |
| Documentação | Swagger / OpenAPI |
| Testes | xUnit |
| Containerização | Docker |
| Deploy | Render (backend + frontend) |

---

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/) (opcional, para rodar via container)
- PostgreSQL local **ou** uma connection string do [Supabase](https://supabase.com)

---

## Como Rodar Localmente

### 1. Clone o repositório

```bash
git clone https://github.com/seu-usuario/client-acquisition.git
cd client-acquisition
```

### 2. Configure o banco de dados

Crie um arquivo `.env` na raiz ou edite `backend/src/API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=client_acquisition;Username=postgres;Password=sua_senha"
  }
}
```

> Para Supabase, copie a connection string e defina `SUPABASE_CONNECTION_STRING` no `.env`.

### 3. Execute o script de banco

```bash
psql -U postgres -d client_acquisition -f backend/database/scripts/001_initial_schema.sql
```

### 4. Suba o backend

```bash
cd backend/src/API
dotnet restore
dotnet run
```

API disponível em: `http://localhost:5000`  
Swagger em: `http://localhost:5000/swagger`

### 5. Suba o frontend

```bash
cd frontend/src
dotnet restore
dotnet run
```

Frontend disponível em: `http://localhost:5001`

---

## Como Rodar via Docker

### Subir tudo com Docker Compose

```bash
docker-compose up --build
```

| Serviço | URL |
|---|---|
| Frontend | http://localhost:8080 |
| Backend (API) | http://localhost:5000 |
| Swagger | http://localhost:5000/swagger |

### Rodar apenas o backend

```bash
docker build -t client-acquisition-api -f backend/src/API/Dockerfile .
docker run -p 5000:5000 \
  -e ConnectionStrings__DefaultConnection="sua_connection_string" \
  client-acquisition-api
```

### Rodar apenas o frontend

```bash
docker build -t client-acquisition-frontend -f frontend/src/Dockerfile .
docker run -p 8080:8080 \
  -e API_BASE_URL="http://localhost:5000/api/" \
  client-acquisition-frontend
```

---

## Testes

```bash
cd backend/tests
dotnet test
```

Cobertura atual:

- ✅ Validação de CPF
- ✅ Validação de idade mínima (18 anos)
- ✅ Cálculo do total do pedido
- ✅ Bloqueio de edição após 24 horas

---

## Variáveis de Ambiente

### Backend

| Variável | Descrição | Exemplo |
|---|---|---|
| `SUPABASE_CONNECTION_STRING` | Connection string do PostgreSQL | `Host=...;Database=...` |
| `ASPNETCORE_ENVIRONMENT` | Ambiente de execução | `Production` |

### Frontend

| Variável | Descrição | Padrão |
|---|---|---|
| `API_BASE_URL` | URL base da API | `http://localhost:5000/api/` |

---

## Banco de Dados

O schema inicial está em:

```
backend/database/scripts/001_initial_schema.sql
```

Tabelas criadas:

- `customers` — clientes com CPF único, e-mail e data de nascimento
- `orders` — pedidos vinculados a clientes com data de criação
- `order_items` — itens de cada pedido com quantidade e valor unitário

---

## Deploy

| Componente | Plataforma | Observação |
|---|---|---|
| Backend | Render | Dockerfile em `backend/src/API/Dockerfile` |
| Frontend | Render | Dockerfile em `frontend/src/Dockerfile` |
| Banco de Dados | Supabase | PostgreSQL gerenciado |

---

## Estrutura de Pastas

```
client-acquisition/
├── backend/
│   ├── database/
│   │   └── scripts/
│   │       └── 001_initial_schema.sql
│   ├── src/
│   │   ├── API/
│   │   ├── Application/
│   │   ├── Domain/
│   │   └── Infrastructure/
│   └── tests/
├── frontend/
│   └── src/
├── docker-compose.yml
└── ClientAcquisition.sln
```