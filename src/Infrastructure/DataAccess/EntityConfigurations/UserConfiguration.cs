using ArticlesApp.Domain.Constants.EntityConstraints;
using ArticlesApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticlesApp.Infrastructure.DataAccess.EntityConfigurations
{
    public sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasQueryFilter(user => !user.IsDeleted);            

            builder.Property(x => x.FirstName).HasMaxLength(UserConstraints.FirstNameMaxLength);

            builder.Property(x => x.LastName).HasMaxLength(UserConstraints.LastNameMaxLength);  
        }
    }
}
