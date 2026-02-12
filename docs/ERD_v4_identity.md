# ERD v4 (SQL Server + ASP.NET Core Identity)

## Identity
- AspNetUsers (Id NVARCHAR(450) PK)
- AspNetRoles (Admin / Viewer)
- AspNetUserRoles (UserId, RoleId)

All created-by fields reference AspNetUsers.Id:
- CreatedByUserId NVARCHAR(450)

## Business Tables
- ContractContexts
- ORs (ORNumber unique, BudgetType OPS/BS)
- LM_Requests (unique per ORId + RequestNumber, optional IsCompleted/IsCancelled)
- Invoices (unique per ORId + InvoiceNumber, optional IsPaid/PaidDate)
- InvoiceRequestLinks (PK InvoiceId + RequestId)
- InvoiceAttachments (unique per InvoiceId + AttachmentType)
- StationeryInvoices (outside OR, optional IsPaid/PaidDate)
- StationeryAttachments (unique per StationeryInvoiceId + AttachmentType)
