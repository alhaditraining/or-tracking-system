# OR Tracking System - Setup Instructions

## Milestone 1 - Identity Foundation

This milestone implements the basic authentication and authorization infrastructure for the OR Tracking System.

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB, Express, or Full)
- Visual Studio 2022 or VS Code

## Initial Setup

### 1. Configure Database Connection

The default connection string in `appsettings.json` uses SQL Server LocalDB:

```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ORTrackingSystem;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
```

**For production or different environments:**
- Use User Secrets for development (recommended)
- Use environment variables for production
- **Never commit real connection strings or passwords to the repository**

To set up User Secrets for development:
```bash
cd ORTrackingSystem
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Your-Connection-String-Here"
```

### 2. Apply Database Migrations

Run the following command to create the database and apply migrations:

```bash
cd ORTrackingSystem
dotnet ef database update
```

This will create:
- The ORTrackingSystem database
- Identity tables (Users, Roles, etc.)

### 3. Configure Admin Password (Optional)

The system seeds a default admin user with:
- Email: `admin@ortracking.local`
- Default Password: `Admin@123` (development only)

**To use a custom admin password in development:**

Set an environment variable before running the application:

```bash
# Windows (PowerShell)
$env:ADMIN_PASSWORD = "YourSecurePassword123!"
dotnet run

# Linux/Mac
export ADMIN_PASSWORD="YourSecurePassword123!"
dotnet run
```

**For production deployment:**
- Set the `ADMIN_PASSWORD` environment variable on the server
- Or modify the `DbInitializer.cs` to use a secure secrets management system

### 4. Run the Application

```bash
cd ORTrackingSystem
dotnet run
```

The application will be available at:
- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`

## Default Users

After first run, the following user is seeded:

| Email | Password | Role | Notes |
|-------|----------|------|-------|
| admin@ortracking.local | Admin@123 | Admin | Default dev password (change in production) |

## Features Implemented

### Authentication & Authorization
- ✅ ASP.NET Core Identity with SQL Server
- ✅ Role-based authorization (Admin, Viewer)
- ✅ Public registration disabled
- ✅ No Register link in UI

### Admin Features (/admin/users)
- ✅ Create new users
- ✅ Assign roles (Admin/Viewer)
- ✅ Activate/Deactivate users
- ✅ View all users and their roles

### Reports (/reports)
- ✅ Accessible to Admin and Viewer roles
- ✅ Placeholder for future reports (Milestone 5)

### Security
- ✅ Role-based policies (AdminOnly, ViewerOrAdmin)
- ✅ Authorization attributes on pages
- ✅ Navigation menu shows links based on user role

## Testing the Implementation

### Test Admin Access
1. Navigate to `https://localhost:5001`
2. Click "Login"
3. Login with: `admin@ortracking.local` / `Admin@123`
4. You should see:
   - "User Management" link in navigation (Admin only)
   - "Reports" link in navigation (Admin and Viewer)
5. Access `/admin/users` to create and manage users
6. Access `/reports` to see the reports placeholder

### Test Viewer Access
1. Login as Admin
2. Go to `/admin/users`
3. Create a new user with Viewer role
4. Logout
5. Login with the new Viewer user
6. You should see:
   - "Reports" link in navigation (accessible)
   - No "User Management" link (Admin only)
7. Attempt to navigate to `/admin/users` directly - should be denied

### Test Registration Disabled
1. Logout if logged in
2. Go to the login page
3. Verify no "Register" link is shown
4. Navigate to `/Account/Register` directly
5. Should see a message that registration is disabled

## Project Structure

```
ORTrackingSystem/
├── Components/
│   ├── Account/          # Identity pages (Login, Register, etc.)
│   ├── Layout/           # Layout components (NavMenu, MainLayout)
│   └── Pages/
│       ├── Admin/        # Admin-only pages
│       │   └── Users.razor    # User management
│       └── Reports.razor      # Reports page
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── ApplicationUser.cs
│   ├── DbInitializer.cs       # Seed roles and admin user
│   └── Migrations/            # EF Core migrations
├── Program.cs            # Application configuration
└── appsettings.json      # Configuration (with placeholder connection string)
```

## Security Notes

⚠️ **IMPORTANT: This repository is public**

- Do NOT commit real connection strings
- Do NOT commit passwords or tokens
- Do NOT commit server IPs or paths
- Use placeholder values in `appsettings.json`
- Use User Secrets for local development
- Use environment variables for production

## Next Steps

- Milestone 2: Implement core business entities (ORs, Requests, Invoices)
- Milestone 3: Build CRUD modules for core functionality
- Milestone 4: Add stationery management
- Milestone 5: Implement reports and archiving
- Milestone 6: Hardening and deployment

## Troubleshooting

### Cannot connect to database
- Ensure SQL Server (LocalDB) is installed
- Check connection string in `appsettings.json` or User Secrets
- Try running migrations again: `dotnet ef database update`

### Cannot login with admin user
- Ensure migrations have been applied
- Check that the application has run at least once (to seed data)
- Verify the admin password (default: `Admin@123` or your custom password)

### Access denied errors
- Check that the user has the correct role assigned
- Verify authorization policies in `Program.cs`
- Check the `[Authorize]` attributes on pages
