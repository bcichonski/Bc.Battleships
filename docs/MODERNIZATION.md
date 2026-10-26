# Bc.Battleships – modernization plan

Status as of 2026-10-21. The baseline is a .NET 6 + CRA/React 17 PoC from July 2022: 6 production projects, 3 test projects, 38 passing tests.

Each phase below is meant to be one PR (or a small series), with the build and tests green at every step.

---

## Phase 0 – Baseline (done)

- [x] Renamed everywhere: namespaces, projects, folders, `package.json`, history and commit messages
- [x] Stripped build output (`obj/`) that was accidentally committed, from the whole history
- [x] Build passes and the 38/38 tests pass (run on the .NET 8 runtime via roll-forward, since net6 is EOL)
- [x] Pushed to `github.com/bcichonski/Bc.Battleships` (private), MIT license

## Phase 1 – Repo hygiene and CI (low risk, no behaviour change) (#1)

- Flatten the `Bc.Battleships/Bc.Battleships/Bc.Battleships` nesting into `src/` + `tests/`
- `.sln` → `.slnx`
- Add `Directory.Build.props`: nullable on, `TreatWarningsAsErrors`, analyzers, `LangVersion latest`
- Add `Directory.Packages.props` for central package management (the test projects currently use mismatched xunit/Test.Sdk/coverlet versions)
- Add `global.json`, `.editorconfig`, `.gitattributes` (line endings: the repo has BOMs and CRLF)
- Rewrite the README: what the app is, architecture sketch, how to run (the current one has a stale `[...]BcBattleships\...` path)
- GitHub Actions: build, test, coverage (coverlet + reportgenerator), client lint/test. Add Dependabot.

## Phase 2 – Platform upgrade (keep behaviour identical) (#2) (done)

