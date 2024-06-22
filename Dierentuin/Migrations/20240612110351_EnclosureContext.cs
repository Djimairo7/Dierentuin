using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin.Migrations
{
    /// <inheritdoc />
    public partial class EnclosureContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enclosure",
                table: "Animals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Enclosure",
                table: "Animals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
