# ADR 0003: Limite de Persistência da Sessão

- Status: aceito
- Data: 2026-09-13

## Decisão

O caso de uso de login retorna um token somente depois que o repositório de sessões informar que a persistência foi concluída com sucesso. O comportamento transacional do Entity Framework Core permanece na Infrastructure; a Application não referencia `DbContext` nem controla transações do SQLite diretamente.

## Consequências

Uma falha de persistência não produz uma resposta de login bem-sucedida. A implementação do repositório e os testes de integração devem provar que o token retornado é o mesmo token armazenado no SQLite.
