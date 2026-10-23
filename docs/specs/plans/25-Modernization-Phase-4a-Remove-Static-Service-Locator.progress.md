# Progress — 25 Modernization Phase 4a: Remove Static Service Locator, Game-over as Aggregate Invariant

| Task | Title | Depends on | Complexity | Status | Notes |
| :--- | :--- | :--- | :--- | :--- | :--- |
| T1 | Game-over as aggregate invariant (`Won`) + regression tests | — | Medium | Done | sonnet; started 09:55Z; finished 09:58Z; regression fails recorded: API round-trip + DB WhenGameIsWon stayed Active; temp UnitTests.Domain/EventBusSubstitution.cs (T2 deletes); verify PASS (49+10+12) |
| T2 | Delete the event bus, service-locator seam and back-references; simplify the EF mapping | T1 | Medium | Done | sonnet; started 09:58Z; finished 10:02Z; EF: navigation removal renamed Ship FK OwnerGameId->ComputerPlayerGameId, pinned shadow FK OwnerGameId -> no pending changes, no migration; NSubstitute dropped; verify PASS (49+8+11 .NET; client 16 + lint) |
