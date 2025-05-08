using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NFTudio.Core.Models;

namespace NFTudio.Api.Data.Mappings;
public class LinkMapping : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ToTable("Links");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnType("TEXT").IsRequired(true);
        builder.Property(x => x.Type).HasColumnType("TEXT").IsRequired(true);
    }
}
