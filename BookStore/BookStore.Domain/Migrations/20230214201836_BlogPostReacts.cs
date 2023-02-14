using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Domain.Migrations
{
    public partial class BlogPostReacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPostRates",
                columns: table => new
                {
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false),
                    BlogPostId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedUserId = table.Column<int>(type: "int", nullable: true),
                    DeletedByUserId = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostRates", x => new { x.BlogPostId, x.CreatedByUserId });
                    table.ForeignKey(
                        name: "FK_BlogPostRates_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostRates_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalSchema: "Membership",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostRates_Users_DeletedByUserId",
                        column: x => x.DeletedByUserId,
                        principalSchema: "Membership",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostRates_CreatedByUserId",
                table: "BlogPostRates",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostRates_DeletedByUserId",
                table: "BlogPostRates",
                column: "DeletedByUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostRates");
        }
    }
}
