using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NFTudio.Core.Models;

namespace NFTudio.Api.Data.Mappings;
public class AssociateMapping : IEntityTypeConfiguration<Associate>
{
    public void Configure(EntityTypeBuilder<Associate> builder)
    {
        builder.ToTable("Associates");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasColumnType("TEXT")
            .HasMaxLength(255)
            .IsRequired(true);
        
        builder.Property(x => x.Description)
            .HasColumnType("TEXT")
            .HasMaxLength(512)
            .IsRequired(false);

        builder.Property(x => x.Email)
            .HasColumnType("TEXT")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.Property(x => x.Benefit)
            .HasColumnType("TEXT")
            .HasMaxLength(512)
            .IsRequired(false);

        builder.Property(x => x.Situation)
            .HasColumnType("TEXT")
            .HasMaxLength(255)
            .IsRequired(true);

        builder.Property(x => x.AssociateImagemUrl)
            .HasColumnType("TEXT")
            .HasMaxLength(255)
            .IsRequired(false);

        builder.HasMany(x => x.Links).WithOne(x => x.Associate).HasForeignKey(x => x.AssociateId);
    }
}
