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

## (no decisions recorded yet)

Seeded by `/grind-setup`. Until this file has content, `create-spec-plan` will find no prior art
and will raise more Open Questions than it otherwise would — so the first few entries pay for
themselves quickly. Good first candidates: how errors propagate (exceptions vs a result type),
how identifiers are generated and whether internal ids may leak past the API boundary, and
whether a repository abstraction sits over the ORM.
