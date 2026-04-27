using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Servixa.Domain.Models.SpecialtyEntity;

namespace Servixa.Persistence.Configurations
{
    public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.ToTable("Specialty");
            builder.HasKey(s => s.Id);
        }
    }
}
