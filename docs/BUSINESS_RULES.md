# Business Rules (Baseline – MUST NOT CHANGE)

This document is the **source of truth** for core rules. Any change requires a formal change request.

## 1) System Scope
- Closed internal system (not public).
- Single sub-contractor company.
- English UI only.

## 2) User Roles
- Admin: full access (data entry + management).
- Viewer: read-only dashboards and reports.
- No public registration. Users are created only by Admin.

## 3) OR Structure (OPS / BS)
- Each agreement is represented as a **Contract Context**.
- Within a contract context, OPS and BS are **two departmental budgets**.
- **Each budget has its own OR Number** (OR differs per budget):
  - OPS budget → ORNumber (OPS)
  - BS budget → ORNumber (BS)
- Both budgets are tracked independently for reporting.

## 4) Requests (LM)
- LM Requests belong to exactly one OR (OPS OR or BS OR).
- Optional safety fields on LM Requests:
  - IsCompleted (optional)
  - IsCancelled (optional)

## 5) Invoices (LM Invoices)
- LM Invoices belong to exactly one OR (OPS OR or BS OR).
- An invoice can group multiple LM Requests (many-to-many link table).
- Optional payment fields on invoices:
  - IsPaid (optional)
  - PaidDate (optional)

## 6) Attachments (MANDATORY)
- Every invoice must have **exactly two attachments**:
  1) Delivery Note
  2) Invoice Image
- No invoice approval is allowed unless both attachments exist.
- Attachments are stored on server filesystem, DB stores metadata and file key/path.

## 7) Stationery Invoice
- Stationery invoices are independent and **NOT linked** to any OR or LM requests.
- Stationery invoices also require exactly two attachments (Delivery Note + Invoice Image).
- Stationery may also have optional payment fields (IsPaid / PaidDate).

## 8) OR Closing Rule
- OR is closed when its invoices cover the OR total value as per agreed implementation (typically approved invoices sum).
- Once closed, the OR becomes read-only (no changes allowed).

## 9) Audit / Archive
- System must support reporting and archiving of records for audit.
