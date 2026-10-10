# GitHub Copilot instructions

The root [AGENTS.md](../AGENTS.md) is the canonical source for repository architecture, constraints, testing requirements, and build commands. Read and follow it for every task. If guidance ever differs, follow `AGENTS.md` and update this file only with Copilot-specific workflow guidance; do not copy or redefine its rules here.

## Technology context

This is a .NET 10 solution using Clean Architecture. Preserve each project's `net10.0`, nullable, and implicit-using settings. Keep domain behavior framework-independent and put code in its owning layer; consult `AGENTS.md` for the exact dependency and service/module boundaries. The API currently uses ASP.NET Core minimal APIs and OpenAPI, while the test project currently uses xUnit. Confirm current project files and established patterns before adding frameworks or assuming planned modules, persistence, or services are implemented.

## Copilot workflow

For feature work, use the agents in this order: **Architecture Reviewer → .NET Engineer → Test Engineer → Code Reviewer**. Start with the read-only architecture assessment before implementation; use its handoff to carry approved decisions forward. The later handoffs are suggested gates, not automatic approvals. Route confirmed defects or review findings to Bug Fixer. For a narrow bug report, Bug Fixer can start diagnosis directly, while still checking architectural constraints and involving review when the change warrants it.

1. **Understand before editing.** Inspect the relevant projects, neighboring implementations, tests, and configuration. Trace dependencies and callers before changing a contract or behavior. Treat the checked-in code as the source of truth; do not assume planned architecture is already implemented.
2. **Plan changes at the right boundary.** Identify the owning layer/module and make the smallest coherent change there. Keep API, application, domain, and infrastructure responsibilities distinct. Avoid broad refactors, speculative abstractions, and unrelated file changes.
3. **Preserve compatibility deliberately.** Before changing public APIs, serialized messages, persistence mappings, or configuration, inspect consumers and compatibility expectations. Call out unavoidable breaking changes and required rollout or migration considerations; do not silently change contracts.
4. **Implement production behavior explicitly.** Validate inputs at the appropriate boundary, handle expected failures predictably, use asynchronous APIs correctly, and avoid hidden global state. Keep sensitive values and tenant-specific data out of logs, exceptions, and responses. Do not introduce new packages or infrastructure without a concrete need.
5. **Prove behavior.** Add or update focused tests that cover success, invalid input, relevant boundary conditions, and failure paths. Prefer deterministic tests; do not weaken or remove assertions merely to make a change pass. Run the applicable checks specified in `AGENTS.md` and report exactly what ran and its outcome. If a check cannot run, state why rather than implying it passed.
6. **Review the final diff.** Check for accidental generated files, unrelated changes, secrets, incomplete error paths, and mismatches between implementation and tests. Keep documentation and examples consistent with changed behavior.

## Completion response

Conclude with a concise summary of the changes, tests/checks actually executed, and any relevant assumptions, compatibility implications, or remaining risks. Never claim unverified behavior or tests as successful. Do not merge or deploy unless explicitly authorized.