- [x] Runtime: all projects target **net10.0**; the `RollForward=Major` workaround is gone
- [x] EF Core 10.0.13 on SQLite. The 2022 `InitialMigration` is untouched and still applies to an empty database. `has-pending-model-changes` reported no changes, so no new migration was added (D3 outcome: none)
- [x] OpenAPI: Swashbuckle removed. `MapOpenApi()` serves `/openapi/v1.json` and Scalar serves `/scalar/v1`, both in Development only (D4)
- [x] SpaProxy removed per D5 (package, csproj properties, `DebugEnsureNodeEnv`, launchSettings env var, log category). Publish still builds the client into `wwwroot`
- [x] MVC removed (D6). `GameController` is replaced by the `GameEndpoints` minimal API group (`GET/POST/PUT /game`), with the JSON contract unchanged
- [x] Mediator 3 (source-generated) replaces MediatR 10
- [x] AwesomeAssertions replaces FluentAssertions
- [x] xunit v3 (`xunit.v3.mtp-off`, VSTest), with the test projects migrated
- [x] Guards use the BCL (`ArgumentNullException.ThrowIfNull` and friends) instead of `Ensure.That`
- [x] `Microsoft.AspNetCore.Http.Abstractions` removed from Domain through a bus-resolver seam (D7). The static `DomainEventBus.Instance` and the seam were removed in Phase 4a (#25); the domain events infrastructure stays, injected through DI
- [x] Tests: 63 pass (44 domain unit, 8 database integration, 11 API integration, 5 of them HTTP tests via `WebApplicationFactory`)

Deviations from the plan:

- `xunit.v3.mtp-off` is used instead of the Microsoft Testing Platform runner, to stay on VSTest.
- `Length`'s guard was a no-op under `Ensure.That`. It now throws `NegativeLengthValueException` on a negative value, which is a deliberate behaviour fix.
- The Vite proxy is deferred to Phase 3. Until then, the CRA `setupProxy.js` forwards `/game` in Development.

| Area | Now | Target |
|---|---|---|
| Runtime | net6.0 (EOL Nov 2024) | **net10.0** (LTS; net8 support ends Nov 2026) |
| EF Core | 6.0.6 | 10.x, keep SQLite |
| OpenAPI | Swashbuckle 6.3 | built-in `Microsoft.AspNetCore.OpenApi` + Scalar UI |
| SPA hosting | `Microsoft.AspNetCore.SpaProxy` 6 + CRA | Vite dev server with proxy; static files in prod |
| Domain deps | `Microsoft.AspNetCore.Http.Abstractions` **2.2** in Domain | removed (see Phase 4a: static event bus, removed in #25) |
| Mediator | MediatR 10 | **[Mediator](https://github.com/martinothamar/Mediator)** 3.x (MIT, source-generated, no reflection; MediatR ≥13 is commercially licensed). `IRequestHandler`/`INotificationHandler` port almost 1:1; handlers return `ValueTask`; registration via `AddMediator()` with compile-time handler discovery. |
| Assertions | FluentAssertions 6.7 | ⚠ FA ≥8 is commercial. Switch to **AwesomeAssertions** (drop-in fork) or Shouldly |
| Test framework | xunit 2.4 | xunit v3 |
| Guards | Ensure.That | BCL `ArgumentNullException.ThrowIfNull` and friends |
| Controllers | MVC `GameController` | Minimal API endpoint group |

Regenerate the EF migration only if the model changes; the 2022 migration still applies.

## Phase 3 – Frontend rewrite (small: ~650 LoC, most of it template boilerplate) (#3) (done)

- [x] CRA (deprecated) → **Vite + React 19 + TypeScript** (strict mode, `createRoot`)
- [x] Remove unused template dependencies: `oidc-client`, `jquery`, `reactstrap`, `react-router-bootstrap`, the whole `workbox-*` service-worker set, `lodash`, `merge`, `http-proxy-middleware`, `aspnetcore-https.js` / `aspnetcore-react.js`
- [x] Drop react-router entirely (single screen); Bootstrap CSS kept, no Bootstrap JS
- [x] Typed API client: hand-written (`/game` is only known to one module); the OpenAPI document was not used for generation
- [x] Fix: the new-game request is an explicit user action (no `newGame()` in render) and the page reload is gone. The state hook refreshes the board with `GET /game`
- [x] Vite dev server on `http://localhost:44463` proxies `/game` to the backend; the CRA `setupProxy.js` is removed
- [x] Tests: Vitest + Testing Library for components; **Playwright** e2e smoke (new game → shoot → hit/miss rendered → game over), run by the `e2e` CI job


## Phase 4 – Refactoring (where the real value is) (#4)

### Bugs found while reading the code

1. **Ships never occupy row 10 or column 10.** `FleetFactory.GetRandomCoordinates` uses `Random.Next(1, BoardSize)` and `Next(1, BoardSize - length)`, both with exclusive upper bounds on 1-based coordinates. Horizontal ships also never reach column 9. A player who knows this skips a whole row and column.
   → Fix the bounds and add a property-style test over many seeds checking that every cell is reachable.
2. **Fire-and-forget domain event through a static service locator.** **Fixed in Phase 4a (#25).** `Ship.EmitEventOnSinking` calls `DomainEventBus.Instance.Publish(...)` without awaiting it. `Instance` resolves `IBus` from `IHttpContextAccessor` statically. The handler then reloads the game on the *same scoped DbContext* while `ShootCommandHandler` is calling `SaveChangesAsync`, so this risks concurrent DbContext use, and "game over" may not be persisted.
   → Make game-over an invariant of the aggregate: `Game.Shoot` sets `GameState.Won` when total HP reaches 0. No event, bus, or static needed.
3. **Shooting an already-sunk compartment reports "Hit" again** and re-emits the sinking event. `GetCompartmentAt(c)?.Hit() != null` is true for any existing compartment, because `Hit()` returns a non-null `bool`.
   → Return `AlreadyHit` / `Hit` / `Sunk` / `Miss` explicitly.
4. **Placement can throw at random.** `FleetFactory` gives up after 9 attempts (`< 10`) and throws `FleetFactoryPositioningFailedException`.
   → Enumerate the valid placements, then pick one at random (deterministic termination).
5. Client-side: duplicate new-game POSTs. **Fixed in Phase 3**: starting a game is an explicit action that sends one `POST /game`, and the client no longer reloads the page.

### Design

- [x] **There is one global "current game" (done in #26).** The API has no game id, so two browsers share and fight over one game. Introduce resources: `POST /games` → id, `GET /games/{id}`, `POST /games/{id}/shots`. `PUT /game` for shooting is not REST-y anyway.
- [x] **Testability (done in #26).** Inject `Random`/seed (or a `TimeProvider`-style abstraction) into fleet placement; replace the static `GameContextProvider` with options.
- [x] **Remove the back-references (done in #25)** `Ship.Owner` and `ComputerPlayer.Game`. They exist only for the event plumbing in (2), and removing them simplifies the EF mapping a lot.
- [x] **Remove the `#pragma warning disable CS8618/CS8601` blocks (done in #26).** Use EF constructor binding or `= null!` on private ctors.
- [x] **Unit of work (done in #26).** `ShootCommandHandler` depended on `BattleshipsDbContext` directly → the repository/unit of work should own `SaveChanges`.
- [x] **`HumanPlayer` (done in #26): kept, no behaviour added.** It was created and then ignored by EF. Either delete it, or implement the computer shooting back (feature, Phase 5).
- [x] **Too many projects for ~1k LoC (done in #26).** Messages + Server → `Application`; `EFMigrationsHelper` → an `IDesignTimeDbContextFactory` inside Persistence. Target: `Domain`, `Application`, `Infrastructure`, `Api` (+ `web/`). **Deviation (D3):** the web project keeps the name `Bc.Battleships` and plays the Api role, and `ClientApp` stays inside it; renaming it and the `web/` move are a separate step. Persistence was renamed `Bc.Battleships.Infrastructure` and `EFMigrationsHelper` replaced by `BattleshipsDbContextFactory`.
- [x] **Strongly-typed ids (done in #26).** 6 near-identical `ValueConverter` classes → one generic convention in `ConfigureConventions`, or Vogen.
- [x] **Modern C# (done in #26).** File-scoped namespaces, primary constructors, records for value objects, collection expressions.

### Tests

- [x] Phase 2 added 5 `WebApplicationFactory` HTTP tests. Any API integration tests that still call Mediator directly with the bus **substituted** never go through HTTP and never exercise the sink → game-over path. Convert them to HTTP tests (done in #27).
- [x] Add regression tests for bugs 1–4 *before* fixing them (done in #4).
- [x] Coverage gate in CI: `scripts/coverage.sh` fails below 90 % line / 85 % branch (filtered; done in #27).

## Phase 5 – Optional / stretch (#5)

- Two-sided game (the computer shoots back, with a hunt/target AI)
- Dockerfile + GHCR image; optional .NET Aspire AppHost for local orchestration
- Deploy a public demo (Azure Container Apps / Fly.io) as a portfolio piece

---

## Recommended order

1 → 2 → (4: regression tests, then bug fixes) → 3 → 4 (design refactors) → 5.

Phases 1 and 2 are mechanical. Do Phase 4's bug fixes before the frontend rewrite, so the new UI is built against the corrected API shape (`/games/{id}`).

## Open decisions

- Keep it one-sided (as originally scoped) or make it two-sided
- Make the repo public after Phase 1 as a portfolio piece?
