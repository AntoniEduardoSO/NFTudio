using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NFTudio.Core.Models;

namespace NFTudio.Api.Data.Mappings;
public class OperationMapping : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.ToTable("Operations");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnType("TEXT").HasMaxLength(255).IsRequired(true);
    }
}