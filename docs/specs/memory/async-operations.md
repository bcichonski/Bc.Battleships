# Async Operations

How this project does asynchronous work: messaging topology, event and command contracts,
dispatch and registration, retries, and idempotency.

This file is one half of the **memory bank** that `create-spec-plan` consults before drafting
(`docs/specs/memory/design-decisions.md` is the other). It matters most for **Integration**
stories, where the plan has to state an event or command contract precisely and back it with
evidence — anything already recorded here is evidence the planner does not have to go re-fetch.

**Scope.** The messaging substrate and the conventions around it. Per-story payload design
belongs in that story's plan, not here.

---

## What to record

- **Topology** — queues, topics, subscriptions, and which component owns each. Where the
  infrastructure is declared (e.g. a bicep or Terraform file), since a new subscription almost
  always needs a matching infrastructure change to avoid a runtime "entity not found".
- **Contracts** — the shape of integration events and commands, how payload type ids or
  dispatch keys are derived, and whether that derivation is deterministic (framework default)
  or explicit.
- **Dispatch & registration** — where a handler is registered, and what happens to a message
  with no registered handler.
- **Delivery semantics** — at-least-once vs at-most-once, retry and dead-letter policy, and
  therefore what idempotency each handler must provide.
- **Ordering** — whether any flow depends on message order, and what guarantees the transport
  actually gives.
- **Outbox** — if writes are published transactionally, how the outbox is written and drained.

---

## Entry template

```markdown
## <Flow or convention name>

### Shape
The contract or topology, concretely.

### Guarantees
Delivery, ordering, and what the consumer must therefore tolerate.

### Where it is wired
Registration site, infrastructure declaration, configuration keys.
```

---

<!-- First real entry goes here. -->

## (nothing recorded yet)

Seeded by `/grind-setup`. If this project does no asynchronous messaging, say so explicitly
here rather than deleting the file — `create-spec-plan` reads this path, and an explicit
"synchronous only; no message bus" is a useful fact that closes questions, whereas a missing
file just reads as an unknown.
