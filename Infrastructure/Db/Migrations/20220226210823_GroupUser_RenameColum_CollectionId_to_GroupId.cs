using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Db.Migrations
{
    public partial class GroupUser_RenameColum_CollectionId_to_GroupId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Groups_CollectionId",
                table: "GroupUsers");

            migrationBuilder.RenameColumn(
                name: "CollectionId",
                table: "GroupUsers",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupUsers_CollectionId",
                table: "GroupUsers",
                newName: "IX_GroupUsers_GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Groups_GroupId",
                table: "GroupUsers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupUsers_Groups_GroupId",
                table: "GroupUsers");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "GroupUsers",
                newName: "CollectionId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupUsers_GroupId",
                table: "GroupUsers",
                newName: "IX_GroupUsers_CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupUsers_Groups_CollectionId",
                table: "GroupUsers",
                column: "CollectionId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
