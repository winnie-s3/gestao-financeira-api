# 💰 Financial Manager API

API para gestão financeira pessoal, permitindo controle de receitas, despesas e análise de dados ao longo do tempo.

O projeto foi desenvolvido com foco em **organização, aplicação de regras de negócio reais e construção de uma API completa**, simulando um sistema utilizado em produção.


## Tecnologias utilizadas

- ASP.NET Core
- Entity Framework Core
- SQL Server
- LINQ
- C#


## 📌 Funcionalidades

### Receitas (Incomes)

- CRUD completo (Create, Read, Update, Delete)
- Filtro por período (data inicial e final)
- Associação com categorias
- Cálculo de total

### Despesas (Expenses)

- CRUD completo
- Filtro por período
- Associação com categorias
- Cálculo de total
- Agrupamento por categoria

### Categorias (Categories)

- CRUD completo
- Validação de duplicidade de nome
- Relacionamento com receitas e despesas

### Dashboard

- Resumo financeiro:
  - Total de receitas
  - Total de despesas
  - Saldo (balance)

- Análise mensal:
  - Receita vs despesa por mês
  - Nome do mês formatado (pt-BR)

- Análise por categoria:
  - Gastos por categoria (ordenado por maior valor)
  - Receitas por categoria

- Endpoint completo:
  - `/dashboard/full` → retorna todos os dados em uma única chamada


## Exemplos de análise

O sistema permite visualizar:

- Distribuição de gastos por categoria
- Evolução financeira ao longo dos meses
- Comparação entre receitas e despesas
- Saldo financeiro consolidado


## Decisões técnicas

- Uso de `IQueryable` para otimizar consultas no banco
- Agrupamentos com `GroupBy` para geração de relatórios
- Separação de responsabilidades com uso de DTOs
- Criação de `ApiResponse<T>` para padronização das respostas
- Validação de regras de negócio (ex: mês exige ano)
- Uso de `OrderByDescending` para priorizar dados mais relevantes
- Tratamento de erros com mensagens claras para o cliente da API


## 🔧 Funcionalidades em desenvolvimento

- Importação de dados via CSV
- Padronização completa de todas as respostas da API
- Inclusão de paginação nas listagens
- Melhorias nas validações
- Melhor organização da camada de serviços


## 🖥️ Front-end (em breve)

Uma interface web será desenvolvida para consumo da API, permitindo:

- Visualização dos dados em dashboard
- Cadastro de receitas e despesas
- Gráficos financeiros
- Melhor experiência do usuário


## Como executar o projeto

1. Clonar o repositório:

```bash
git clone https://github.com/winnie-s3/gestao-financeira-api.git
```

2. Acessar a pasta do projeto:

```bash
cd gestao-financeira-api
```

3. Configurar a string de conexão no appsettings.json

4. Rodar as migrations:

```bash
dotnet ef database update
```

5. Executar a aplicação:

```bash
dotnet run
```


## Estrutura do projeto

- Controllers → endpoints da API
- Entities → entidades do banco
- DTOs → transferência de dados
- Services → contexto do banco (DbContext)
- Responses → padronização das respostas (ApiResponse)


## Melhorias futuras

- Autenticação e autorização (JWT)
- Deploy em ambiente cloud (Azure / AWS)
- Logs e monitoramento
- Testes automatizados
- Cache para otimização de performance


## Sobre o projeto

Este projeto foi desenvolvido com o objetivo de praticar e consolidar conhecimentos em desenvolvimento backend com .NET.

Ele cobre desde operações básicas de CRUD até geração de relatórios e aplicação de regras de negócio, simulando um sistema financeiro real.

Além disso, faz parte de um processo de evolução prática, com foco em construção de projetos completos e preparação para o mercado.
