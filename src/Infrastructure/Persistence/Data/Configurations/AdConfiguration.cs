using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Data.Configurations
{
    public class AdConfiguration : IEntityTypeConfiguration<Ad>
    {


        public void Configure(EntityTypeBuilder<Ad> builder)
        {
            var statusEnumconverter = new EnumToNumberConverter<Status, int>();
            var propertyTypeEnumconverter = new EnumToNumberConverter<PropertyType, int>();

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder.Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.UserId)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(t => t.Price)
                .IsRequired();

            builder.Property(t => t.Location)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.PropertyType)                
                .IsRequired()
                .HasConversion(propertyTypeEnumconverter);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion(statusEnumconverter);


        }
    }
}
