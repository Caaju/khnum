---
name: analyze-feature
description: Analise uma especificação de feature Khnum em busca de contradições, decisões ausentes e critérios de aceitação testáveis.
---

Leia `.khnum/constitution.md`, `.khnum/quality-gates.md` e a pasta da feature solicitada em `specs/`.

Retorne:

1. Um resumo conciso do comportamento.
2. Contradições entre especificação, OpenAPI, modelo de dados e cenários de aceitação.
3. Decisões ausentes necessárias antes da implementação.
4. Uma lista de testes focados, agrupados por Domain, Application e integração.
5. A menor ordem de implementação possível.

Não modifique arquivos durante a análise.
