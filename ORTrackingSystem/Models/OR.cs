using System.ComponentModel.DataAnnotations;

namespace ORTrackingSystem.Models;

public class OR
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string ORNumber { get; set; } = string.Empty;

    public int ContractContextId { get; set; }

    public BudgetType BudgetType { get; set; }

    public ORStatus Status { get; set; } = ORStatus.Open;

    public decimal TotalValue { get; set; }

    public DateTime CreatedDate { get; set; }

    [Required]
    [MaxLength(450)]
    public string CreatedByUserId { get; set; } = string.Empty;

    // Navigation properties
    public ContractContext ContractContext { get; set; } = null!;
    public ICollection<LM_Request> LM_Requests { get; set; } = new List<LM_Request>();
    public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
