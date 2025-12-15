<!-- Copilot / AI assistant instructions tailored to the `mass` repo -->
# Guidance for AI coding agents

Short, actionable notes to help you be productive in this repository.

- **Big picture:** MASS is a "Multi Author Story System".  It lets users work together to write a story. This is a Blazor server-style app using Interactive Razor Components (see `Program.cs`). Persists application state with EF Core + Npgsql (Postgres) in `MassDbContext` (see `Data/ApplicationDbContext.cs`). Identity is customized (`MassIdentityUser`, `MassApplicationRole`, `MassMassIdentityUserRole`) and integrated with Razor UI under `Components/Account`.

- **Startup & config:** `Program.cs` configures services and requirements. IMPORTANT: the app reads the connection string from the `DB_CONNECTION` environment variable and will throw if it is missing. Use a Postgres connection string like `Host=...;Database=...;Username=...;Password=...`.

- **Database & migrations:** Uses `dotnet-ef` and Npgsql with snake_case naming via `.UseSnakeCaseNamingConvention()`.
  - Add migrations: `dotnet ef migrations add <Name>`
  - Apply: `dotnet ef database update`
  - DataProtection key storage is persisted to the DB (`DataProtectionKeys` DbSet).

- **Domain model patterns:** Entities in `Data/` (e.g., `Story.cs`, `Chapter.cs`, `Entry.cs`) use `required virtual` navigation properties and set `CreatedAt`/`LastModifiedAt` defaults (consistent pattern across models).

- **Identity / Auth specifics:** Identity is configured with custom user/role types; the join entity is named `MassMassIdentityUserRole` and a composite key is configured in `OnModelCreating`. Redirects and status messages are handled by `IdentityRedirectManager` and a short-lived cookie named `Identity.StatusMessage`.

- **Email & UX clues:** An in-repo no-op email sender (`IdentityNoOpEmailSender`) is registered as `IEmailSender<MassIdentityUser>`; search `RegisterConfirmation.razor` for the special-case note that you should remove the no-op block if adding a real provider.

- **Logging & runtime:** Logging uses simple console formatting with timestamps (configured in `Program.cs`). App runs on .NET 10 (project targets `net10.0`).

- **Dev workflow (quick):**
  - `dotnet restore`
  - `dotnet build` (or run the VS Code `build` task)
  - `DB_CONNECTION='<conn>' dotnet run`
  - For iterative dev: `dotnet watch run` (there is a `watch` task configured)

- **Where to look for common tasks:**
  - Startup and DI: `Program.cs`
  - DB model and migrations: `Data/` and `Data/Migrations/`
  - Identity UI and helpers: `Components/Account/`
  - Static assets/locales: `wwwroot/` (contains many locale folders)

- **Conventions & gotchas to follow:**
  - Keep using `required virtual` for navigations to align with existing models.
  - Use `DateTime.UtcNow` initializers consistent with current code (models currently use `DateTime.UtcNow` assigned to `DateTimeOffset`). Do not change without a repo-wide plan.
  - Remember the app exits early if `DB_CONNECTION` is missing; ensure migrations and local DB availability during testing.

- **Tests:** There are no unit/integration tests in the repo currently. If you add tests, keep them in a separate test project and reference the main project.

If anything here is unclear or you'd like specific examples (e.g., how to add a migration and seed data, or the exact Register confirmation flow), tell me what to expand and I'll update this file.
