using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dierentuin.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Species = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Size = table.Column<int>(type: "INTEGER", nullable: false),
                    Diet = table.Column<int>(type: "INTEGER", nullable: false),
                    Activity = table.Column<int>(type: "INTEGER", nullable: false),
                    Prey = table.Column<string>(type: "TEXT", nullable: false),
                    Enclosure = table.Column<string>(type: "TEXT", nullable: false),
                    Space = table.Column<double>(type: "REAL", nullable: false),
                    Security = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animal");
        }
    }
}
