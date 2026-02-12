namespace ORTrackingSystem.Models.ViewModels;

public class StationeryInvoiceReportRow
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public bool? IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
    public DateTime CreatedDate { get; set; }
}
