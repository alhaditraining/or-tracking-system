namespace ORTrackingSystem.Models.ViewModels;

public class InvoiceReportRow
{
    public string ORNumber { get; set; } = string.Empty;
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool? IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public BudgetType BudgetType { get; set; }
    public string ContractName { get; set; } = string.Empty;
}
