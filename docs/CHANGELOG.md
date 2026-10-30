# Changelog

All notable changes to this project will be documented in this file.
Each entry corresponds to a completed story and is generated from the actual PR diff after
implementation — not from the spec plan.
Entries are ordered by merge date (newest first).

To add entries, run the `generate-changelog-entry` skill (or the `changelog-generator` agent).
It scans `docs/specs/plans/` for `FINAL` plans with no entry here yet, finds each one's merged
PR on GitHub, and drafts from the diff. The format for an entry is in
`docs/changelog-entry-template.md`.

---

## [5] Modernization phase 5: Docker containerization — 2026-10-29

Tags: `api` `docker` `ci` `tests`
PR: [#34](https://github.com/bcichonski/Bc.Battleships/pull/34)
Builds on: #3, #26

### What was built
- Multi-stage `Dockerfile`: `node:24` builds the SPA, `dotnet/sdk:10.0` publishes the web host, and the `dotnet/aspnet:10.0` runtime runs as the non-root `app` user on port 8080 with a `HEALTHCHECK` on `/healthz`.
- `compose.yaml` publishes `8080:8080` and keeps the SQLite database on the named volume `battleships-data`, mounted at `/data`.
- Liveness endpoint `GET /healthz`, mapped before the SPA fallback and covered by `HealthCheckTests`.
- MSBuild property `SkipClientBuild` skips the npm steps of the publish target when the SPA is already built.
- `.dockerignore`, a CI `docker` job that builds the image without pushing it, and `scripts/docker-smoke.sh` (compose up, wait for healthy, create a game, shoot, restart, check the game survives, check non-root, tear down).

### Key decisions reflected in the diff
- `/healthz` is liveness only, with no dependency checks, so the probe reflects process health.

## [27] Modernization phase 4d: HTTP integration tests against SQLite + coverage gate — 2026-10-26

Tags: `api` `tests` `ci`
PR: [#31](https://github.com/bcichonski/Bc.Battleships/pull/31)
Builds on: #26

### What was built
- `Tests.Common`: `TestDatabase` (a unique SQLite temp file per test-class instance, with pools cleared and `-wal`/`-shm` files removed on dispose) and `BattleshipsWebApplicationFactory` (owns a `TestDatabase`, can override board size and ship lengths).
- API integration tests are HTTP-only: `GameFlowTests` and the `GameApi` helper replace `ApiIntegrationTestBase`, `ApiTestContext` and the mediator-driven `Games/*` tests.
- `GameFlowTests` plays a full game by shooting every cell, covers repeat shots on hit cells and water, and checks that a shot on a finished game is rejected and not recorded.
- Database integration tests apply the real migrations with `MigrateAsync` on a per-instance `TestDatabase`, replacing `EnsureCreated`/`EnsureDeleted`.
- `scripts/coverage.sh` runs the tests with coverage and builds a filtered ReportGenerator report. The CI `dotnet` job now calls it, and the summary and artifact upload run even when tests fail.

### Key decisions reflected in the diff
- Coverage is a warning, not a gate: below 90% line or 85% branch (`COVERAGE_MIN_LINE` / `COVERAGE_MIN_BRANCH`) the script warns and adds a workflow annotation, and it fails only if the tests or the report fail.

## [26] Modernization phase 4c: design refactoring (game resources, project consolidation, modern C#) — 2026-10-26

Tags: `api` `domain` `sql` `client`
PR: [#30](https://github.com/bcichonski/Bc.Battleships/pull/30)
Builds on: #3, #4, #25

### What was built
- Game resources API replaces `/game`: `POST /games` (201 with `Location`), `GET /games/{id}`, and `POST /games/{id}/shots`. Unknown games return 404 and finished games return 409, both as `ProblemDetails`. A fallback under `/games/*` returns 404 for malformed ids instead of the SPA's `index.html`.
- `ShootOutcome` lets `ShootCommandHandler` reject a shot on a non-active game before touching the aggregate.
- Client: `gameApi` and `useGame` use the new routes, the game id is kept in `localStorage` (`battleships.gameId`) so a reload resumes the game, and a 404 clears it. The Vite proxy forwards `/games`, and a new e2e test covers reload-resume.
- Projects consolidated: `Messages` and `Server` merged into `Bc.Battleships.Application`, `Persistence` renamed `Bc.Battleships.Infrastructure` (now holding the design-time `BattleshipsDbContextFactory`), `EFMigrationsHelper` removed, and `IBus`/`IEvent` moved to `Domain/Events`.
- `IUnitOfWork` owns `SaveChangesAsync`, so handlers no longer save through the repository.
- `GameOptions` (section `Game`) and `GameOptionsValidator` replace the static `GameContextProvider`.
- Strongly-typed ids are `readonly record struct`s implementing `IGuidId`, converted by one generic `GuidIdConverter<TId>` registered in `ConfigureConventions`.
- Modern C# across the code: file-scoped namespaces, primary constructors, collection expressions.
- Bug fix: `GameToGameDtoConverter` places compartments by their own coordinates, so flooded cells are no longer misplaced after a reload.

### Key decisions reflected in the diff
- The web project stays `Bc.Battleships` in the Api role rather than being renamed, which leaves the `ClientApp` path untouched.
- The EF model is unchanged: the migration moves with the renamed project and no new migration is added.

## [4] Modernization phase 4b: domain bug fixes — 2026-10-26

Tags: `domain` `api` `tests`
PR: [#29](https://github.com/bcichonski/Bc.Battleships/pull/29)
Builds on: #25

### What was built
- `ShootResult` gains `Sunk` and `AlreadyShot`. `Ship.Hit` no longer re-hits a flooded compartment, and the shot that floods the last compartment returns `Sunk`.
- `ShootCommandHandler` keeps the HTTP `{hit, currentHitPoints}` shape; `hit` now means the targeted cell is a hit after the shot, so re-shooting a hit cell stays a hit.
- `FleetFactory` lists every in-bounds, non-colliding placement and picks one uniformly at random, so ships can reach row 10 and column 10. The retry loop is gone, and `FleetFactoryPositioningFailedException` is thrown only when no placement exists.
- `Random` can be injected for deterministic fleets and defaults to `Random.Shared`.
- Regression tests: `WhenShipIsHit`, `WhenPositioningFleetOnWholeBoard`, and new cases in `WhenShootWasFired`, `WhenBeingShootAt`, `WhenGameIsWon` and the API `WhenShooting`.

## [25] Modernization phase 4a: remove static service locator, game-over as aggregate invariant — 2026-10-23

Tags: `domain` `sql` `tests`
PR: [#28](https://github.com/bcichonski/Bc.Battleships/pull/28)
Builds on: #2

### What was built
- `Game.Shoot` sets the new `GameState.Won` when the computer fleet's hit points reach 0, and the existing handler save persists it.
- `DomainEventBus` is a plain DI service: the static `Instance`, `Use` and `SetCurrentBusResolver` are removed, along with the web host's `IHttpContextAccessor` seam.
- `AbandonTheShipDomainEvent` and its handler are removed.
- Back-references `Ship.Owner` and `ComputerPlayer.Game` are removed. `ComputerPlayer.PositionShips` takes the `GameContext`, and `FleetFactory` depends on `GameContext` instead of `Game`.
- New tests: domain and database `WhenGameIsWon`, API `WhenShootingTheLastShip`, `WhenPublishingDomainEvent` and `DomainEventBusWiringTests`. The event-based game-over tests and the NSubstitute dependency are removed.

### Key decisions reflected in the diff
- The event abstractions (`IEvent`, `IBus`, `IDomainEvent`, `IDomainEventBus`) are kept for future use; only the static access is removed.
- The ship foreign key stays pinned to the `OwnerGameId` shadow property, so removing the back-reference needs no migration.

## [3] Modernization phase 3: frontend rewrite (Vite + React 19 + TypeScript) — 2026-10-21

Tags: `client` `tests` `ci`
PR: [#23](https://github.com/bcichonski/Bc.Battleships/pull/23)
Builds on: #2

### What was built
- `ClientApp` rewritten in place from Create React App to Vite, React 19 and strict TypeScript, with an ESLint flat config and Vitest on jsdom. Build output stays in `ClientApp/build`.
- Vite dev server on port 44463, proxying `/game` to `ASPNETCORE_URLS` or `http://localhost:5191`.
- Runtime dependencies reduced to `react`, `react-dom` and `bootstrap`; the router, jQuery, reactstrap, workbox, service worker and `aspnetcore-*.js` scripts are removed.
- `src/api/gameApi.ts` is the only module that knows the API routes, and `src/hooks/useGame.ts` holds the game state and view selection.
- Bug fix: a game starts only from an explicit Start action, guarded against double clicks, instead of during render with a page reload.
- Vitest tests for the API client, game logic and `App`, plus a Playwright e2e test that plays a full game to "You win!".
- CI: the `client` job adds a build step, and a new `e2e` job runs Playwright.

### Key decisions reflected in the diff
- There is no router, because the app is a single screen.

## [2] Modernization phase 2: platform upgrade to .NET 10 (Mediator, EF Core 10, OpenAPI) — 2026-10-21

Tags: `api` `domain` `sql` `tests`
PR: [#22](https://github.com/bcichonski/Bc.Battleships/pull/22)
Builds on: #1

### What was built
- All projects target .NET 10; the phase 1 `RollForward` and `CheckEolTargetFramework` workarounds and the NuGet audit exception are removed.
- EF Core 10.0.13 replaces 6.0.6, and the 2022 migration is unchanged.
- Source-generated `Mediator` 3.0.2 replaces MediatR, with Scoped lifetime, scanning the Server, Domain and Messages assemblies.
- `GameEndpoints` (a `/game` Minimal API group) replaces the MVC `GameController`; the Razor error page is removed.
- `Microsoft.AspNetCore.OpenApi` and Scalar replace Swashbuckle, in Development only.
- `Microsoft.AspNetCore.SpaProxy`, `Ensure.That` and the Domain's `Microsoft.AspNetCore.Http.Abstractions` reference are removed; guards use BCL exceptions.
- Test stack: AwesomeAssertions replaces FluentAssertions, and xunit v3 (`xunit.v3.mtp-off`) replaces xunit 2.
- New tests: `GameEndpointsTests` (`WebApplicationFactory`), `WhenApplyingMigrations`, a cross-assembly event wiring test, and guard tests for `Length`, `NumberToLettersConverter` and `BoardDto`.

### Key decisions reflected in the diff
- `Length` now really rejects negative values; the old `Ensure.That` guard never threw.

## [1] Modernization phase 1: repo hygiene & CI — 2026-10-21

Tags: `ci` `tests`
PR: [#6](https://github.com/bcichonski/Bc.Battleships/pull/6)

### What was built
- Layout flattened into `src/` and `tests/`, with `Bc.Battleships.slnx` replacing the `.sln`.
- `Directory.Build.props` turns on nullable, .NET analyzers and `TreatWarningsAsErrors`, and `Directory.Packages.props` adds central package management.
- `global.json`, `.editorconfig` and `.gitattributes` added, and UTF-8 BOMs stripped from source files.
- CI workflow: a `dotnet` job (Release build, tests with coverage, ReportGenerator summary and artifact) and a `client` job (`npm ci`, lint, test).
- Dependabot configured weekly for NuGet, npm and GitHub Actions.
- README rewritten, and the changelog scaffolding added.

### Key decisions reflected in the diff
- `global.json` pins SDK 10.0.100 because `.slnx` can't be built with SDK 8, while the target stays `net6.0` until phase 2.
