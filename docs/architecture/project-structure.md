# Estrutura de projetos

A estrutura abaixo é um modelo inicial. O nome de um módulo deve refletir seu bounded context ou capacidade de negócio, e não uma tecnologia específica.

```text
khnum/
├── docs/
│   └── architecture/
│       ├── overview.md
│       ├── layers.md
│       ├── project-structure.md
│       └── principles.md
├── specs/
│   └── <feature>/
│       ├── <feature>.spec.md
│       └── docs/
├── src/
│   ├── Khnum.Api/
│   │   ├── Endpoints/
│   │   │   └── <Context>/
│   │   ├── DependencyInjection/
│   │   ├── Middleware/
│   │   └── Program.cs
│   ├── Khnum.Application/
│   │   ├── Abstractions/
│   │   └── <Context>/
│   │       └── <UseCase>/
│   ├── Khnum.Domain/
│   │   ├── Common/
│   │   └── <Context>/
│   └── Khnum.Infrastructure/
│       ├── DependencyInjection/
│       ├── Persistence/
│       └── Integrations/
└── tests/
    ├── Khnum.Domain.Tests/
    ├── Khnum.Application.Tests/
    └── Khnum.IntegrationTests/
```

## Convenções

- Use um projeto por camada principal enquanto a escala da solução justificar essa divisão.
- Organize o código de negócio por contexto e caso de uso, evitando pastas globais como `Services` ou `Helpers` sem responsabilidade clara.
- Mantenha interfaces na camada que precisa delas; implementações ficam na Infrastructure.
- Não reutilize DTOs HTTP como comandos ou entidades de domínio.
- Separe testes unitários de testes que exigem composição, banco ou rede.
- Registre dependências concretas na borda da aplicação.
- Documente decisões específicas em `specs/<feature>/docs` ou em um ADR global.

## Crescimento da solução

Quando um contexto crescer de forma independente, ele pode ser extraído para módulos próprios. A extração deve ser motivada por limites de negócio, autonomia de evolução ou necessidade operacional, e não apenas pelo número de arquivos.
