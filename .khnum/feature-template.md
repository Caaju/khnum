# Template de Feature

Crie uma pasta em `specs/feature-<name>/` contendo:

```text
feature.yaml
<feature>.spec.md
docs/
acceptance/
```

O manifesto deve definir:

```yaml
id: feature-name
name: Human readable name
status: draft
boundedContext: Context name
entrypoint: HTTP or application entry point
projects: []
tests:
  domain: []
  application: []
  integration: []
security: []
```

Uma feature só pode passar para `approved` depois que suas decisões abertas forem registradas em `.khnum/decisions/` ou resolvidas na especificação da feature.
