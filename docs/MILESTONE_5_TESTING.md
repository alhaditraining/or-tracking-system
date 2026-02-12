# Milestone 5 - Reports & Archive Testing Guide

## Overview
This milestone implements comprehensive read-only reporting functionality for the OR Tracking System.

## New Pages Implemented

### 1. Dashboard (/reports/dashboard)
**Purpose**: High-level KPIs and metrics overview

**Features**:
- Overall metrics: Total ORs, Total OR Value, Total Invoiced, Remaining
- Split by Budget Type (OPS and BS)
- Metrics include:
  - Total ORs (Open/Closed breakdown)
  - Total OR Value
  - Total Invoiced Amount
  - Remaining Amount
  - Paid vs Unpaid Invoice Totals

**Authorization**: ViewerOrAdmin (both Admin and Viewer can access)

**Performance**: Uses database-level aggregations (GroupBy) to avoid loading all data into memory

### 2. OR Reports (/reports/ors)
**Purpose**: Searchable list of all ORs with filtering

**Features**:
- Filters:
  - Contract Context (dropdown)
  - Budget Type (OPS/BS/All)
  - Status (Open/Closed/All)
  - Date Range (From/To)
  - Archive Filter: All / Active Only (Open) / Archived Only (Closed)
- Columns:
  - OR Number
  - Contract Name
  - Budget Type
  - Status (with badge)
  - Total Value
  - Invoiced Total (computed)
  - Remaining (computed)
  - Created Date
- Click on any row to view details

**Authorization**: ViewerOrAdmin

**Performance**: Uses projection to compute invoice totals at database level

### 3. OR Details (/reports/ors/{orId})
**Purpose**: Detailed view of a single OR with related data

**Features**:
- OR Information:
  - OR Number, Contract Name, Budget Type, Status, Created Date
  - Total Value, Invoiced Total, Remaining
- LM Requests Table:
  - Request Number, Amount, Completed, Cancelled, Created Date
- Invoices Table:
  - Invoice Number, Amount, Paid Status, Paid Date, Created Date
  - Delivery Notes Count
  - Invoice Images Count
  - Attachment Status (Complete/Missing badge)
- Warning banner if any invoices have missing attachments

**Authorization**: ViewerOrAdmin

**Navigation**: "Back to OR Reports" button

### 4. Invoice Reports (/reports/invoices)
**Purpose**: Searchable list of all invoices across all ORs

**Features**:
- Filters:
  - Contract Context (dropdown)
  - Budget Type (OPS/BS/All)
  - Paid Status (Paid/Unpaid/All)
  - Date Range (From/To)
  - Archive Filter: All / Active Only (Unpaid) / Archived Only (Paid)
- Columns:
  - OR Number
  - Contract Name
  - Budget Type
  - Invoice Number
  - Amount
  - Paid Status (with badge)
  - Paid Date
  - Created Date
- Summary: Total Records and Total Amount

**Authorization**: ViewerOrAdmin

### 5. Stationery Reports (/reports/stationery)
**Purpose**: List of all stationery invoices (separate from OR invoices)

**Features**:
- Filters:
  - Paid Status (Paid/Unpaid/All)
  - Date Range (From/To)
  - Archive Filter: All / Active Only (Unpaid) / Archived Only (Paid)
- Columns:
  - Invoice Number
  - Amount
  - Paid Status (with badge)
  - Paid Date
  - Created Date
- Summary: Total Records and Total Amount

**Authorization**: ViewerOrAdmin

## Navigation Changes

Updated the navigation menu to show individual report links under ViewerOrAdmin:
- Dashboard (with speedometer icon)
- OR Reports (with cash-stack icon)
- Invoice Reports (with receipt icon)
- Stationery Reports (with pencil-square icon)

The old generic "Reports" link has been replaced with these specific links.

## Archive Implementation

Archive is implemented as filter-based views, not actual deletion:

**For ORs**:
- Active = Open status
- Archived = Closed status

**For Invoices** (both regular and stationery):
- Active = Unpaid (IsPaid != true)
- Archived = Paid (IsPaid == true)

