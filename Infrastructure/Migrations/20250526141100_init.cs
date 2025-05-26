using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOptions_Users_MyUserId",
                table: "UserOptions");

            migrationBuilder.DropTable(
                name: "UserTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOptions",
                table: "UserOptions");

            migrationBuilder.RenameTable(
                name: "UserOptions",
                newName: "UserOption");

            migrationBuilder.RenameIndex(
                name: "IX_UserOptions_MyUserId",
                table: "UserOption",
                newName: "IX_UserOption_MyUserId");

            migrationBuilder.AlterColumn<int>(
                name: "MyUserId",
                table: "UserOption",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "UserOption",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOption",
                table: "UserOption",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    MyUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTag_Users_MyUserId",
                        column: x => x.MyUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTag_MyUserId",
                table: "UserTag",
                column: "MyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserOption_Users_MyUserId",
                table: "UserOption",
                column: "MyUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOption_Users_MyUserId",
                table: "UserOption");

            migrationBuilder.DropTable(
                name: "UserTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOption",
                table: "UserOption");

            migrationBuilder.RenameTable(
                name: "UserOption",
                newName: "UserOptions");

            migrationBuilder.RenameIndex(
                name: "IX_UserOption_MyUserId",
                table: "UserOptions",
                newName: "IX_UserOptions_MyUserId");

            migrationBuilder.AlterColumn<int>(
                name: "MyUserId",
                table: "UserOptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "UserOptions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOptions",
                table: "UserOptions",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserTags",
                columns: table => new
                {
                    MyUserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTags", x => new { x.MyUserId, x.Title, x.Priority });
                    table.ForeignKey(
                        name: "FK_UserTags_Users_MyUserId",
                        column: x => x.MyUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_UserOptions_Users_MyUserId",
                table: "UserOptions",
                column: "MyUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
