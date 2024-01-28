using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories;

public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new (){Name = "Manager",NormalizedName = "MANAGER"},
            new (){Name = "Administrator",NormalizedName = "ADMINISTRATOR"}
        );
    }
}
