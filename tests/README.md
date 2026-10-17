# Testes do Sienna

Framework: **MSTest 4**, executado pelo Microsoft.Testing.Platform (veja o `global.json` na raiz).
Dublês: **NSubstitute**.

## Projetos

| Projeto | Testa | Depende de |
|---|---|---|
| `Sienna.Tests.Common` | — (apoio: builders, asserções, valores) | Domain |
| `Sienna.Domain.UnitTests` | Entidades, regras e cálculos do domínio | Domain |
| `Sienna.Application.UnitTests` | Handlers, serviços e cálculos da Application | Application |

## Rodar

Todos os testes da solução:

```bash
dotnet test
```

Um projeto:

```bash
dotnet test --project tests/Sienna.Application.UnitTests
```

Só uma classe:

```bash
dotnet test --project tests/Sienna.Application.UnitTests --filter "FullyQualifiedName~CampaignInsightsCalculator"
```

## Convenções

- **Pastas e namespaces espelham o código testado:**
  `Sienna.Application/UseCases/Social/Instagram/GetCampaignInsights/X.cs` →
  `tests/Sienna.Application.UnitTests/UseCases/Social/Instagram/GetCampaignInsights/XTests.cs`
- **Classe:** `[TestClass] public sealed class <ClasseTestada>Tests`. **Método:** `[TestMethod] Metodo_Cenario_ResultadoEsperado`.
- **Vários casos do mesmo teste:** `[TestMethod]` + `[DataRow(...)]`.
- **Corpo:** arrange / act / assert, separados por uma linha em branco.
- **Results:** `ResultAssert.Success(...)` e `ResultAssert.Failure(result, "Codigo.Do.Erro", ErrorType.X)`.
- **Coleções:** `Assert.AreSequenceEqual(esperado, atual)`, `Assert.IsEmpty(...)`, `Assert.ContainsSingle(...)`.
- **Datas:** `TestTime.Now` e `TestTime.Brasilia(...)`, não `DateTime.UtcNow`.
- **Dados:** builders de `Sienna.Tests.Common/Builders`. Se um objeto é montado em mais de um teste, crie um builder.
- **Dublês:** NSubstitute. Dublês reutilizados por uma área ficam juntos (ex.: `InstagramMocks`).
- **Cancelamento:** receba o `TestContext` no construtor e passe `testContext.CancellationToken` para métodos assíncronos.
- **Paralelismo:** os testes rodam em paralelo (`MSTestSettings.cs`). Não use estado estático mutável.

## Adicionar um projeto de teste

1. Crie `tests/Sienna.<Camada>.<Tipo>Tests/Sienna.<Camada>.<Tipo>Tests.csproj` referenciando só a camada testada.
   Todo projeto cujo nome termina em `Tests` recebe sozinho o MSTest, o NSubstitute, o `Tests.Common` e os usings.
2. Copie o `MSTestSettings.cs`.
3. Coloque as versões de pacotes novos em `Directory.Packages.props`.
4. Adicione o projeto à pasta `tests` do `Sienna.slnx`.

## Próximos passos

- **`Sienna.Infrastructure.IntegrationTests`:** repositórios com Postgres real (pacote `Testcontainers.PostgreSql`).
  Exemplos: `GetDueForInsightsAsync`, `GetCampaignInsightDataAsync`.
- **`Sienna.WebApi.IntegrationTests`:** endpoints de ponta a ponta com `Microsoft.AspNetCore.Mvc.Testing`.
  Exige `public partial class Program;` no WebApi e um banco de testes. Nunca use o banco do Render,
  porque o Debug roda a migração e os workers.
- **Cobertura:** adicione o pacote `Microsoft.Testing.Extensions.CodeCoverage` e rode `dotnet test --coverage`.
