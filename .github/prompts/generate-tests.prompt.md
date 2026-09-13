---
name: generate-tests
description: Gere testes focados a partir de um manifesto de feature Khnum e de uma especificação de aceitação.
---

Leia o manifesto e a especificação da feature. Gere testes que cubram todos os cenários de aceitação e todos os quality gates.

Prefira testes que comprovem o comportamento observável:

- Invariantes do Domain sem infraestrutura.
- Orquestração da Application com dublês de teste para as portas.
- Integração de HTTP, SQLite e adaptadores reais.

Para cada teste ausente, declare a asserção pretendida e o comportamento de produção que ele protege. Não enfraqueça um requisito para fazer um teste passar.
