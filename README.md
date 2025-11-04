# FCG - FIAP Cloud Games

**Tech Challenge - Fase 1**  
Plataforma de venda de jogos digitais e gestão de usuários.

## Sobre o Projeto

A FIAP Cloud Games (FCG) é uma API REST desenvolvida em .NET 8 para gerenciar usuários, jogos e promoções. Este MVP implementa autenticação segura, cadastro de jogos e biblioteca pessoal de jogos adquiridos.


## Arquitetura

O projeto segue **Clean Architecture** com as seguintes camadas:

```
├── FCG.Api/                # Controllers, Middlewares
├── FCG.Application/        # Commands, Queries (CQRS)
├── FCG.Domain/            # Entidades, Regras de Negócio
├── FCG.Infrastructure.Data/    # EF Core, Repositories
└── FCG.Infrastructure.AWS/     # AWS Cognito
```

### Padrões Utilizados
- **CQRS** - Separação de Commands e Queries
- **DDD** - Domain-Driven Design
- **Repository Pattern** - Abstração de dados
- **Mediator Pattern** - MediatR
- **Dependency Injection** - Inversão de controle

## Autenticação

Sistema de autenticação via **AWS Cognito** com JWT Bearer:
- **Cadastro** com validação de senha segura
- **Confirmação** de email
- **Login** com token JWT
- **Dois níveis de acesso**: Admin e User

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- AWS Cognito
- MediatR + FluentValidation
- xUnit + FluentAssertions

## Como Executar

### Pré-requisitos
- .NET 8 SDK
- SQL Server
- Conta AWS (Cognito configurado)

### Configuração

1. **Configure o `appsettings.json`**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FCGDB;..."
  },
  "AWS": {
    "Region": "us-east-1",
    "Cognito": {
      "UserPoolId": "us-east-1_XXX",
      "ClientId": "xxx",
      "ClientSecret": "xxx"
    }
  }
}
```

2. **Configure variáveis de ambiente** (Admin seed):
```bash
export Admin__Email="admin@fcg.com"
export Admin__Password="Admin@123"
```

3. **Aplique as migrations**:
```bash
dotnet ef database update --project src/FCG.Infrastructure.Data
```

4. **Execute o projeto**:
```bash
dotnet run --project src/FCG.Api
```

5. **Acesse o Swagger**: `http://localhost:5005/swagger`

## Funcionalidades

### Usuários
- Cadastro com validações (email, senha segura)
- Autenticação JWT
- Confirmação de email
- Recuperação de senha
- Ativação/Desativação (Admin)

### Jogos
- Cadastro de jogos (Admin)
- Listagem de jogos ativos
- Ativação/Desativação (Admin)
- Compra de jogos (User)
- Biblioteca pessoal

### Promoções
- Criação de promoções com desconto (Admin)
- Validação de período
- Listagem de promoções

## Estrutura do Banco

**Entidades principais:**
- `Users` - Usuários do sistema
- `Games` - Catálogo de jogos
- `UserGames` - Biblioteca (relacionamento N:N)
- `Promotions` - Descontos em jogos

## Testes

Execute os testes unitários:
```bash
dotnet test
```

Testes implementados em:
- `FCG.Domain.Tests` - Entidades e regras de negócio
