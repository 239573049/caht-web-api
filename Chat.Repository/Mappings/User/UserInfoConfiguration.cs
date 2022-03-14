using Chat.Core.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Repository.Mappings.User;

public class UserInfoConfiguration : EntityConfiguration<UserInfo>
{
    public override void Configure(EntityTypeBuilder<UserInfo> builder)
    {
        builder.ToTable("Chat_UserInfo");
        base.Configure(builder);
    }
}