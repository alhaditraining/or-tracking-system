# OR Tracking System (OR / OPS / BS)

Internal system for tracking OR budgets split into two departmental budgets (OPS & BS), managing requests (LM), invoices with mandatory attachments, stationery invoices (outside OR), reporting, and archiving.

## Current Status

**Milestone 1 (Identity Foundation) - Completed ✅**

See [SETUP.md](SETUP.md) for detailed setup instructions.

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
- **Blazor Server (.NET 9)**
- **SQL Server**
- **ASP.NET Core Identity** (roles: Admin / Viewer)
- Attachments stored on server filesystem + metadata in DB

## Quick Start

### Prerequisites
- .NET 9 SDK
- SQL Server (LocalDB, Express, or Full)

### Setup and Run

1. **Clone the repository**
   ```bash
   git clone https://github.com/alhaditraining/or-tracking-system.git
   cd or-tracking-system
   ```

2. **Configure database connection** (optional - uses LocalDB by default)
   ```bash
   cd ORTrackingSystem
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your-Connection-String-Here"
   ```

3. **Apply migrations**
   ```bash
   cd ORTrackingSystem
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Login**
   - Navigate to https://localhost:5001
   - Default admin credentials:
     - Email: `admin@ortracking.local`
     - Password: `Admin@123` (development only)

For detailed setup instructions, configuration options, and troubleshooting, see [SETUP.md](SETUP.md).

## Features (Milestone 1)

- ✅ ASP.NET Core Identity with SQL Server
- ✅ Role-based authorization (Admin, Viewer)
- ✅ Public registration disabled
- ✅ Admin user management (/admin/users)
- ✅ Reports placeholder (/reports)
- ✅ Role-based navigation

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

## Project Structure

```
or-tracking-system/
├── docs/                      # Documentation
├── ORTrackingSystem/          # Main Blazor Server application
│   ├── Components/
│   │   ├── Account/          # Identity pages
│   │   ├── Layout/           # Layout components
│   │   └── Pages/
│   │       ├── Admin/        # Admin-only pages
│   │       └── Reports.razor # Reports page
│   ├── Data/                 # EF Core context and models
│   ├── Migrations/           # EF Core migrations
│   └── Program.cs            # Application startup
├── SETUP.md                   # Detailed setup instructions
└── README.md                  # This file
```

## Roadmap

- [x] **Milestone 1**: Identity Foundation
- [ ] **Milestone 2**: Core Schema (EF Core)
- [ ] **Milestone 3**: Core Modules (OR, LM Requests, Invoices)
- [ ] **Milestone 4**: Stationery
- [ ] **Milestone 5**: Reports & Archive
- [ ] **Milestone 6**: Hardening & Delivery
