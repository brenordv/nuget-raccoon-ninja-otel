---
name: csharp
description: Always use this whenever you need to read, understand, modify, update, create or change anything in the repository.
argument-hint: "Prompt with instructions of what to do"
disable-model-invocation: false
---

# C# Skill
## Prime Directives
- Treat the repository as the source of truth; match existing patterns and structure.
- Follow `.editorconfig` strictly, even when other guidance conflicts.
- Minimize diffs; change only what is necessary.
- Apply SOLID:
  - Single responsibility principle: keep classes small and focused. they should only do one thing, and do it well.
  - Open/closed principle: classes should be open for extension, but closed for modification.
  - Liskov substitution principle: derived classes must be substitutable for their base classes.
  - Interface segregation principle: clients should not be forced to depend on methods they do not use.
  - Dependency inversion principle: high-level modules should not depend on low-level modules; both should depend on abstractions.
    - This also applies to properties: If a data type has an interface/abstraction, use the most appropriate one for the use case.
- Apply DRY: Don't Repeat Yourself. Refactor when you see repeated code to improve maintainability, readability, and testability.
- Always consider YAGNI: You Aren't Gonna Need It. Don't add code unless you need it. 
- Avoid speculative abstractions.
- Prefer design patterns already present in the repo; do not introduce new ones unless required.
- Any new code should always follow the prime directives, be testable, and have well-thought-out unit tests.
- Before start working on any task, you must:
    1. Understand the codebase, focusing on the context/goal provided by the user.
- Before handing off your work, you must:
    1. Run `dotnet format` and fix any reported problems.
    2. Make sure the code is changed, and the new use-case is covered by unit tests.
    3. If there are no tests, create test coverage for it to improve repository maintainability.
    4. Run `dotnet test` to make sure you didn't break anything.
    5. Summarize the change to the user and report any problems, caveats, or warnings with the code change. In the summary, include the skills that were used to solve the request.
- Avoid using reflection, since they drastically reduce code readability and create "magical" behavior in the code.
- Code projects should live under `src/`, and test projects under `src/tests/`.
- Imperative that nullable reference types are disabled in all projects.
- Do not use `dynamic` type. If that's the absolute only option, stop, explain to the user what is going on, and your reasoning to think this is the only option, and ask the user for insights.
- When dealing with nullable types, prefer safe guarding and asserting you have all you need instead of multiple null-coalescing checks during the execution of the method. Think of efficiency and easy to read/maintain the code after it is done.

## Formatting & Style (Golden Rule)
- Obey `.editorconfig` without exceptions.
- Match the surrounding code style exactly.
- Use file-scoped namespaces when the repo does; keep usings outside namespaces.
- Always use braces, even for single-line blocks.
- Never use `#region` / `#endregion`, unless you are creating helpers for test files. In this case, you can create a `#region` called `Test Helpers` at the end of the test class and centralize the helpers there.
- When the constructor is only assigning the value of variables passed via argument to private instances, use the Primary Constructor instead.
- When creating code, organize it in a way that improves readability and reduce nesting.
- While the use of LINQ is allowed, you are forbidden from using LINQ Query. To reduce complexity, and improve readability, only LINQ Methods (Where, Select, GroupBy, etc.) are allowed.
- When chaining methods, place each method on a new line. Example:
```csharp
products.Where(p => p.IsActive)
        .Select(p => p.Price)
        .ToList();
```
- Only one public class/record/enum/struct, etc. per file.
- Implement nullable only when necessary.
- Whenever possible, prefer to use collection initialization instead of `new {}` statements.
- Whenever instantiating an object, and right after setting values to its properties, use object initializer and do it all at once.
- When creating a class that will only hold data, prefer to use a record instead of a class.
- Properties inside class/record/struct should be readonly when possible, and preferably init instead of set. Immutability is a key factor in maintainability.
- Classes holding extension methods should live inside an `Extensions` folder.

## Naming & Conventions
- Async methods must end with `Async`.
- Respect naming conventions from `.editorconfig` and existing code.
- Use explicit types or `var` based on `.editorconfig` and local patterns.
- Use PascalCase for public types and members; camelCase for locals and parameters.
- Prefix interfaces with `I` and attribute types with `Attribute`.
- Enum names are singular; `[Flags]` enums are plural.
- Private instance fields use `_camelCase`; private static fields use `s_camelCase`; private static readonly fiels use`PascalCase`.
- Prefix type parameters with `T` (`T`, `TModel`, `TRequest`).
- Avoid consecutive underscores in identifiers.
- Avoid obscure abbreviations; prefer clear, descriptive names.
- Avoid single-letter names except for tight, obvious loop counters.
- Use `@`-prefixed identifiers only for interop with reserved keywords.

