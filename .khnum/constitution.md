# Constituição de Desenvolvimento Khnum

## Fonte de verdade

- A especificação da feature em `specs/` é a fonte de verdade do comportamento.
- Não implemente comportamentos que não estejam documentados.
- Resolva contradições entre especificação, contrato, modelo de dados e testes antes da implementação.

## Arquitetura

- `Khnum.Domain` não possui dependência de ASP.NET Core, Entity Framework Core, SQLite ou pacotes de infraestrutura.
- `Khnum.Application` coordena os casos de uso e define as portas de que precisa.
- `Khnum.Infrastructure` implementa as portas da Application e contém os detalhes de persistência e tecnologia.
- `Khnum.Api` é responsável apenas por HTTP, serialização, códigos de status e composição.
- DTOs HTTP não são reutilizados como comandos da Application.

## Desenvolvimento

- Toda feature começa com uma pasta em `specs/`.
- Toda mudança de comportamento possui um teste automatizado.
- Prefira uma fatia vertical a abstrações compartilhadas especulativas.
- Mantenha os contratos públicos estáveis e documente as mudanças intencionais.

## Segurança

- Senhas em texto plano existem somente durante o processamento da requisição.
- Nunca registre senhas, hashes de senha, tokens de sessão ou dados sensíveis desnecessários.
- Falhas de autenticação por dados inválidos, usuário inativo ou credenciais incorretas não devem revelar qual condição ocorreu.
- Um token de sessão só é retornado depois que sua persistência for concluída com sucesso.

## Validação obrigatória

Todo prompt que modificar código, testes ou documentação deve executar
`./tools/khnum.ps1 verify` antes de concluir.

Prompts somente de análise devem informar que a execução foi omitida por não alterarem arquivos.

## Conclusão

Uma feature só está concluída quando sua especificação, implementação, testes, verificações de arquitetura e verificações de segurança estão alinhados e passam em `tools/khnum.ps1 verify`.
