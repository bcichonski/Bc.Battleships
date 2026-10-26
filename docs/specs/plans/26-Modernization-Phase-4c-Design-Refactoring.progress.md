# Progress — 26 Modernization Phase 4c: Design Refactoring

| Task | Title | Depends on | Complexity | Status | Notes |
| :--- | :--- | :--- | :--- | :--- | :--- |
| T1 | Consolidate projects: Application, Infrastructure, design-time factory | — | Medium | Done | sonnet, started 09:06Z, finished 09:09Z; wave verify PASS (68/8/14) |
| T2 | Strongly-typed id convention and nullable-suppressor removal | T1 | Medium | Done | sonnet, started 09:09Z, finished 09:12Z; verify FAIL(tests) on pre-existing flaky API test Put_WithMiss (row A can be fully covered by ships), passes 3/3 reruns; 70/9/14 otherwise green — T4 to make it deterministic |
| T3 | GameOptions replaces the static GameContextProvider | T2 | Medium | Done | sonnet, started 09:13Z, finished 09:16Z; wave verify PASS (77/9/15) |
| T4 | Game resources API and unit of work (backend) | T3 | High | Done | opus, started 09:16Z, returned 09:22Z (no build/test run); verify FAIL(tests): 12 API tests, POST /games 500 — ArgumentNullException 'source' in GameToGameDtoConverter.GetBoardState; retry 1 (fresh opus) started 09:23Z, finished 09:26Z (root cause: ComputerPlayer() left CoordinatesShoot null after T2); wave verify PASS (77/11/27) |
| T5 | Client and e2e on the game resources API | T4 | Medium | Done | sonnet, started 09:26Z, finished 09:31Z; lint/build/vitest 18/e2e 2 pass. Found backend defect: reloaded board misplaces flooded cells (converter indexes Ship.Compartments by position; EF load order not guaranteed) — fixed by T4 follow-up (sonnet, 09:30Z–09:33Z): converter places compartments by their Coordinates + regression test; wave verify PASS |
| T6 | Modern C# sweep and docs | T4 | Medium | Done | sonnet, started 09:26Z, finished 09:28Z (self-reported 115/115); wave verify PASS with T5 (backend 77/11/28, client 18 + lint + build; e2e 2 pass) |
