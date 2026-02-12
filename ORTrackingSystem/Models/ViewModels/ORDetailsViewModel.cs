namespace ORTrackingSystem.Models.ViewModels;

public class ORDetailsViewModel
{
    public int Id { get; set; }
    public string ORNumber { get; set; } = string.Empty;
    public string ContractName { get; set; } = string.Empty;
    public BudgetType BudgetType { get; set; }
    public ORStatus Status { get; set; }
    public decimal TotalValue { get; set; }
    public decimal InvoicedTotal { get; set; }
    public decimal Remaining { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public List<LMRequestRow> LMRequests { get; set; } = new();
    public List<InvoiceRow> Invoices { get; set; } = new();
    public bool HasMissingAttachments { get; set; }
}

public class LMRequestRow
{
    public string RequestNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool? IsCompleted { get; set; }
    public bool? IsCancelled { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class InvoiceRow
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool? IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public int DeliveryNoteCount { get; set; }
    public int InvoiceImageCount { get; set; }
    public bool HasMissingAttachments => DeliveryNoteCount == 0 || InvoiceImageCount == 0;
}
