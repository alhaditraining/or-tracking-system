namespace ORTrackingSystem.Models.ViewModels;

public class DashboardViewModel
{
    public BudgetMetrics OPS { get; set; } = new();
    public BudgetMetrics BS { get; set; } = new();
    public BudgetMetrics Total { get; set; } = new();
}

public class BudgetMetrics
{
    public int TotalORs { get; set; }
    public int OpenORs { get; set; }
    public int ClosedORs { get; set; }
    public decimal TotalORValue { get; set; }
    public decimal TotalInvoicedAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public decimal PaidInvoicesTotal { get; set; }
    public decimal UnpaidInvoicesTotal { get; set; }
}
