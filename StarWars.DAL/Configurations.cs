using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarWars.DAL.Entities;

namespace StarWars.DAL;

public class FavoriteCharacterConfiguration : IEntityTypeConfiguration<FavoriteCharacter>
{
    public void Configure(EntityTypeBuilder<FavoriteCharacter> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(500);
    }
}

public class RequestHistoryConfiguration : IEntityTypeConfiguration<RequestHistory>
{
    public void Configure(EntityTypeBuilder<RequestHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Endpoint)
            .IsRequired();

        builder
            .Property(x => x.RequestedAt)
            .IsRequired();
    }
}