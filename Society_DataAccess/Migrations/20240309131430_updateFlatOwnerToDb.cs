using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateFlatOwnerToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlatOwner_Society_SocietyId",
                table: "FlatOwner");

            migrationBuilder.DropIndex(
                name: "IX_FlatOwner_SocietyId",
                table: "FlatOwner");

            migrationBuilder.DropColumn(
                name: "SocietyId",
                table: "FlatOwner");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SocietyId",
                table: "FlatOwner",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FlatOwner_SocietyId",
                table: "FlatOwner",
                column: "SocietyId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlatOwner_Society_SocietyId",
                table: "FlatOwner",
                column: "SocietyId",
                principalTable: "Society",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
