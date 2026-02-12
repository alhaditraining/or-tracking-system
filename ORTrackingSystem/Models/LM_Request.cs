using System.ComponentModel.DataAnnotations;

namespace ORTrackingSystem.Models;

public class LM_Request
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string RequestNumber { get; set; } = string.Empty;

    public int ORId { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal Amount { get; set; }

    public bool? IsCompleted { get; set; }

    public bool? IsCancelled { get; set; }

    public DateTime CreatedDate { get; set; }

    [Required]
    [MaxLength(450)]
    public string CreatedByUserId { get; set; } = string.Empty;

    // Navigation properties
    public OR OR { get; set; } = null!;
    public ICollection<InvoiceRequestLink> InvoiceRequestLinks { get; set; } = new List<InvoiceRequestLink>();
}
