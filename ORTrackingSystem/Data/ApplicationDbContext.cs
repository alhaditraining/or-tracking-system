using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ORTrackingSystem.Models;

namespace ORTrackingSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    // DbSets for business entities
    public DbSet<ContractContext> ContractContexts { get; set; }
    public DbSet<OR> ORs { get; set; }
    public DbSet<LM_Request> LM_Requests { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceRequestLink> InvoiceRequestLinks { get; set; }
    public DbSet<InvoiceAttachment> InvoiceAttachments { get; set; }
    public DbSet<StationeryInvoice> StationeryInvoices { get; set; }
    public DbSet<StationeryAttachment> StationeryAttachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ContractContext configuration
        modelBuilder.Entity<ContractContext>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.ContractName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedByUserId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.CreatedDate).IsRequired();

            // Unique constraint on ContractName
            entity.HasIndex(e => e.ContractName).IsUnique();

            // Foreign key to ApplicationUser
            entity.HasOne(e => e.CreatedByUser)
                .WithMany()
                .HasForeignKey(e => e.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // OR configuration
        modelBuilder.Entity<OR>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.ORNumber).IsRequired().HasMaxLength(100);
            entity.Property(e => e.BudgetType).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.TotalValue).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedByUserId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.CreatedDate).IsRequired();

            // Unique constraint on ORNumber
            entity.HasIndex(e => e.ORNumber).IsUnique();

            // Foreign key to ContractContext
            entity.HasOne(e => e.ContractContext)
                .WithMany(c => c.ORs)
                .HasForeignKey(e => e.ContractContextId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // LM_Request configuration
        modelBuilder.Entity<LM_Request>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.RequestNumber).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedByUserId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.CreatedDate).IsRequired();

            // Unique constraint on (ORId, RequestNumber)
            entity.HasIndex(e => new { e.ORId, e.RequestNumber }).IsUnique();

            // Foreign key to OR
            entity.HasOne(e => e.OR)
                .WithMany(o => o.LM_Requests)
                .HasForeignKey(e => e.ORId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Invoice configuration
        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.CreatedByUserId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.CreatedDate).IsRequired();

            // Unique constraint on (ORId, InvoiceNumber)
            entity.HasIndex(e => new { e.ORId, e.InvoiceNumber }).IsUnique();

            // Foreign key to OR
            entity.HasOne(e => e.OR)
                .WithMany(o => o.Invoices)
                .HasForeignKey(e => e.ORId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // InvoiceRequestLink configuration (composite key)
        modelBuilder.Entity<InvoiceRequestLink>(entity =>
        {
            entity.HasKey(e => new { e.InvoiceId, e.RequestId });

            // Foreign key to Invoice
            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.InvoiceRequestLinks)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Foreign key to LM_Request
            entity.HasOne(e => e.Request)
                .WithMany(r => r.InvoiceRequestLinks)
                .HasForeignKey(e => e.RequestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // InvoiceAttachment configuration
        modelBuilder.Entity<InvoiceAttachment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.AttachmentType).IsRequired();
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FileSize).IsRequired();
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.CreatedByUserId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.UploadedDate).IsRequired();

            // Unique constraint on (InvoiceId, AttachmentType)
            entity.HasIndex(e => new { e.InvoiceId, e.AttachmentType }).IsUnique();

            // Foreign key to Invoice
            entity.HasOne(e => e.Invoice)
                .WithMany(i => i.InvoiceAttachments)
                .HasForeignKey(e => e.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // StationeryInvoice configuration
        modelBuilder.Entity<StationeryInvoice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.InvoiceNumber).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedByUserId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.CreatedDate).IsRequired();

            // Unique constraint on InvoiceNumber
            entity.HasIndex(e => e.InvoiceNumber).IsUnique();
        });

        // StationeryAttachment configuration
        modelBuilder.Entity<StationeryAttachment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).UseIdentityColumn();
            entity.Property(e => e.AttachmentType).IsRequired();
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FileSize).IsRequired();
            entity.Property(e => e.ContentType).HasMaxLength(100);
            entity.Property(e => e.CreatedByUserId).IsRequired().HasMaxLength(450);
            entity.Property(e => e.UploadedDate).IsRequired();

            // Unique constraint on (StationeryInvoiceId, AttachmentType)
            entity.HasIndex(e => new { e.StationeryInvoiceId, e.AttachmentType }).IsUnique();

            // Foreign key to StationeryInvoice
            entity.HasOne(e => e.StationeryInvoice)
                .WithMany(s => s.StationeryAttachments)
                .HasForeignKey(e => e.StationeryInvoiceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
