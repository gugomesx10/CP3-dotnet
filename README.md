# 🎮 ArenaSync API

## 📌 Sobre o Projeto

A **ArenaSync API** é uma API RESTful desenvolvida em **ASP.NET Core .NET 8**, com foco no gerenciamento de times competitivos de e-sports, jogadores e perfis competitivos.

O projeto foi desenvolvido como parte do **CP3 - Advanced Business Development with .NET - 2026**, aplicando conceitos avançados de:

- APIs RESTful
- Entity Framework Core
- Relacionamentos entre entidades
- Oracle Database
- DTOs
- Swagger/OpenAPI
- Data Annotations
- Arquitetura em camadas
- Boas práticas de desenvolvimento

---

# 👨‍🎓 Informações Acadêmicas

- **Aluno:** Gustavo Gomes Martins
- **RM:** 555999
- **Turma:** 2TDSPO

---

# 🛠️ Tecnologias Utilizadas

## Backend
- ASP.NET Core .NET 8
- C#
- Entity Framework Core 8
- Oracle Entity Framework Core
- Swagger / OpenAPI

## Banco de Dados
- Oracle Database

## Ferramentas
- Rider / Visual Studio
- Git & GitHub
- Swagger UI

---

# 📂 Estrutura do Projeto

```text
/Controllers
    TimeController.cs
    JogadorController.cs
    PerfilCompetitivoController.cs

/Entities
    Time.cs
    Jogador.cs
    PerfilCompetitivo.cs

/DTOs
    /Responses
        TimeResumoDto.cs
        JogadorResumoDto.cs
        PerfilCompetitivoResponseDto.cs

    TimeCreateDto.cs
    JogadorCreateDto.cs
    PerfilCompetitivoCreateDto.cs

/Data
    ApplicationContext.cs
    ApplicationContextFactory.cs

/Migrations

Program.cs
appsettings.json
README.md
```

---

# 🧠 Regras de Negócio

A API possui dois relacionamentos obrigatórios:

## 🔹 Relacionamento 1:N

### Time → Jogadores

Um time pode possuir vários jogadores.

### Exemplo

- FURIA
    - KSCERATO
    - yuurih
    - FalleN

---

## 🔹 Relacionamento 1:1

### Jogador → PerfilCompetitivo

Cada jogador possui apenas um perfil competitivo.

### Exemplo

- KSCERATO
    - KDA
    - WinRate
    - Horas Jogadas

---

# 🗄️ Entidades

## 🏆 Time

| Campo | Tipo |
|---|---|
| Id | int |
| Nome | string |
| Jogo | string |
| Pais | string |
| Ranking | int |

---

## 🎯 Jogador

| Campo | Tipo |
|---|---|
| Id | int |
| Nickname | string |
| Funcao | string |
| Idade | int |
| TimeId | int |

---

## 📈 PerfilCompetitivo

| Campo | Tipo |
|---|---|
| Id | int |
| KDA | double |
| WinRate | double |
| HorasJogadas | int |
| JogadorId | int |

---

# 🔗 Relacionamentos Implementados

## 1:N

```text
Time
 └── Jogadores
```

## 1:1

```text
Jogador
 └── PerfilCompetitivo
```

---

# 📦 DTOs Utilizados

O projeto utiliza DTOs para:
- entrada de dados
- saída personalizada
- evitar ciclos infinitos
- melhorar organização da API

## DTOs de Entrada

- TimeCreateDto
- JogadorCreateDto
- PerfilCompetitivoCreateDto

## DTOs de Resposta

- TimeResumoDto
- JogadorResumoDto
- PerfilCompetitivoResponseDto

---

# ⚙️ Configuração do Banco Oracle

## appsettings.json

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/orcl"
  }
}
```

---

# 🚀 Como Executar o Projeto

## 1️⃣ Clonar o repositório

```bash
git clone URL_DO_REPOSITORIO
```

---

## 2️⃣ Restaurar dependências

```bash
dotnet restore
```

---

## 3️⃣ Executar migrations

```bash
dotnet ef database update
```

---

## 4️⃣ Rodar o projeto

```bash
dotnet run
```

---

# 📄 Swagger

A documentação Swagger pode ser acessada em:

```text
https://localhost:PORTA/swagger
```

---

# 📌 Endpoints Disponíveis

# 🏆 Time

## GET

```http
GET /api/Time
```

## GET por ID

```http
GET /api/Time/{id}
```

## GET por jogo

```http
GET /api/Time/jogo/{jogo}
```

## POST

```http
POST /api/Time
```

## PUT

```http
PUT /api/Time/{id}
```

## DELETE

```http
DELETE /api/Time/{id}
```

---

# 🎯 Jogador

## GET

```http
GET /api/Jogador
```

## GET por ID

```http
GET /api/Jogador/{id}
```

## GET por Time

```http
GET /api/Jogador/time/{timeId}
```

## GET por Função

```http
GET /api/Jogador/funcao/{funcao}
```

## POST

```http
POST /api/Jogador
```

## PUT

```http
PUT /api/Jogador/{id}
```

## DELETE

```http
DELETE /api/Jogador/{id}
```

---

# 📈 PerfilCompetitivo

## GET

```http
GET /api/PerfilCompetitivo
```

## GET por ID

```http
GET /api/PerfilCompetitivo/{id}
```

## GET por Jogador

```http
GET /api/PerfilCompetitivo/jogador/{jogadorId}
```

## POST

```http
POST /api/PerfilCompetitivo
```

## PUT

```http
PUT /api/PerfilCompetitivo/{id}
```

## DELETE

```http
DELETE /api/PerfilCompetitivo/{id}
```

---

# 🧪 Exemplos de Requisição

# POST - Time

```json
{
  "nome": "FURIA",
  "jogo": "CS2",
  "pais": "Brasil",
  "ranking": 3
}
```

---

# POST - Jogador

```json
{
  "nickname": "KSCERATO",
  "funcao": "Rifler",
  "idade": 25,
  "timeId": 1
}
```

---

# POST - PerfilCompetitivo

```json
{
  "kda": 1.32,
  "winRate": 68,
  "horasJogadas": 12000,
  "jogadorId": 1
}
```

---

# ✅ Funcionalidades Implementadas

- CRUD completo
- Relacionamento 1:1
- Relacionamento 1:N
- DTOs de entrada
- DTOs de resposta
- Include e ThenInclude
- Swagger/OpenAPI
- XML Documentation
- SwaggerResponse
- Entity Framework Core
- Oracle Database
- Migrations
- Validações com Data Annotations
- Status Codes corretos
- API RESTful padronizada

---

# 📚 Conceitos Aplicados

- REST API
- Clean Code
- DTO Pattern
- Entity Framework Core
- Swagger Documentation
- Relational Database
- Oracle SQL
- Dependency Injection
- Data Validation
- Serialization
- Boas práticas em APIs

---

# ✅ Status do Projeto

✔️ Projeto finalizado  
✔️ Banco Oracle integrado  
✔️ Swagger funcionando  
✔️ Relacionamentos implementados  
✔️ API funcional