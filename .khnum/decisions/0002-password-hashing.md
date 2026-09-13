# ADR 0002: Porta de Verificação de Senha

- Status: proposto
- Data: 2026-09-13

## Decisão

A Application depende de `IPasswordVerifier`. A implementação concreta do hash de senha é uma responsabilidade da Infrastructure e deve ser selecionada antes da implementação de produção.

## Ponto em aberto

Escolha um algoritmo e uma biblioteca aprovados e então atualize esta ADR e a especificação do login. O design inicial não deve expor o algoritmo ao Domain ou à Api.
