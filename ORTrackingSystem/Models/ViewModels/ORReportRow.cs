namespace ORTrackingSystem.Models.ViewModels;

public class ORReportRow
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
}
