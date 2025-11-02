# 🚀 FCG - FIAP Pós-Graduação

API REST de autenticação e gerenciamento de usuários com AWS Cognito.  
**Projeto da Pós-Graduação em Arquitetura de Sistemas .NET - FIAP 2025**

---

## 📋 Sobre

Sistema que demonstra a aplicação prática de **Clean Architecture**, **CQRS**, **DDD** e integração com serviços cloud (AWS Cognito).

---

## 🏗️ Arquitetura e Padrões

### Clean Architecture - Camadas

```
├── Api/                    # Controllers, Middlewares, DI
├── Application/            # Commands, Queries, Handlers (CQRS)
├── Domain/                 # Entities, Value Objects, Interfaces
├── Infrastructure.Data/    # EF Core, Repositories, Migrations
└── Infrastructure.AWS/     # Cognito Integration
```

### Padrões Implementados

#### 1. **CQRS** (Command Query Responsibility Segregation)
- **Commands**: Operações de escrita (`SignUpCommand`, `SignInCommand`)
- **Handlers**: Processamento via MediatR
- **Validators**: FluentValidation para cada Command

```csharp
// Exemplo: SignUpCommand
public record SignUpCommand(string Name, string Email, string Password) : IRequest<Guid>;

// Handler correspondente
public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Guid>
{
    // Lógica de criação de usuário
}
```

#### 2. **Repository Pattern**
```csharp
IBaseRepository<T>          // Operações genéricas CRUD
IUserRepository             // Operações específicas de User
```

#### 3. **DDD** (Domain-Driven Design)
- **Entidades Ricas**: `User` com Factory Methods
- **Value Objects**: `Role` usando Enumeration Pattern
- **Domain Exceptions**: Validações de negócio

```csharp
// Factory Method
public static User CreateUser(string name, string email) 
    => new(name, email, Role.User);

// Enumeration Pattern
public sealed class Role
{
    public static readonly Role Admin = new(1, "Admin");
    public static readonly Role User = new(2, "User");
}
```

#### 4. **Dependency Injection**
Cada camada possui seu próprio `DependencyInjection.cs`:

```csharp
builder.Services.AddApiServices(configuration);         // Auth + Swagger + CORS
builder.Services.AddApplicationServices();              // MediatR + FluentValidation
builder.Services.AddDatabaseInfrastructure(configuration); // EF Core + Repositories
builder.Services.AddAwsInfrastructure(configuration);   // Cognito
```

#### 5. **Outros Padrões**
- **Mediator**: MediatR para desacoplamento
- **Strategy**: FluentValidation validators
- **Unit of Work**: DbContext do EF Core
- **Options**: Configurações tipadas (AWS, Cognito)

---

## 🔐 Autenticação JWT (AWS Cognito)

### Fluxo
1. **SignUp** → Cria usuário no Cognito + Banco local
2. **ConfirmSignUp** → Confirma email + Adiciona ao grupo (Admin/User)
3. **SignIn** → Retorna JWT com `cognito:groups`
4. **Endpoints protegidos** → Validação automática via JWT Bearer

### Políticas de Autorização
- `AdminOnly`: Requer `cognito:groups = "Admin"`
- `UserOrAdmin`: Requer apenas autenticação válida

---

## �️ Stack Técnica

- **.NET 8** | **ASP.NET Core Web API**
- **EF Core** (SQL Server) + **Migrations**
- **MediatR** (CQRS) | **FluentValidation**
- **AWS Cognito** (JWT Authentication)
- **Swagger/OpenAPI**

---

## 🚀 Como Executar

### 1. Configurar `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FCGDB;Trusted_Connection=True;"
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

### 2. Aplicar Migrations
```bash
dotnet ef database update
```

### 3. Executar
```bash
dotnet run
```
Acesse: `http://localhost:5005/swagger`

---

## � Endpoints Principais

| Método | Endpoint | Autenticação | Descrição |
|--------|----------|--------------|-----------|
| POST | `/api/accounts/signup` | ❌ | Cadastro |
| POST | `/api/accounts/confirm-signup` | ❌ | Confirmar email |
| POST | `/api/accounts/signin` | ❌ | Login (retorna JWT) |
| POST | `/api/accounts/change-password` | ✅ User | Alterar senha |
| POST | `/api/accounts/enable-user` | ✅ Admin | Habilitar usuário |
| POST | `/api/accounts/disable-user` | ✅ Admin | Desabilitar usuário |

---

## � Princípios SOLID

- **S**ingle Responsibility - Cada classe uma responsabilidade
- **O**pen/Closed - Extensível via interfaces
- **L**iskov Substitution - Implementações substituíveis
- **I**nterface Segregation - Interfaces específicas
- **D**ependency Inversion - Dependência de abstrações

---

## 🎓 Conceitos Demonstrados

✅ Clean Architecture  
✅ CQRS Pattern  
✅ Domain-Driven Design  
✅ Repository Pattern  
✅ Dependency Injection  
✅ JWT Authentication  
✅ Cloud Integration (AWS)  
✅ Entity Framework Core  
✅ FluentValidation  
✅ Middleware Pipeline  

---

**FIAP - Pós-Graduação Arquitetura de Sistemas .NET | 2025**