Three filter options available:
1. **All**: Shows everything
2. **Active Only**: Shows only open/unpaid items
3. **Archived Only**: Shows only closed/paid items

## Testing Scenarios

### Scenario 1: Admin Access
1. Login as admin@ortracking.local
2. Verify Dashboard, OR Reports, Invoice Reports, and Stationery Reports links appear
3. Navigate to each page and verify access is granted
4. Test all filters on each page

### Scenario 2: Viewer Access
1. Create a Viewer user (if not exists) via /admin/users
2. Login as Viewer
3. Verify same report links appear as Admin
4. Navigate to each page and verify access is granted
5. Verify Viewer cannot access /admin/* pages

### Scenario 3: Dashboard Metrics
1. Navigate to /reports/dashboard
2. Verify KPI cards show:
   - Total ORs with Open/Closed breakdown
   - Total OR Value
   - Total Invoiced amount
   - Remaining amount
3. Verify OPS and BS sections show separate metrics
4. Create test data and verify metrics update correctly

### Scenario 4: OR Reports Filtering
1. Navigate to /reports/ors
2. Test each filter:
   - Select a Contract Context, verify ORs are filtered
   - Select Budget Type (OPS/BS), verify filtering
   - Select Status (Open/Closed), verify filtering
   - Set date range, verify filtering
3. Test archive filters:
   - "Active Only" shows only Open ORs
   - "Archived Only" shows only Closed ORs
   - "All" shows everything
4. Click "Clear" button to reset all filters

### Scenario 5: OR Details
1. Navigate to /reports/ors
2. Click on an OR row
3. Verify OR details page shows:
   - OR information
   - LM Requests table (if any)
   - Invoices table (if any)
4. If an invoice has no attachments, verify warning appears
5. Click "Back to OR Reports" to return

### Scenario 6: Invoice Reports
1. Navigate to /reports/invoices
2. Test filtering by Contract Context, Budget Type, Paid Status, Date Range
3. Test archive filters (Paid/Unpaid)
4. Verify Total Amount is correctly calculated

### Scenario 7: Stationery Reports
1. Navigate to /reports/stationery
2. Test filtering by Paid Status and Date Range
3. Test archive filters
4. Verify Total Amount is correctly calculated

## Data Models Used

- **OR**: Main order entity with TotalValue, Status, BudgetType
- **ContractContext**: Parent contract for ORs
- **Invoice**: Invoices linked to ORs
- **LM_Request**: Request items within ORs
- **InvoiceAttachment**: File attachments for invoices (DeliveryNote or InvoiceImage)
- **StationeryInvoice**: Standalone stationery invoices

## ViewModels Created

- `DashboardViewModel` and `BudgetMetrics`: For dashboard KPIs
- `ORReportRow`: For OR listing
- `ORDetailsViewModel`: For OR details page
- `LMRequestRow` and `InvoiceRow`: For detail tables
- `InvoiceReportRow`: For invoice listing
- `StationeryInvoiceReportRow`: For stationery listing

## Performance Optimizations

1. **Dashboard**: Uses database-level GroupBy and Sum operations instead of loading all data
2. **OR Reports**: Uses projection without unnecessary Include to compute totals at SQL level
3. **OR Details**: Avoids redundant computations by storing results in variables
4. All queries use efficient EF Core patterns to minimize database round-trips

## Security

- All pages require `ViewerOrAdmin` policy
- No write operations implemented (read-only)
- CodeQL scan: 0 vulnerabilities found
- No secrets or sensitive data exposed in views

## Known Limitations

1. No pagination implemented (may need to add for large datasets)
2. No export functionality (CSV/Excel) - can be added in future
3. No chart visualizations - currently table/card based
4. Archive is filter-based, not a separate database state

## Future Enhancements

1. Add pagination for large result sets
2. Add export to CSV/Excel functionality
3. Add chart visualizations to dashboard
4. Add drill-down capabilities from dashboard to filtered reports
5. Add sorting capabilities to report tables
6. Add search functionality (e.g., search by OR number, invoice number)
