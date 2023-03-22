using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Domain.Migrations
{
    public partial class AboutCompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkHours = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedinAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutCompany", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookStoreForgotPasswords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedUserId = table.Column<int>(type: "int", nullable: true),
                    DeletedByUserId = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookStoreForgotPasswords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookStoreForgotPasswords_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Membership",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookStoreForgotPasswords_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Membership",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookStoreForgotPasswords_CreatedByUserId",
                table: "BookStoreForgotPasswords",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookStoreForgotPasswords_DeletedByUserId",
                table: "BookStoreForgotPasswords",
                column: "DeletedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutCompany");

            migrationBuilder.DropTable(
                name: "BookStoreForgotPasswords");
        }
    }
}
