using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStore.Migrations
{
    public partial class AddProcessedDurationStringColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedDurationTicks",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "ProcessedDuration",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedDuration",
                table: "Orders");

            migrationBuilder.AddColumn<long>(
                name: "ProcessedDurationTicks",
                table: "Orders",
                type: "BIGINT",
                nullable: true);
        }
    }
}
