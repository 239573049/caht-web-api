using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Repository.Migrations
{
    public partial class T3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_GroupsListUserInfos_Chat_UserInfo_UserInfoId",
                table: "Chat_GroupsListUserInfos");

            migrationBuilder.DropIndex(
                name: "IX_Chat_GroupsListUserInfos_UserInfoId",
                table: "Chat_GroupsListUserInfos");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Chat_GroupsListUserInfos_UserInfoId",
                table: "Chat_GroupsListUserInfos",
                column: "UserInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_GroupsListUserInfos_Chat_UserInfo_UserInfoId",
                table: "Chat_GroupsListUserInfos",
                column: "UserInfoId",
                principalTable: "Chat_UserInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
