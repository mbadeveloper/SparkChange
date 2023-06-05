using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SparkChange.Domain.Configurations
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> entity)
        {
            entity
                .ToTable("BasketItem")
                .HasKey(p => p.Id);

            entity
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            entity
                .Property(p => p.BasketId)
                .IsRequired();

            entity
                .Property(p => p.ProductId)
                .IsRequired();

            entity
                .Property(p => p.Quantity)
                .IsRequired();

            entity
                  .HasOne(x => x.Product)
                  .WithOne();

        }
    }
}
