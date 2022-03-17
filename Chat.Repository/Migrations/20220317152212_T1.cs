using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Repository.Migrations
{
    public partial class T1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chat_ApplicationRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BeAppliedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FriendStatue = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat_ApplicationRecord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chat_Friend",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BelongId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FriendsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat_Friend", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chat_GroupList",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat_GroupList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chat_GroupsListUserInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserInfoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat_GroupsListUserInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chat_UserInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WXOpenId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mobile = table.Column<long>(type: "bigint", maxLength: 11, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChatHead = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Statue = table.Column<int>(type: "int", nullable: false),
                    EMail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chat_UserInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chat_ApplicationRecord_Id",
                table: "Chat_ApplicationRecord",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_Friend_Id",
                table: "Chat_Friend",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_GroupList_Id",
                table: "Chat_GroupList",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_GroupsListUserInfos_GroupListId_UserInfoId",
                table: "Chat_GroupsListUserInfos",
                columns: new[] { "GroupListId", "UserInfoId" });

            migrationBuilder.CreateIndex(
                name: "IX_Chat_GroupsListUserInfos_Id",
                table: "Chat_GroupsListUserInfos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Chat_UserInfo_Id",
                table: "Chat_UserInfo",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chat_ApplicationRecord");

            migrationBuilder.DropTable(
                name: "Chat_Friend");

            migrationBuilder.DropTable(
                name: "Chat_GroupList");

            migrationBuilder.DropTable(
                name: "Chat_GroupsListUserInfos");

            migrationBuilder.DropTable(
                name: "Chat_UserInfo");
        }
    }
}
