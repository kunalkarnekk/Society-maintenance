using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSocietyExpenseTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SocietyExpenseExpenseId",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SocietyExpenses",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocietyExpenses", x => x.ExpenseId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SocietyExpenseExpenseId",
                table: "Expenses",
                column: "SocietyExpenseExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_SocietyExpenses_SocietyExpenseExpenseId",
                table: "Expenses",
                column: "SocietyExpenseExpenseId",
                principalTable: "SocietyExpenses",
                principalColumn: "ExpenseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_SocietyExpenses_SocietyExpenseExpenseId",
                table: "Expenses");

            migrationBuilder.DropTable(
                name: "SocietyExpenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_SocietyExpenseExpenseId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SocietyExpenseExpenseId",
                table: "Expenses");
        }
    }
}
