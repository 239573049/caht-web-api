using Chat.Core.Entities.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Repository.Mappings.Groups;

public class GroupListConfiguration : EntityConfiguration<GroupList>
{
    public override void Configure(EntityTypeBuilder<GroupList> builder)
    {
        builder.ToTable("Chat_GroupList");
        base.Configure(builder);
    }
}
