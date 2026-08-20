# 🏙️ API Cidades & Alunos

API REST em **C# / ASP.NET Core** com **MySQL**, desenvolvida como projeto acadêmico.
Permite importar cidades a partir de um arquivo CSV, consultar cidades e estados (UFs),
e inclui um módulo extra para armazenar e consultar a foto de um aluno em base64.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-ASP.NET%20Core-239120?logo=csharp&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-ADO.NET-4479A1?logo=mysql&logoColor=white)
![OpenAPI](https://img.shields.io/badge/Docs-OpenAPI%20%2F%20Scalar-6BA539?logo=openapiinitiative&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-blue)

---

## 📌 Sobre o projeto

O projeto simula um cenário real de backend: receber um arquivo bruto (CSV com
mais de 5.500 cidades brasileiras), processá-lo e expor os dados através de uma
API REST completa, com CRUD, filtros e documentação automática.

**Principais funcionalidades:**
- 📥 Importação de cidades via upload de CSV (com *upsert* — reimportar não duplica dados)
- 🔎 Consulta de cidades por ID, por estado (UF) ou lista completa
- ✏️ CRUD completo (criar via importação, editar e excluir)
- 📷 Upload e consulta de foto de aluno, convertida para base64
- 📖 Documentação interativa da API via OpenAPI (Scalar)

---

## 🛠️ Tecnologias

| Camada | Tecnologia |
|---|---|
| Linguagem | C# |
| Framework | ASP.NET Core (Web API) |
| Banco de dados | MySQL |
| Acesso a dados | ADO.NET puro (`MySql.Data`) |
| Leitura de CSV | CsvHelper |
| Autenticação | JWT Bearer |
| Documentação | OpenAPI + Scalar |

---

## 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas simples e explícita, sem ORM,
usando ADO.NET diretamente para ter controle total sobre as queries:

```
Controllers   →  recebem a requisição HTTP, validam entrada e formatam a resposta
Services      →  regra de negócio, faz a ponte entre Controller e Repository
Repository    →  acesso ao banco (MySQL) via ADO.NET puro
Entidades     →  classes que representam as tabelas do banco
DTOs          →  objetos de request/response expostos pela API
```

### Estrutura de pastas
```
IntroAPI/
├── Controllers/
│   ├── DTOS/
│   │   ├── CidadeResponse.cs
│   │   ├── CidadeAlterarRequest.cs
│   │   └── Aluno*.cs
│   ├── CidadesController.cs
│   └── AlunosController.cs
├── Entidades/
│   ├── Cidade.cs
│   └── Aluno.cs
├── Repository/
│   ├── MySqlDbContext.cs
│   ├── CidadeRepository.cs
│   └── AlunoRepository.cs
├── Services/
│   ├── CidadeServices.cs
│   └── AlunoServices.cs
├── Exemplo/
│   ├── cidade.csv
│   └── script.sql
├── Program.cs
└── appsettings.json
```

---

## 🚀 Como rodar o projeto

### Pré-requisitos
- [.NET SDK 10](https://dotnet.microsoft.com/download)
- MySQL Server acessível (local ou remoto)

### Passo a passo

```bash
# 1. Clonar o repositório
git clone https://github.com/Caio-Aranda/aspnet-cidades-api.git
cd aspnet-cidades-api

# 2. Restaurar os pacotes
dotnet restore
```

3. Ajuste a string de conexão em `appsettings.json` com os dados do seu MySQL:
```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=cidades_db;user=root;password=SUA_SENHA;"
}
```

4. Execute o script `Exemplo/script.sql` no seu banco para criar as tabelas necessárias.

```bash
# 5. Rodar a aplicação
dotnet run
```

6. Acesse a documentação interativa da API:
```
https://localhost:{porta}/doc
```

---

## 📚 Endpoints

### Cidades

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/Cidades/importar` | Importa cidades a partir de um arquivo CSV |
| `GET` | `/Cidades` | Lista todas as cidades |
| `GET` | `/Cidades/total` | Retorna a quantidade total de cidades |
| `GET` | `/Cidades/{id}` | Retorna uma cidade pelo ID |
| `GET` | `/Cidades/estados` | Lista as UFs cadastradas |
| `GET` | `/Cidades/estado/{uf}` | Lista as cidades de uma UF |
| `PUT` | `/Cidades/{id}` | Atualiza uma cidade |
| `DELETE` | `/Cidades/{id}` | Remove uma cidade |

**Exemplo — importar CSV:**
```bash
curl -X POST https://localhost:{porta}/Cidades/importar \
  -F "arquivo=@Exemplo/cidade.csv"
```

### Alunos (foto em base64)

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/Alunos/{id}/foto` | Envia a foto de um aluno |
| `GET` | `/Alunos/{id}/foto` | Retorna a foto do aluno em base64 |

**Exemplo — enviar foto:**
```bash
curl -X POST https://localhost:{porta}/Alunos/1/foto \
  -F "foto=@foto.jpg"
```

---

## 💡 Decisões técnicas

- **Upsert na importação**: o CSV pode ser reimportado a qualquer momento sem gerar
  duplicados — cidades já existentes são atualizadas via `ON DUPLICATE KEY UPDATE`.
- **ADO.NET puro**: sem ORM, para ter controle explícito sobre as queries SQL e
  reforçar o entendimento da camada de acesso a dados.
- **Foto armazenada como `LONGBLOB`**: mantém o projeto autocontido, sem depender
  de armazenamento externo.

---

## ✍️ Autor

**Caio Aranda**
Estudante de Sistemas de Informação — Unoeste
[GitHub](https://github.com/Caio-Aranda) · caioaranda28@gmail.com
