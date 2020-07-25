using Microsoft.EntityFrameworkCore.Migrations;

namespace UserManagement.Infrastructure.Migrations
{
    public partial class UsersMetadataModifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historic_usersMetaData_UserMetaDataUserId",
                table: "Historic");

            migrationBuilder.DropForeignKey(
                name: "FK_usersMetaData_AspNetUsers_UserId",
                table: "usersMetaData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usersMetaData",
                table: "usersMetaData");

            migrationBuilder.DropIndex(
                name: "IX_Historic_UserMetaDataUserId",
                table: "Historic");

            migrationBuilder.DropColumn(
                name: "UserMetaDataUserId",
                table: "Historic");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "usersMetaData",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "usersMetaData",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserMetaDataId",
                table: "Historic",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_usersMetaData",
                table: "usersMetaData",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_usersMetaData_UserId",
                table: "usersMetaData",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Historic_UserMetaDataId",
                table: "Historic",
                column: "UserMetaDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historic_usersMetaData_UserMetaDataId",
                table: "Historic",
                column: "UserMetaDataId",
                principalTable: "usersMetaData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_usersMetaData_AspNetUsers_UserId",
                table: "usersMetaData",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historic_usersMetaData_UserMetaDataId",
                table: "Historic");

            migrationBuilder.DropForeignKey(
                name: "FK_usersMetaData_AspNetUsers_UserId",
                table: "usersMetaData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_usersMetaData",
                table: "usersMetaData");

            migrationBuilder.DropIndex(
                name: "IX_usersMetaData_UserId",
                table: "usersMetaData");

            migrationBuilder.DropIndex(
                name: "IX_Historic_UserMetaDataId",
                table: "Historic");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "usersMetaData");

            migrationBuilder.DropColumn(
                name: "UserMetaDataId",
                table: "Historic");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "usersMetaData",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserMetaDataUserId",
                table: "Historic",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_usersMetaData",
                table: "usersMetaData",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Historic_UserMetaDataUserId",
                table: "Historic",
                column: "UserMetaDataUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historic_usersMetaData_UserMetaDataUserId",
                table: "Historic",
                column: "UserMetaDataUserId",
                principalTable: "usersMetaData",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_usersMetaData_AspNetUsers_UserId",
                table: "usersMetaData",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
