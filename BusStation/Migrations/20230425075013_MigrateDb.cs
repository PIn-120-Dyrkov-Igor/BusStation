using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusStation.Migrations
{
    /// <inheritdoc />
    public partial class MigrateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Passers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Surname = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Patronymic = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PassportSeries = table.Column<int>(type: "int", maxLength: 4, nullable: false),
                    PassportNumber = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                    DateOfBird = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Passers");
        }
    }
}
