using Chat.Core.Entities.Groups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Repository.Mappings.Groups;

public class ApplicationRecordConfiguration : EntityConfiguration<ApplicationRecord>
{
    public override void Configure(EntityTypeBuilder<ApplicationRecord> builder)
    {
        builder.ToTable("Chat_ApplicationRecord");
        base.Configure(builder);
    }
}
