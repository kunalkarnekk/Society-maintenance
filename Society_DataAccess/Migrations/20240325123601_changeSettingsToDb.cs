using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class changeSettingsToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaintenanceAmount",
                table: "Settings",
                newName: "RentMaintenanceCost");

            migrationBuilder.AddColumn<float>(
                name: "OwnerMaintenanceCost",
                table: "Settings",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerMaintenanceCost",
                table: "Settings");

            migrationBuilder.RenameColumn(
                name: "RentMaintenanceCost",
                table: "Settings",
                newName: "MaintenanceAmount");
        }
    }
}
