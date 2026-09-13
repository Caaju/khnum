---
name: implement-feature
description: Implemente uma feature Khnum a partir da especificação usando testes e o harness do repositório.
---

Leia `.khnum/constitution.md`, `.khnum/quality-gates.md`, o manifesto da feature e a especificação antes de editar.

Implemente nesta ordem:

1. Comportamento e testes do Domain.
2. Portas, caso de uso e testes da Application.
3. Adaptadores da Infrastructure e testes de integração.
4. Contrato da Api e testes do endpoint.
5. Atualizações de documentação para qualquer decisão alterada.

Mantenha a mudança limitada à feature. Execute `./tools/khnum.ps1 verify` e informe os arquivos alterados, testes executados, verificações ignoradas e decisões não resolvidas.
