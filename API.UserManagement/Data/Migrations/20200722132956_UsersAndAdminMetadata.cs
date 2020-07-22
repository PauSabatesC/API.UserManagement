using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.UserManagement.Migrations
{
    public partial class UsersAndAdminMetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "adminActions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Action = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    UserAdminId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adminActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_adminActions_AspNetUsers_UserAdminId",
                        column: x => x.UserAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "usersMetaData",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    TimesPasswordReset = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usersMetaData", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_usersMetaData_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Historic",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    UserMetaDataUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historic_usersMetaData_UserMetaDataUserId",
                        column: x => x.UserMetaDataUserId,
                        principalTable: "usersMetaData",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatedBy",
                table: "AspNetUsers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UpdatedBy",
                table: "AspNetUsers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_adminActions_UserAdminId",
                table: "adminActions",
                column: "UserAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Historic_UserMetaDataUserId",
                table: "Historic",
                column: "UserMetaDataUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CreatedBy",
                table: "AspNetUsers",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_UpdatedBy",
                table: "AspNetUsers",
                column: "UpdatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CreatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_UpdatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "adminActions");

            migrationBuilder.DropTable(
                name: "Historic");

            migrationBuilder.DropTable(
                name: "usersMetaData");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CreatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UpdatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AspNetUsers");
        }
    }
}
