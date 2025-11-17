# Appointment Ingestion Service

Opinionated sample that ingests appointments through a .NET 9 API backend and a React/Vite frontend.

## Prerequisites
- [.NET SDK 9.x](https://dotnet.microsoft.com/download)
- [Node.js 24.x](https://nodejs.org/)

## Backend (WebApi)
```bash
# Restore + run locally
dotnet restore
dotnet run --project WebApi/WebApi.csproj
```
The API exposes Swagger UI at `https://localhost:5001/docs` and in-memory persistence, so no extra dependencies are required.

## Frontend (Frontend)
```bash
cd Frontend
npm install
npm run dev
```
The Vite dev server proxies API calls to the backend; set `VITE_API_BASE_URL` in `.env` if you use non-default ports.

## Tests
- Backend: `dotnet test Tests/Tests.csproj` (runs unit + acceptance suites).
- Frontend unit/component tests: `cd Frontend && npm run test`.
- Frontend lint/storybook/playwright commands are wired up via `npm run lint`, `npm run storybook`, etc., as needed.

## Time Breakdown (≈10.5h)
- Backend: ~6h
- Frontend: ~4h
- Manual testing: 10m
- Documentation, CI: ~20m
