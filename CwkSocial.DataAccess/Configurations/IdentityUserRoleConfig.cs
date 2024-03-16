using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;
using CwkSocial.DataAccess.Models;

namespace CwkSocial.DataAccess.Configurations;

internal class IdentityUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        //builder.HasKey(iur => iur.RoleId);
        builder.HasKey(iur => new { iur.UserId, iur.RoleId }); // Configure composite primary key
    }
}
