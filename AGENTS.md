# AGENTS — AI Coding Agent Instructions

Purpose: Provide concise, actionable guidance so AI coding agents are productive immediately.

## Quick Commands
- Build solution: `dotnet build CloudAccounting.slnx`
- Run web app (development): `dotnet run --project CloudAccounting.Web`
- Run unit tests: `dotnet test CloudAccounting.UnitTests/CloudAccounting.UnitTests.csproj`
- Run integration tests: `dotnet test CloudAccounting.IntegrationTests/CloudAccounting.IntegrationTests.csproj` (may require test DB/setup; see README)

## Key Projects & Locations
- Cloud host / entrypoint: CloudAccounting.Web/
- Application layer (use cases, handlers): CloudAccounting.Application/ (see `UseCases/`)
- Domain models & exceptions: CloudAccounting.Core/
- Shared DTOs/utilities: CloudAccounting.Shared/ and CloudAccounting.SharedKernel/
- Integration tests and test fixtures: CloudAccounting.IntegrationTests/

## Conventions & Notes
- Use case handlers are organized by feature under `CloudAccounting.Application/UseCases/`.
- Pipeline behaviors live in `CloudAccounting.Application/Behaviors/`.
- Mapping configs use Mapster in `CloudAccounting.Application/Mappings/`.
- Tests follow typical xUnit/dotnet conventions; run `dotnet test` per-project.

## When To Load These Instructions
- Use these instructions for tasks that require understanding project structure, build/test commands, or where to place new features.
- For file-level guidance (linting, formatting, or specific applyTo globs), prefer creating `*.instructions.md` scoped to the relevant path.

## Useful Links
- Project README: [README.md](README.md)

If you want, I can also add a `.github/copilot-instructions.md` variant or scoped `*.instructions.md` files for specific subsystems (API, tests, migrations).
