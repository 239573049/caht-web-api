using Chat.Core.Entities.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Repository.Mappings.Groups;

public class FriendConfguration : EntityConfiguration<Friend>
{
    public override void Configure(EntityTypeBuilder<Friend> builder)
    {
        builder.ToTable("Chat_Friend");
        base.Configure(builder);
    }
}