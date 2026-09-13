---
name: scaffold-solution
description: Crie a estrutura inicial de uma solução .NET Khnum com projetos, referências arquiteturais e testes base.
---

Leia antes de editar:

- `.khnum/constitution.md`
- `.khnum/quality-gates.md`
- `docs/architecture/overview.md`
- `docs/architecture/layers.md`
- `docs/architecture/project-structure.md`
- `khnum.slnx`

## Objetivo

Crie somente a fundação técnica da solução. Não implemente regras da feature de login nem outros comportamentos de negócio.

## Projetos obrigatórios

Crie:

```text
src/
├── Khnum.Domain/
├── Khnum.Application/
├── Khnum.Infrastructure/
└── Khnum.Api/

tests/
├── Khnum.Domain.Tests/
├── Khnum.Application.Tests/
└── Khnum.IntegrationTests/
```

Use:

- biblioteca de classes para Domain, Application e Infrastructure;
- ASP.NET Core Web API para Api;
- framework de testes já adotado pelo SDK ou pela solução.

## Referências permitidas

Configure as referências exatamente assim:

```text
Khnum.Domain
  sem referências a projetos da solução

Khnum.Application
  -> Khnum.Domain

Khnum.Infrastructure
  -> Khnum.Application
  -> Khnum.Domain

Khnum.Api
  -> Khnum.Application
  -> Khnum.Infrastructure

Khnum.Domain.Tests
  -> Khnum.Domain

Khnum.Application.Tests
  -> Khnum.Application
  -> Khnum.Domain

Khnum.IntegrationTests
  -> Khnum.Api
  -> Khnum.Application
  -> Khnum.Infrastructure
```

Não adicione Entity Framework Core, SQLite, FluentValidation ou bibliotecas de hash nesta etapa, a menos que sejam indispensáveis para a criação da fundação. Dependências específicas devem ser adicionadas durante a implementação da feature.

## Estrutura inicial

Crie apenas diretórios e arquivos mínimos:

```text
Khnum.Domain/
Khnum.Application/
Khnum.Infrastructure/
Khnum.Api/
  Program.cs

Khnum.Domain.Tests/
Khnum.Application.Tests/
Khnum.IntegrationTests/
```

Não crie entidades, handlers, endpoints de login, repositórios ou regras de negócio nesta etapa.

## Solução

Adicione todos os projetos ao `khnum.slnx`.

Se a solução existente não puder ser utilizada pelo SDK instalado, informe o problema claramente e não substitua o arquivo sem confirmação.

## Validação

Depois de criar a estrutura:

1. Execute `dotnet restore`.
2. Execute `dotnet build --configuration Release`.
3. Execute `dotnet test --configuration Release`.
4. Execute `./tools/khnum.ps1 verify`.

Se o harness ainda não conseguir executar alguma verificação, informe o motivo.

## Critérios de conclusão

A tarefa só estará concluída quando:

- todos os projetos existirem;
- todos estiverem associados ao `khnum.slnx`;
- as referências entre projetos respeitarem a arquitetura;
- a solução compilar;
- os projetos de teste forem descobertos;
- os testes base passarem;
- `./tools/khnum.ps1 verify` for executado.

Relate ao final:

- projetos criados;
- referências adicionadas;
- pacotes adicionados;
- comandos executados;
- resultado de cada validação;
- verificações ignoradas e seus motivos;
- decisões ainda pendentes.
