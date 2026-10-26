# Design Decisions

Standing architectural and technical decisions for this project — the patterns a plan should
follow rather than re-litigate.

This file is one half of the **memory bank** that `create-spec-plan` consults before drafting
(`docs/specs/memory/async-operations.md` is the other). The spec planner reads it to align a
new plan with decisions already made, so anything recorded here stops being an Open Question
in every future plan. Keep it current: a decision that lives only in a merged PR will be
rediscovered and possibly contradicted.

**Scope.** Durable, cross-cutting choices — error handling, identifiers, value objects,
persistence patterns, API boundaries. Not story-specific design (that belongs in the plan) and
not a changelog of what was built (that is `docs/CHANGELOG.md`).

---

## Decision template

Copy this shape for each decision. Drop sections that add nothing.

```markdown
## <Decision, stated as the rule>

### Purpose
What problem this solves, in one or two sentences.

### Rationale
Why this option over the alternatives actually considered. Name the alternatives.

### Usage
How to apply it — the pattern, the types or helpers involved, where the seams are.

### Trade-offs
What this costs, and the cases where it is the wrong tool.
```

---

<!-- First real decision goes here. Delete this comment and the template above once the file
     has a few entries and the shape is self-evident from them. -->

## Domain events: keep the infrastructure, publish only through an injected, awaited bus

### Purpose
Lets one aggregate tell others that something happened (e.g. a future `GameWon` for player statistics) without coupling them.

### Rationale
Decided in #25. The original static `DomainEventBus.Instance` (service locator via `IHttpContextAccessor`, fire-and-forget, handler reusing the scoped DbContext mid-save) was removed. The event abstractions were deliberately **kept** at the owner's request even though nothing publishes an event today, so the next story that needs one doesn't re-introduce them ad hoc.

### Usage
- Event types implement `IDomainEvent` (Domain, `Events/`), which extends `IEvent` (Domain, `Events/`, a Mediator `INotification`).
- Publish through `IDomainEventBus`, injected (scoped; registered in `Application/Infrastructure/Services.cs` with `IBus` → `Bus` → Mediator) and **awaited**. Never through a static.
- Publish from the application layer (command handler) **after** the aggregate change is saved, not from inside entities. Entities don't hold references back to their owners for event purposes.
- Handlers may live in Domain or Application; Mediator's source generator runs in Application and scans both.
- Rules *inside* one aggregate (e.g. game-over when the fleet's HP hits 0) are enforced synchronously by the aggregate root, never via an event.

### Trade-offs
Cross-aggregate reactions published after save are eventually consistent: a failing handler doesn't roll back the first aggregate. If that matters, add an outbox rather than publishing inside the same `SaveChangesAsync`.
