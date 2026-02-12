# Milestones

## Milestone 1 — Identity Foundation
- Blazor Server + Identity (SQL Server)
- Disable public registration
- Seed roles (Admin/Viewer)
- Seed Admin user
- Admin-only: /admin/users (create users, assign roles, activate/deactivate)

## Milestone 2 — Core Schema (EF Core)
- Add business entities + IdentityDbContext
- Migrations: ContractContexts, ORs, LM_Requests, Invoices, Attachments, Stationery
- Constraints/Indexes per BUSINESS_RULES

## Milestone 3 — Core Modules
- OR CRUD (OPS/BS OR numbers)
- LM Requests CRUD (optional flags)
- Invoice creation (group requests)
- Attachments upload + enforce 2 types

## Milestone 4 — Stationery
- Stationery invoices CRUD + attachments

## Milestone 5 — Reports & Archive
- OR summary
- OPS/BS budget reports
- invoice and stationery reports
- archive filters (IsPaid / PaidDate)

## Milestone 6 — Hardening & Delivery
- permissions enforcement
- closed OR read-only
- validations
- deployment
