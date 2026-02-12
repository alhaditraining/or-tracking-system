# OR Tracking System (OR / OPS / BS)

Internal system for tracking OR budgets split into two departmental budgets (OPS & BS), managing requests (LM), invoices with mandatory attachments, stationery invoices (outside OR), reporting, and archiving.

## Scope
- **Closed internal system** (not public, not SaaS)
- **Single sub-contractor company**
- **English UI only**
- Users are limited to two roles:
  - **Admin**: full access
  - **Viewer**: read-only access to dashboards and reports

## Key Concepts
- **Contract Context**: agreement umbrella between sub-contractor and main company.
- **OR**: financial envelope within a contract context.
- **OPS / BS**: two separate departmental budgets under the same agreement, each has its own **OR Number**.
- **LM Request**: request entry created before invoicing.
- **Invoice**: created by grouping LM requests under the same OR (OPS or BS).
- **Stationery Invoice**: independent invoices outside OR.

## Mandatory Attachments
Every invoice must have **exactly two attachments**:
1) Delivery Note  
2) Invoice Image  

(Enforced by attachment type constraints and approval validation.)

## Payment / Safety Fields (Optional)
- LM Request: `IsCompleted`, `IsCancelled` (optional)
- Invoice: `IsPaid`, `PaidDate` (optional)

## Tech Stack
- **Blazor Server (.NET 8)**
- **SQL Server**
- **ASP.NET Core Identity** (roles: Admin / Viewer)
- Attachments stored on server filesystem + metadata in DB

## Security Notes (IMPORTANT)
- This repo is public temporarily. **Do not commit secrets**:
  - Real connection strings
  - Passwords/tokens
  - Server IPs / paths
- Use placeholder configuration in `appsettings.json`.
- Use local secrets for development (User Secrets) and environment variables for production.

## Development Milestones
See:
- `docs/PDR_Baseline.pdf`
- `docs/ERD_v4_identity.md`
- `docs/BUSINESS_RULES.md`
- `docs/MILESTONES.md`

## Quick Start (placeholder)
1) Configure SQL Server connection (local/dev)
2) Run EF migrations
3) Run the app
