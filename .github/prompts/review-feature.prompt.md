---
name: review-feature
description: Revise uma feature Khnum em busca de defeitos, regressões, problemas de segurança e testes ausentes.
---

Leia `.khnum/constitution.md`, `.khnum/quality-gates.md`, o manifesto da feature, a especificação, a implementação e os testes.

Relate primeiro os achados, ordenados por severidade, com referências clicáveis aos arquivos quando disponíveis. Priorize:

- comportamento incorreto de autenticação ou ordem incorreta de criação da sessão;
- token retornado antes de a persistência ser concluída;
- exposição de informações por meio de respostas ou logs;
- violações das dependências de arquitetura;
- divergências entre contrato e implementação;
- testes ausentes para cenários de aceitação.

Se não houver achados, declare isso claramente e liste as lacunas de testes restantes.

Ao finalizar a revisão, execute:

./tools/khnum.ps1 verify

Relate:
- resultado da verificação;
- testes executados;
- verificações ignoradas;
- decisões ainda pendentes.
