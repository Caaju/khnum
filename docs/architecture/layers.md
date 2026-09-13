# Camadas e dependências

## Domain

O Domain contém o modelo de negócio e as regras que devem permanecer válidas independentemente de HTTP, persistência, mensageria ou framework.

Pode conter:

- entidades e agregados;
- objetos de valor;
- serviços de domínio quando uma regra não pertencer a uma entidade;
- eventos e erros de domínio;
- invariantes e políticas puramente de negócio.

O Domain não referencia projetos de API, Application, Infrastructure ou bibliotecas de infraestrutura.

## Application

A Application contém os casos de uso e coordena a execução do negócio.

Pode conter:

- comandos, consultas e resultados;
- handlers ou serviços de aplicação;
- validação de entrada do caso de uso;
- portas para persistência, tempo, identidade e serviços externos;
- transações e unidade de trabalho no limite do caso de uso.

A Application pode depender do Domain, mas não deve conhecer HTTP, banco concreto, ORM ou configuração de ambiente.

## Infrastructure

A Infrastructure implementa as portas exigidas pela Application e concentra detalhes técnicos.

Pode conter:

- persistência e mapeamentos;
- clientes de serviços externos;
- mensageria;
- armazenamento de arquivos;
- relógio e geradores concretos;
- registro de dependências.

A Infrastructure não deve introduzir regras de negócio para compensar uma deficiência do domínio ou do caso de uso.

## Api

A Api adapta protocolos externos para os casos de uso da Application.

Pode conter:

- endpoints, controllers ou consumers;
- DTOs de entrada e saída;
- autenticação do protocolo;
- serialização e códigos de resposta;
- middleware e observabilidade de borda;
- composição da aplicação.

A Api não acessa repositórios, DbContext ou serviços externos diretamente para executar negócio.

## Direção das dependências

| Projeto | Pode depender de | Responsabilidade |
|---|---|---|
| `Domain` | Nenhum projeto da solução | Regras e modelo de negócio |
| `Application` | `Domain` | Casos de uso e portas |
| `Infrastructure` | `Application`, `Domain` | Adaptadores técnicos |
| `Api` | `Application` e composição de `Infrastructure` | Protocolo e entrada da aplicação |

Dependências entre módulos do mesmo nível devem ser evitadas. Quando dois módulos precisam conversar, prefira uma porta explícita no módulo consumidor ou um contrato compartilhado pequeno e estável.
