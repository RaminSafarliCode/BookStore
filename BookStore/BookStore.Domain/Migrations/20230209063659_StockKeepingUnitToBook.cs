using Microsoft.EntityFrameworkCore.Migrations;

namespace BookStore.Domain.Migrations
{
    public partial class StockKeepingUnitToBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StockKeepingUnit",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockKeepingUnit",
                table: "Books");
        }
    }
}
