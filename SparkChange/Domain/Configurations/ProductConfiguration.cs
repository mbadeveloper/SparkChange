using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SparkChange.Contracts;

namespace SparkChange.Domain.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entity)
        {
            entity
            .ToTable("Product")
            .HasKey(p => p.Id);

            entity
                .Property(p => p.Id)
                 .ValueGeneratedOnAdd();

            entity
                .Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();

            entity
                .Property(p => p.Price)
                .HasPrecision(7,2)
                .IsRequired();

            entity
                .Property(p => p.Unit)
                .HasConversion(new EnumToNumberConverter<ProductUnitValue, byte>())
                .IsRequired();

            entity
                .Property(p => p.Currency)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}
