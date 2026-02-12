using System.ComponentModel.DataAnnotations;

namespace ORTrackingSystem.Models;

public class InvoiceAttachment
{
    public int Id { get; set; }

    public int InvoiceId { get; set; }

    public AttachmentType AttachmentType { get; set; }

    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    public long FileSize { get; set; }

    [MaxLength(100)]
    public string? ContentType { get; set; }

    public DateTime UploadedDate { get; set; }

    [Required]
    [MaxLength(450)]
    public string CreatedByUserId { get; set; } = string.Empty;

    // Navigation properties
    public Invoice Invoice { get; set; } = null!;
}
