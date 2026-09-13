# Instruções de Engenharia Khnum

Antes de alterar o código, leia `.khnum/constitution.md`, o manifesto da feature relevante e sua especificação.

Para implementar uma feature:

1. Identifique as mudanças em Domain, Application, Infrastructure e Api.
2. Declare uma hipótese local sobre o comportamento e um teste que possa refutá-la.
3. Adicione ou atualize testes focados junto com a mudança de comportamento.
4. Mantenha as dependências alinhadas às regras de arquitetura.
5. Execute `./tools/khnum.ps1 verify` antes de informar a conclusão.
6. Relate decisões não resolvidas, verificações ignoradas e resultados dos testes.

Não adicione CQRS, mediadores, repositórios genéricos, eventos ou novas camadas sem uma necessidade documentada. Não exponha segredos em logs, saída de testes ou respostas de erro.
