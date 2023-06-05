using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SparkChange.Domain.Configurations
{
    public class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> entity)
        {
            entity
                .ToTable("Basket")
                .HasKey(p => p.Id);

            entity
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity
                .Property(p => p.CustomerId)
                .IsRequired();

            entity
                .Property(p => p.Currency)
                .HasConversion<string>()
                .IsRequired();

            entity
                .HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(x => x.BasketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