## Language & Runtime Practices
- Prefer readability over cleverness.
- Use modern C# syntax when it matches repo style (pattern matching, `is null`, `using var`).
- Pass `CancellationToken` through call chains when available.
- Use `ValueTask<T>` only for hot paths that frequently complete synchronously.
- The use of `ConfigureAwait` is forbidden. Async code should be awaited by using `await`.
- Validate the data the method requires as early as possible, especially when a datatype is nullable, to avoid multiple null-coalescing checks in the method.
- If the validation is complex (more than a single check) or done in multiple places, follow the logic below:
    - If the validation is only useful for that specific class/service, create a private validation method.
    - If the validation depends on a single variable, and is generic enough to be used on the whole system, create a validation method in  the most appropriate custom exception, following patterns like `ArgumentException.ThrowIfNullOrWhiteSpace`.
    - Otherwise, create an extension method, based on the most appropriate variable involved in the validation, adding unit tests to make sure the validation works.
- When a method that returns a list fails, do not return `null`, instead prefer returning an empty list.

## Exceptions & Error Handling
- Discover the repository’s base exception type and use it for custom exceptions.
- If no base exception exists, introduce a domain-appropriate base and inherit from it.
- Avoid throwing or catching `Exception`; catch specific exceptions and recover when possible.
- If the repo provides error helpers (for example, `LogAndProcessError`), use them consistently.

## Serialization
- If the repository has `Newtonsoft.Json`, then use it as a source of truth, and avoid using other packages. Otherwise, use `System.Text.Json`.

## Logging
- Always use structured logging with message templates and named placeholders.
- Never use string interpolation inside log messages.
- Use logging scopes when available to enrich context.
- Log messages should be concise but meaningful. Indicating which step of the flow failed, and include available data to facilitade debugging.
- When adding a nullable variable to the log message template, include braces around it, to facilitate the visualization of null/empty variables. Example: `User Name: [{UserName}]`.
- Avoid making the code too verbose by adding too much `LogInformation` entries. Add them when they provide meaningful insights to the flow.
- Check if there's an Error Handling Middleware that logs the errors and avoids double-logging an exception.
    - If the method where the error happened has more details to include in the log message, create an error log with the relevant data.
- Avoid generic error log messages. (I.E: `An error has happened: {ExceptionMessage}`); The error messages must be meangful and provide insights on what happened.

## Refit External Calls (Default Pattern)
- Define Refit interfaces under the API clients area and use Refit attributes for routes, bodies, and parameters.
- Register all Refit clients in a single extension method, and wire them in `Program.cs` via `RegisterRefitClients`.
    - If the repository already have an initialization method with a different name, use that instead.
- Always configure each client with `BaseAddress` from configuration and reuse the shared `ConfigureRefitClient` policy setup.
- Use client-credentials authentication via the existing profiles when calling external services.
    - Prefer using the NuGet package `Borrowell.Authentication.Jwt.ClientCredentials` to configure authentication.
    - If the repository doesn't have this package installed/configured, create an extension method to configure authentication in a centralized place.
- Apply the standard retry policies: unauthorized retry plus transient HTTP error policy.
- Configure request timeouts and shared message handlers centrally when the repo provides them.
- Prefer typed `ApiResponse<T>` when you need to access the body/content returned; If all you need is the status codes and headers, use `IApiResponse` instead.
- Always validate responses with custom methods in the repository, if available. Otherwise, use `EnsureSuccessStatusCode`.
- Log failures with structured logging and rethrow a domain-appropriate exception.

## Global Error Handling
When an Error Handling Middleware is present:
- Use single error-handling middleware to capture exceptions and return `ProblemDetails` (dotnet native).
    - Do not include technical details in production. For production only a meaningful error message should be returned.
    - In case of doubt of which environment is being targeted, assume it is production.
- Check `HttpResponse.HasStarted` before writing any error response.
- Map known exception types to consistent status codes and titles.
- Log errors with structured context (method, path, client identifiers) before responding.

## Configuration Validation
- Validate configuration at startup with Options `Validate` + `ValidateOnStart`.
- Use reusable validation helpers and clear failure messages.
- Fail fast on invalid configuration; do not defer config errors to runtime.
- All variables read from settings files (`appsettings.json`, `local.settings.json`, etc.) must be validating during startup, as soon as possible, and if missing/invalid, 
follow the repositories pattern to propagate a meaningful error message that will make it clear what is wrong. 
- If the repository doesn't have a pattern, throw a domain-appropriate exception instead.

