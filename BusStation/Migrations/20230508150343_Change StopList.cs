using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusStation.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStopList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StopLists_Stops_StopId",
                table: "StopLists");

            migrationBuilder.DropIndex(
                name: "IX_StopLists_StopId",
                table: "StopLists");

            migrationBuilder.AddColumn<int>(
                name: "StopListId",
                table: "Stops",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_StopListId",
                table: "Stops",
                column: "StopListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_StopLists_StopListId",
                table: "Stops",
                column: "StopListId",
                principalTable: "StopLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_StopLists_StopListId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_StopListId",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "StopListId",
                table: "Stops");

            migrationBuilder.CreateIndex(
                name: "IX_StopLists_StopId",
                table: "StopLists",
                column: "StopId");

            migrationBuilder.AddForeignKey(
                name: "FK_StopLists_Stops_StopId",
                table: "StopLists",
                column: "StopId",
                principalTable: "Stops",
                principalColumn: "Id");
        }
    }
}
