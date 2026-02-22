# Contributing

Thank you for your interest in contributing to `Raccoon.Extensions.OpenTelemetry`.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later
- A Git client

## Getting Started

1. Fork and clone the repository.
2. Install the Git hooks (see below).
3. Restore dependencies: `dotnet restore`
4. Build: `dotnet build`
5. Run all tests: `dotnet test`

## Installing Git Hooks

The `hooks/` folder contains Git hooks that enforce code quality before changes leave your machine. Install them by pointing Git at the folder:

```bash
git config core.hooksPath hooks
```
(or copy the files to `.git/hooks/` directly)

This enables two hooks:

| Hook          | What it does                                                                                                                                                                                                    |
|---------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| `post-commit` | Runs `dotnet format` on changed `.cs` files and reports any formatting drift. Non-blocking.<br/>This allows to create another commit with just the linting changes, making it easy to track meaningful changes. |
| `pre-push`    | Runs `dotnet format --verify-no-changes` and `dotnet test`. Blocks the push if either fails.                                                                                                                    |

> The pipeline runs dotnet format and dotnet test automatically. You don't NEED to enable the hooks, but they can help you catch formatting issues early, before the workflow breaks. :) 

## Project Structure

```
src/         Library packages (the NuGet packages)
src/tests/   Unit tests (one test project per library)
src_demo/    Demo applications (not published to NuGet)
hooks/       Git hooks
```

- Library packages live under `src/` and test projects under `src/tests/`.
- Each test project mirrors its corresponding library project name with a `.Tests` suffix.
- Demo applications under `src_demo/` are not packable and exist only for manual integration testing. If adding a new feature, please add it to the demo application as well so it's easy to test it in different scenarios.

## Code Style

This project uses an `.editorconfig` that enforces formatting rules. 
The CI pipeline runs `dotnet format --verify-no-changes` and will reject PRs that don't comply. Run `dotnet format` locally before pushing.

Key conventions:

- **File-scoped namespaces** for all `.cs` files.
- **Braces required** on all blocks, even single-line.
- **LF line endings**, 4-space indentation for `.cs` files.
- **One public type per file.**
- **Primary constructors** when the constructor only assigns parameters to fields.
- **Records** for types that only hold data.
- **Immutable properties** (`init` over `set`) whenever possible.
- **Collection initializers** over `new List<T> { ... }` statements.
- **LINQ method syntax only** (`Where`, `Select`, etc.). LINQ query syntax is not used.

### Naming

- `PascalCase` for public types and members; `camelCase` for locals and parameters.
- `_camelCase` for private instance fields; `s_camelCase` for private static fields.
- Async methods end with `Async`.
- Interfaces are prefixed with `I`; type parameters with `T`.

## Writing Code

- Follow existing patterns in the repository. Match the style of the surrounding code.
- Keep classes and methods small and focused (Single Responsibility Principle).
- Prefer readability to cleverness.
- Validate inputs early. Use guard methods like `ArgumentException.ThrowIfNullOrWhiteSpace` at the top of methods.
- Use structured logging with message templates and named placeholders. Never use string interpolation inside log messages.
- Every public class, record, interface, and method must have XML documentation comments (`/// <summary>`).
- Extension methods live inside an `Extensions` folder within their project.

## Testing

- Framework: **xUnit** with native `Assert` methods.
- All tests follow **AAA** (Arrange, Act, Assert) with section comments.
- Test names follow the pattern: `{Method}_{Action}_{ExpectedResult}`.
- Group related test cases into `[Theory]` with `TheoryData<T>` generators.
- Test helpers live in a `#region Test Helpers` block at the end of the test class.
- Aim for at least 90% coverage on new code.

Run the full test suite before submitting a PR:

```bash
dotnet test
```

## Submitting Changes

1. Create a feature branch from `master`.
2. Make your changes in small, focused commits.
3. Ensure `dotnet format --verify-no-changes` passes.
4. Ensure `dotnet test` passes.
5. Open a pull request against `master`.

The CI pipeline will run formatting checks, tests, and SonarCloud analysis on your PR.

## License

By contributing, you agree that your contributions will be licensed under the [MIT License](license.md).
