# Appointment Ingestion Service

Opinionated sample that ingests appointments through a .NET 10 API backend and a React/Vite frontend, now orchestrated with Aspire.

## Prerequisites
- [.NET SDK 10.x](https://dotnet.microsoft.com/download)
- [Node.js 25.x](https://nodejs.org/)

## Orchestrated (Aspire)
```bash
dotnet restore
dotnet run --project AppointmentIngestionService.AppHost/AppointmentIngestionService.AppHost.csproj
```
This starts the backend API and the Vite dev server together; the AppHost wires `VITE_API_BASE_URL` for you. The Vite server port is assigned dynamically—check Aspire output for the URL.

## Backend only (WebApi)
```bash
dotnet run --project WebApi/WebApi.csproj
```

## Frontend only (Frontend)
```bash
cd Frontend
npm install
npm run dev
```
Set `VITE_API_BASE_URL` if you are not running the Aspire host.

## Tests
- Backend: `dotnet test Tests/Tests.csproj` (runs unit + acceptance suites).
- Frontend unit/component tests: `cd Frontend && npm run test`.
- Frontend lint/storybook/playwright commands are wired up via `npm run lint`, `npm run storybook`, etc., as needed.
