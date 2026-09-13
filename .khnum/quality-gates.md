# Critérios de Qualidade

## Sempre obrigatórios

- A especificação existe e identifica o ponto de entrada externo.
- O manifesto da feature existe e lista os projetos afetados e os testes necessários.
- As regras de Domain e Application possuem testes isolados.
- O comportamento de integração possui um teste quando envolve persistência ou HTTP.
- O Domain não possui dependências para Api ou Infrastructure.
- Valores sensíveis não aparecem em logs nem em respostas de erro.

## Gates específicos do login

- A entrada inválida é rejeitada antes da consulta ao repositório.
- Usuários inativos e inexistentes possuem a mesma resposta externa de falha que uma senha incorreta.
- Uma falha de autenticação não cria sessão.
- Uma resposta de sucesso contém o token de sessão persistido.
- A expiração da sessão ocorre depois do horário de criação.
- As chaves estrangeiras do SQLite e as restrições de e-mail único estão habilitadas.
- O token retornado é um UUID v4.

## Ordem de verificação

1. Validar o harness e os metadados da feature.
2. Restaurar e compilar a solução.
3. Executar os testes unitários e de integração.
4. Verificar a formatação e as regras de dependência entre projetos.
5. Executar as verificações de segurança e contrato.
