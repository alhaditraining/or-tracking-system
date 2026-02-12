using System.ComponentModel.DataAnnotations;

namespace ORTrackingSystem.Models;

public class Invoice
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string InvoiceNumber { get; set; } = string.Empty;

    public int ORId { get; set; }

    public decimal Amount { get; set; }

    public bool? IsPaid { get; set; }

    public DateTime? PaidDate { get; set; }

    public DateTime CreatedDate { get; set; }

    [Required]
    [MaxLength(450)]
    public string CreatedByUserId { get; set; } = string.Empty;

    // Navigation properties
    public OR OR { get; set; } = null!;
    public ICollection<InvoiceRequestLink> InvoiceRequestLinks { get; set; } = new List<InvoiceRequestLink>();
    public ICollection<InvoiceAttachment> InvoiceAttachments { get; set; } = new List<InvoiceAttachment>();
}
