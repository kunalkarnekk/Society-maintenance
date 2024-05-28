using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addNewColumnSocietyExpensesInDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExpenseAmount",
                table: "Expenses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseAmount",
                table: "Expenses");
        }
    }
}
