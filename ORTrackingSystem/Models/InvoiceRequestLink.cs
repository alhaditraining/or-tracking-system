namespace ORTrackingSystem.Models;

public class InvoiceRequestLink
{
    public int InvoiceId { get; set; }
    public int RequestId { get; set; }

    // Navigation properties
    public Invoice Invoice { get; set; } = null!;
    public LM_Request Request { get; set; } = null!;
}
