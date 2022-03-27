using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class counterUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "counter",
                table: "Bookmark",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "counter",
                table: "Bookmark");
        }
    }
}
