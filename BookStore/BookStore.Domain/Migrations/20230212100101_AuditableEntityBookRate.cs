using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Domain.Migrations
{
    public partial class AuditableEntityBookRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookRates",
                columns: table => new
                {
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedUserId = table.Column<int>(type: "int", nullable: true),
                    DeletedByUserId = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRates", x => new { x.BookId, x.CreatedByUserId });
                    table.ForeignKey(
                        name: "FK_BookRates_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookRates_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Membership",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookRates_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Membership",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookRates_CreatedByUserId",
                table: "BookRates",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRates_DeletedByUserId",
                table: "BookRates",
                column: "DeletedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookRates");
        }
    }
}
