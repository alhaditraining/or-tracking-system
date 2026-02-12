using System.ComponentModel.DataAnnotations;

namespace ORTrackingSystem.Models;

public class StationeryInvoice
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string InvoiceNumber { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public bool? IsPaid { get; set; }

    public DateTime? PaidDate { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    [Required]
    [MaxLength(450)]
    public string CreatedByUserId { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<StationeryAttachment> StationeryAttachments { get; set; } = new List<StationeryAttachment>();
}
