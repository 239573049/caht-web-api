using Chat.Core.Entities.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Repository.Mappings.Groups;
/// <summary>
/// 群列表配置
/// </summary>
public class GroupsListUserInfosConfiguration : EntityConfiguration<GroupsListUserInfos>
{
    public override void Configure(EntityTypeBuilder<GroupsListUserInfos> builder)
    {
        builder.ToTable("Chat_GroupsListUserInfos");
        base.Configure(builder);
    }
}
