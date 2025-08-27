# Tech Challenge DGTallab

Projeto criado como teste técnico para a DGTallab. A solução contém:

- API REST em ASP.NET Core (.NET 9) com EF Core (PostgreSQL)
- Frontend Blazor WebAssembly
- AppHost (.NET Aspire) para orquestrar e iniciar API e Frontend juntos

## Requisitos

- .NET SDK 9.0
- PostgreSQL 17
- Acesso para criar/alterar banco (usuário e senha)

## Banco de dados

A conexão é configurada via `appsettings.json` do projeto `TechChallengeDgtallab.ApiService`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=techchallenge;Username=postgres;Password=masterkey"
  }
}
```

Observações:
- A base `techchallenge` deve existir no PostgreSQL (ou o usuário deve ter permissão para criá-la).
- As migrações do EF Core são aplicadas automaticamente no startup da API (`ApplyDatabaseMigrations`).
- Você pode sobrescrever a connection string por variável de ambiente `ConnectionStrings__DefaultConnection` se preferir.

## Como executar (AppHost)

O AppHost inicia a API e o Frontend em sequência, expondo os endpoints HTTP externamente.

Opção 1 — Visual Studio/VS Code:
- Selecione o projeto de inicialização: `TechChallengeDgtallab.AppHost`
- Inicie em modo Debug/Run (F5)

Opção 2 — CLI (na raiz do repositório):
```powershell
# Windows PowerShell
# Inicia AppHost, que sobe API e Frontend
 dotnet run --project .\source\TechChallengeDgtallab.AppHost\TechChallengeDgtallab.AppHost.csproj
```

## Endereços padrão

- API: http://localhost:5598
  - Swagger: http://localhost:5598/ (Swagger UI na raiz)
- Frontend (Blazor WASM): http://localhost:5118

As URLs acima também estão definidas em `launchSettings.json` e referenciadas em `TechChallengeDgtallab.Core.Configuration`.

## Estrutura dos projetos

- `TechChallengeDgtallab.ApiService` — API ASP.NET Core, controllers, handlers, serviços e migrações EF Core
- `TechChallengeDgtallab.Web` — Frontend Blazor WebAssembly (MudBlazor, Blazored.LocalStorage)
- `TechChallengeDgtallab.Infra` — Contexto EF Core, repositórios e mapeamentos
- `TechChallengeDgtallab.Core` — DTOs, requests/responses, contratos e configurações compartilhadas
- `TechChallengeDgtallab.AppHost` — Orquestração (Aspire) para iniciar API e Frontend juntos
