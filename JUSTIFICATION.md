# Design Justification

## Architecture Overview
Each capability lives in a vertical slice under `WebApi/Features/*`, and every slice keeps a consistent layering: Presentation endpoints, Contracts for request/response DTOs, Application services, Domain objects plus value types, and Infrastructure for persistence abstractions.  

This structure fits a SaaS product because new capabilities can be added by cloning a slice without touching unrelated code, and the thin composition root in `Program.cs` keeps cross-cutting concerns (validation, EF Core, DI) centralized.

## Design Choice: Separate API and Domain Models
The split enforces Single Responsibility Principle: Contracts exist solely to translate HTTP payloads, versioning concerns, and documentation metadata, whereas Domain types focus on invariants and behaviors (e.g., `AppointmentTime` bounds, `ServiceDuration` arithmetic).  

Because each side has one reason to change, we can iterate on endpoints without touching the core model, and refine business rules without worrying about serialization quirks. This keeps the codebase evolvable in the long run.

## Testing Philosophy
Different test layers have distinct strengths. Unit tests are the scalpel: fast feedback and precise focus on a single behavior. Integration tests are the broadsword: wider coverage and higher realism.  

Unit tests surface regressions quickly and isolate logic, but they miss I/O subtleties. Integration tests catch more edge cases, yet they are slower, harder to wire up, and trickier to keep hermetic. I typically add one integration test per acceptance criterion so every slice has an end-to-end safeguard without bloating the suite.


## Trade-offs and Omissions
- Operational observability is minimal; I leaned on the default ASP.NET Core logging. Normally I would use OpenTelemetry to send logs, traces and metrics to a centralized backend.
- Data access uses EF Core's in-memory provider rather than a SQL-backed store, trading realism for zero-setup development.
- The UI lacks a react-testing-library harness and Zod helpers, so future contributors may resort to copy/pasting boilerplate until those utilities are added.
- The app is intentionally not optimized for high-throughput ingestion; introducing a queue and worker would increase complexity now, so this version targets interactive form submissions rather than bulk pipelines.

## Deployment
There's already a GitHub Actions workflow that builds and tests the app. To deploy:
- Backend: add a Dockerfile that dotnet publishes the WebApi into a runtime image, push that image to Azure Container Registry, and run it in Azure Container Apps (or Container Instances if you don’t need autoscaling). You probably want some kind of bicep file as IaC to set up the deployment environments correctly.
- Frontend: unless you have server-side rendering needs, deploy it as static files to Azure Static Web Apps or an Azure Storage static site + CDN. That keeps container costs down and caching/CDN support is built in. Only containerize the frontend if you truly need a Node server.
