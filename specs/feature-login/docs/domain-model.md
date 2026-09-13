# Modelo de domínio

## Bounded context

O serviço pertence ao contexto delimitado de **Autenticação e Sessão**. Seu modelo é responsável por validar credenciais e criar sessões; cadastro, recuperação de senha, autorização e gestão de perfis ficam fora deste contexto.

## Entidades

### Login

Representa uma identidade que pode autenticar na aplicação.

A entidade possui:

- `Id`: UUID persistido como texto.
- `Email`: objeto de valor normalizado e validado.
- `PasswordHash`: hash da senha.
- `IsActive`: indica se a autenticação está permitida.
- `CreatedAtUtc` e `UpdatedAtUtc`.

Invariantes:

- O e-mail é obrigatório e único.
- A senha armazenada é sempre um hash.
- Um login inativo não pode criar sessão.

### LoginSession

Representa uma sessão autenticada.

A entidade possui:

- `Token`: UUID único da sessão.
- `LoginId`: referência ao `Login` autenticado.
- `CreatedAtUtc` e `ExpiresAtUtc`.
- `IsRevoked`.

Invariantes:

- Uma sessão sempre pertence a um login existente.
- O token é único.
- `ExpiresAtUtc` deve ser posterior a `CreatedAtUtc`.
- Uma sessão revogada não é uma sessão ativa.

### Email

Objeto de valor que encapsula o e-mail do usuário.

Responsabilidades:

- Rejeitar valor nulo, vazio ou em formato inválido.
- Normalizar o valor para comparação e persistência.
- Comparar valores sem depender da camada HTTP.

## Serviços e portas

A verificação de senha é uma dependência externa ao modelo de entidades e deve ser representada pela porta `IPasswordVerifier` na Application. O domínio não deve conhecer o algoritmo utilizado.

Portas principais:

```text
ILoginRepository
  FindActiveByEmailAsync(email)

ILoginSessionRepository
  AddAsync(session)

IPasswordVerifier
  Verify(plainTextPassword, passwordHash)

IClock
  UtcNow
```

## Caso de uso Login

1. Validar o comando de entrada.
2. Buscar login ativo pelo e-mail normalizado.
3. Retornar falha genérica se o login não existir ou estiver inativo.
4. Verificar a senha usando `IPasswordVerifier`.
5. Retornar a mesma falha genérica se a senha não corresponder.
6. Criar `LoginSession` com UUID e expiração configurada.
7. Persistir a sessão.
8. Retornar o token criado.

A busca deve considerar apenas registros ativos ou, alternativamente, garantir que o status seja verificado antes da criação da sessão. Em ambos os casos, usuário inexistente, inativo e senha incorreta devem produzir a mesma resposta externa.

## Segurança do domínio

- Senhas em texto plano existem somente durante o processamento da requisição.
- Logs não devem conter senha, hash, token ou dados sensíveis desnecessários.
- A mensagem de autenticação inválida deve ser: `Credenciais inválidas ou usuário inativo.`
- A sessão deve ser criada somente depois da verificação bem-sucedida.
