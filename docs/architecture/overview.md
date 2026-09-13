# Arquitetura da solução

## Objetivo

Definir os limites arquiteturais da solução Khnum e orientar a evolução de novas funcionalidades sem acoplar regras de negócio a frameworks, protocolos ou detalhes de infraestrutura.

Este documento descreve decisões válidas para a solução como um todo. Regras de uma funcionalidade específica devem permanecer na documentação da respectiva feature em `specs/`.

## Estilo arquitetural

A solução adota uma arquitetura limpa em camadas, organizada por responsabilidades:

```text
Api -> Application -> Domain
Infrastructure -> Application
Infrastructure -> Domain
```

As dependências apontam para o núcleo da aplicação. O domínio representa o negócio; a Application coordena casos de uso; a API expõe interfaces externas; e a Infrastructure implementa integrações técnicas.

## Objetivos arquiteturais

- manter regras de negócio independentes de tecnologia;
- permitir testes unitários sem servidor ou banco;
- tornar integrações substituíveis por meio de portas e adaptadores;
- manter contratos externos separados dos modelos internos;
- permitir a adição de módulos sem concentrar toda a solução em uma única feature;
- preservar simplicidade enquanto a complexidade real não justificar abstrações adicionais.

## Fora do escopo

Este documento não define:

- entidades de um domínio específico;
- fluxos de autenticação ou autorização;
- contratos HTTP de uma feature;
- esquema de banco específico;
- algoritmo de hash, provedor de mensagens ou outra tecnologia particular.

Essas decisões devem ser registradas junto à feature ou em uma decisão arquitetural específica quando tiverem alcance transversal.

## Regra de evolução

Uma nova funcionalidade deve ser adicionada ao módulo ou bounded context ao qual pertence. Uma decisão só deve ser promovida para a arquitetura global quando houver necessidade comprovada de reutilização ou impacto em mais de um contexto.
