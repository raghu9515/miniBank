# miniBank

A learning-friendly banking/fintech sample built with ASP.NET Core 8 and Vue 3 + Vite. The project illustrates fundamentals such as JWT authentication, rate limiting, request correlation, and audit logging so you can explore common building blocks in regulated applications.

## Project structure

```
miniBank/
├── server/           # ASP.NET Core 8 Web API
└── client/           # Vue 3 single-page application powered by Vite
```

## Server (ASP.NET Core 8)

Key features:

- Minimal Web API with controllers for authentication, accounts, and audit log access.
- JWT authentication + authorization for protected endpoints.
- Fixed window rate limiting for API resilience.
- Correlation IDs (`X-Correlation-ID`) propagated via middleware.
- File-based audit logging for logins, account creation, and transfers.
- Swagger/OpenAPI enabled in development.

### Run the API

Install the .NET 8 SDK, then from the `server` directory:

```bash
dotnet restore
dotnet run --urls "http://0.0.0.0:5000"
```

Useful endpoints:

- `POST /api/auth/register` – create a demo account.
- `POST /api/auth/login` – returns a JWT for subsequent requests.
- `GET /api/accounts` – list accounts (authorization required).
- `POST /api/accounts/transfer` – perform a transfer and record an audit event.
- `GET /api/audit` – view the audit trail (authorization required).
- `GET /health` – lightweight health probe.

Default JWT settings live in `server/appsettings.json`. Replace the signing key before any real deployment.

## Client (Vue 3 + Vite)

The UI shows accounts, recent transactions, and audit entries using the API.

### Run the UI

From the `client` directory install dependencies and start the dev server:

```bash
npm install
npm run dev
```

Then browse to http://localhost:5173. The UI expects the API at http://localhost:5000 by default (configure in `client/src/utils/api.ts`).

### Key UI files

- `src/views/DashboardView.vue` – lists sample accounts and recent transactions.
- `src/views/AuditLogView.vue` – fetches and displays audit entries.
- `src/stores/bank.ts` – Pinia store that calls the API.
- `src/utils/api.ts` – axios client pre-configured with correlation IDs and JWT injection.

## Security & audit considerations

This repository is intentionally lightweight for education. For production scenarios you would add:

- Proper password hashing (e.g., ASP.NET Identity), key vault storage, and HTTPS certificates.
- Database persistence instead of in-memory stores and file-based audit logs.
- Background processing for audit fan-out, SIEM ingestion, and anomaly detection.
- Observability (structured logs, metrics, tracing) and secrets management.

Enjoy experimenting!
