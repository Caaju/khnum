# ADR 0001: Arquitetura Limpa em Camadas

- Status: aceito
- Data: 2026-09-13

## Decisão

A solução utiliza os projetos Domain, Application, Infrastructure e Api. As dependências apontam para o núcleo. A Application define as portas necessárias aos seus casos de uso; a Infrastructure as implementa.

## Consequências

O Domain permanece independente de tecnologia. Os detalhes de HTTP e persistência ficam nas bordas. Algum código de mapeamento é intencional e preferível a deixar contratos externos vazarem para o núcleo.