## Dependency Injection Organization
- Register services via extension methods grouped by concern (DI, AutoMapper, HTTP clients, etc.).
- Keep `Program.cs` minimal by delegating setup to these extensions.
- Choose explicit lifetimes (`Singleton`, `Scoped`, `Transient`) based on behavior.

## Documentation Comments (Summary Required)
- Every new public class, interface, struct, record, and method must have XML documentation comments.
- Every new internal/private methods or classes should be documented unless the repo explicitly avoids it.
- Use `/// <summary>` on types and members; keep it concise and action-oriented.
- Use `<see cref="..."/>` when referencing other types or members in the summary.
- For orchestration methods, write a high-level overview in the orchestrator summary and put step detail in the called methods.
- Use `<param>` for every parameter and `<returns>` for non-`void` methods.
- Use `<exception cref="...">` for exceptions the method may throw and document the condition.
- Use `<remarks>` for important constraints or non-obvious behavior.
- Keep XML well-formed and place comments directly above the declaration (and above attributes).

## Architecture & Design
- Keep methods and classes focused; refactor when they get too large.
- Prefer interfaces to concretions and keep interfaces small and focused.
- Favor composition; avoid deep inheritance chains.
- Create domain-specific folders for models, requests, responses, and constants when adding new domains.
- Prefer extension methods over helper classes; document extension methods with summaries.

## Testing
- Use xUnit and native `Assert` methods.
- Tests must always follow AAA: Arrange, Act, Assert.
- The comments defining the AAA areas should be capitalized and with an space between the comment slashes and the word. Example: `// Arange`
- Before creating a unit tests method:
  - Understand the code you want to test.
  - Understand the flow that involves that code: When it is used, why, and for what purpose.
  - Plan accordingly to cover happy path, sad/broken path, and edge cases, but keep it grounded and avoid extreme edge cases.
- When planning the tests you are going to create, be careful and think things through to avoid creating unit tests that are duplicated that differ only in the test data.
- Group related facts into `[Theory]` with `TheoryData<T>` generators in `TheoryDataGenerator`.
- Use realistic data via `Faker` in `MockDataGenerators`.
- Service tests should have a `BuildSut` helper under a `Test Helpers` region, at the end of the file.
- Mirror production folder structure in test projects; tests live in `*.Tests`.
- If neither `TheoryDataGenerator`, and `MockDataGenerators` exist in the project, place the generator methods under a `Test Helpers` region, at the end of the file.
- The test names should be descriptive, action-oriented, concise, and follow the pattern: `{Method being tested}_{Action being performed}_{Expected result}`.
- Test coverage should be kept at least 90%, save if the ROI is too low. In this case, consider refactoring the code to make it more testable. If nothing else is possible, add a comment explaining why the test coverage is not what is expected.
- The name of test projects should be the same as the project they test, with the suffix `.Tests`.
    - The same applies to test files, which should be named the same as the tested file, with the suffix `Tests.cs`.
- When using a hardcoded test value more than once, create a constant with a meaningful name instead of repeating it multiple times.
  - If the same value is used in multiple tests, create a `const` field in the test class, and reference it from the tests. Otherwise, create a `const` field in the tested class.

## NuGet packages
- Use the latest stable version of each package.
- Always use transient dependencies and avoid adding the same dependency on multiple projects, unless necessary. 
  - On that note, pay attention to the repercussions of moving a package to a more central project, if it doesn't make sense, leave a note on the csproj file explaining why.
  - The only exception to this rule is the following packages: `coverlet.collector`, and `Microsoft.NET.Test.Sdk`. They need to be present on all test projects for the pipelines to work properly.

## Web API Conventions (When Applicable)
- Controller actions return `IActionResult`.
- If the API is mature enough to contain an error handling middleware, controllers will orchestrate only; no business logic and no `try/catch`.
    - If no error handling middleware is found, then add a `try/catch` to the endpoint method in a consistent way with other existing endpoints.
- Validate early in controllers; return `400 Bad Request` as soon as possible.
- `GET` returns `200 OK` with data or `204 No Content` when empty.
- `POST` returns `201 Created` with data.
- Do not return status codes inside response bodies.

## Analyzer & Tooling Expectations
- Respect analyzer severities configured in `.editorconfig`.
- All projects must use the latest stable version of `Roslynator.Analyzers`.

## AI Guardrails
- Follow existing repository patterns before introducing new abstractions.
- Avoid unnecessary changes outside the scope of the request.
- If a rule conflicts, the order of precedence is:
    1. Repository conventions and `.editorconfig`
    2. Local file patterns
    3. This skill
