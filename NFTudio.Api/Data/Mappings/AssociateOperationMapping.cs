using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NFTudio.Core.Models;

namespace NFTudio.Api.Data.Mappings;
public class AssociateOperationMapping : IEntityTypeConfiguration<AssociateOperation>
{
    public void Configure(EntityTypeBuilder<AssociateOperation> builder)
    {
        builder.ToTable("AssociateOperations");

            builder.HasKey(ao => new { ao.AssociateId, ao.OperationId });

            builder.HasOne(ao => ao.Associate)
                   .WithMany(a => a.AssociateOperations)
                   .HasForeignKey(ao => ao.AssociateId);

            builder.HasOne(ao => ao.Operation)
                   .WithMany(o => o.AssociateOperations)
                   .HasForeignKey(ao => ao.OperationId);
    }
}