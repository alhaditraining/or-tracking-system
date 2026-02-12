using System.ComponentModel.DataAnnotations;
using ORTrackingSystem.Data;

namespace ORTrackingSystem.Models;

public class ContractContext
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string ContractName { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    [Required]
    [MaxLength(450)]
    public string CreatedByUserId { get; set; } = string.Empty;

    // Navigation properties
    public ApplicationUser? CreatedByUser { get; set; }
    public ICollection<OR> ORs { get; set; } = new List<OR>();
}
