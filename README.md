#  API Cidades & Alunos

API REST em **C# / ASP.NET Core** com **MySQL**, desenvolvida como projeto de avaliação da disciplina de Linguagens de Programação I. 
O sistema permite importar dados geográficos a partir de um arquivo CSV, realizar o CRUD completo de cidades e alunos, e inclui um módulo para conversão, armazenamento e consulta de fotos de perfil em formato Base64.

![.NET](https://img.shields.io/badge/.NET-10-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-ASP.NET%20Core-239120?logo=csharp&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-ADO.NET-4479A1?logo=mysql&logoColor=white)
![OpenAPI](https://img.shields.io/badge/Docs-OpenAPI%20%2F%20Scalar-6BA539?logo=openapiinitiative&logoColor=white)
![License](https://img.shields.io/badge/license-MIT-blue)

---

##  Sobre o projeto

O projeto simula um cenário real de backend: receber um arquivo bruto (CSV com milhares de cidades brasileiras), processá-lo em memória e expor os dados através de uma API REST completa e documentada de forma automatizada.

**Principais funcionalidades:**
-  Importação em massa de cidades via upload de arquivo CSV.
-  Consulta de cidades por ID, por estado (UF) ou listagem completa.
-  Cadastro e gerenciamento de Alunos com vínculo de Cidades.
-  Upload de fotos de alunos (conversão em backend e armazenamento em Base64).
-  Documentação interativa e visual da API via OpenAPI (Scalar).

---

##  Tecnologias

| Camada | Tecnologia |
|---|---|
| Linguagem | C# |
| Framework | ASP.NET Core (Web API) |
| Banco de dados | MySQL |
| Acesso a dados | ADO.NET puro (`MySql.Data`) |
| Leitura de Arquivos | Nativa (`StreamReader` / `MemoryStream`) |
| Documentação | OpenAPI + Scalar |

---

##  Arquitetura

O projeto segue a arquitetura em camadas, utilizando a Injeção de Dependência nativa do .NET e dispensando o uso de ORMs (como Entity Framework) para priorizar o controle direto das transações via ADO.NET:

```text
Controllers   →  Recebem as requisições HTTP, validam os dados (DTOs) e formatam a resposta.
Services      →  Camada de regras de negócio (ex: conversão de arquivos e lógica do CSV).
Repository    →  Acesso exclusivo ao banco de dados (MySQL) com controle de transações.
Entidades     →  Modelos que representam as tabelas do banco.
DTOs          →  Objetos de transferência de dados para isolar as entidades da exposição web.

```

### Estrutura de pastas principal

```text
IntroAPI/
├── Controllers/
│   ├── DTOS/
│   │   ├── AlunoResponse.cs
│   │   ├── AlunoCriarRequest.cs
│   │   ├── AlunoAlterarRequest.cs
│   │   └── AlunoAlterarParcialRequest.cs
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
├── Program.cs
└── appsettings.json

```

---

##  Como rodar o projeto

### Pré-requisitos

* [.NET SDK](https://dotnet.microsoft.com/download) instalado.
* Servidor MySQL rodando localmente (XAMPP, MySQL Workbench, etc.) ou em nuvem.

### Passo a passo

1. **Clone o repositório:**

```bash
git clone [https://github.com/Caio-Aranda/](https://github.com/Caio-Aranda/)[NOME-DO-SEU-REPOSITORIO].git
cd [NOME-DO-SEU-REPOSITORIO]

```

2. **Configure o Banco de Dados:**
Ajuste a string de conexão no arquivo `appsettings.json` com os dados do seu MySQL:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=NomeDoSeuBanco;uid=root;password=SUA_SENHA;"
}

```

*Certifique-se de que as tabelas `Cidade` e `Aluno` já estejam criadas no seu banco de dados.*

3. **Rode a aplicação:**

```bash
dotnet run

```

4. **Acesse a documentação interativa:**
Abra o navegador e acesse a interface do Scalar para testar os endpoints:

```text
https://localhost:{porta}/scalar/v1

```

---

##  Endpoints Principais

### Cidades

| Método | Rota | Descrição |
| --- | --- | --- |
| `POST` | `/api/Cidades/importar` | Importa cidades a partir de um arquivo CSV |
| `GET` | `/api/Cidades` | Lista todas as cidades |
| `GET` | `/api/Cidades/total` | Retorna a quantidade total de cidades |
| `GET` | `/api/Cidades/{id}` | Retorna uma cidade pelo ID |
| `GET` | `/api/Cidades/estados` | Lista as UFs cadastradas |
| `GET` | `/api/Cidades/estado/{uf}` | Lista as cidades de uma UF específica |

### Alunos

| Método | Rota | Descrição |
| --- | --- | --- |
| `GET` / `POST` / `PUT` / `DELETE` | `/api/Alunos` | CRUD completo da entidade Aluno |
| `POST` | `/api/Alunos/{id}/foto` | Recebe arquivo de imagem e salva como Base64 |
| `GET` | `/api/Alunos/{id}/foto` | Retorna a string Base64 da foto do aluno |

---

##  Decisões Técnicas

* **Transações Manuais:** A importação em massa das cidades utiliza `MySqlTransaction` (`BeginTransaction` e `Commit`), garantindo a integridade do banco: se uma linha do CSV falhar, nenhuma cidade é salva pela metade.
* **Leitura Nativa:** Dispensa de pacotes de terceiros (como `CsvHelper`). O CSV é lido diretamente via `StreamReader`, tratando culturas numéricas e quebras de linha dinamicamente.
* **Armazenamento de Imagens:** Fotos convertidas no backend via `MemoryStream` e gravadas no MySQL utilizando o tipo `LONGTEXT`, facilitando o consumo direto no front-end em tags `<img>`.

---

##  Autor

**Caio Aranda**

Estudante de Sistemas de Informação — Unoeste
[GitHub](https://github.com/Caio-Aranda) · caioaranda28@gmail.com
