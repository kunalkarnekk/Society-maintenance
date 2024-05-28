using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateFlatOwnerToDb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Maintenances_OwnerId",
                table: "Maintenances");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_OwnerId",
                table: "Maintenances",
                column: "OwnerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Maintenances_OwnerId",
                table: "Maintenances");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_OwnerId",
                table: "Maintenances",
                column: "OwnerId");
        }
    }
}
