# Princípios arquiteturais

## Separação de responsabilidades

Cada componente deve ter uma razão principal para mudar:

- a API muda por contrato ou protocolo externo;
- a Application muda por fluxo de caso de uso;
- o Domain muda por regra de negócio;
- a Infrastructure muda por tecnologia ou integração.

## Inversão de dependência

A camada que coordena um caso de uso define as abstrações de que precisa. Adaptadores concretos implementam essas abstrações na Infrastructure. A composição ocorre na borda da aplicação.

## Portas pequenas

Interfaces devem representar uma necessidade real de um caso de uso. Evite repositórios genéricos, interfaces com operações não utilizadas e abstrações criadas apenas por antecipação.

## Domínio protegido

Modelos de domínio não devem carregar detalhes de serialização, persistência, logging ou transporte. Quando uma tecnologia impõe uma restrição, o impacto deve ser contido no adaptador ou no mapeamento.

## Testabilidade

- regras de domínio devem ser testáveis sem infraestrutura;
- casos de uso devem aceitar dublês das portas externas;
- testes de integração devem verificar a composição real e os adaptadores relevantes;
- testes de contrato devem proteger interfaces externas estáveis.

## Pragmatismo

Não introduzir CQRS, mediadores, barramento de eventos, microsserviços ou abstrações genéricas sem uma necessidade observável. A arquitetura deve acompanhar a complexidade do domínio.

## Observabilidade e segurança

Logs, métricas e tracing devem ser adicionados nas bordas e nos casos de uso sem expor segredos, credenciais ou dados sensíveis. Falhas externas devem ser traduzidas para contratos estáveis e não devem revelar detalhes de implementação.

## Decisões arquiteturais

Decisões que afetem toda a solução devem ser registradas como ADR em `.khnum/decisions/`, contendo:

- contexto e problema;
- decisão tomada;
- alternativas consideradas;
- consequências;
- data e status.
