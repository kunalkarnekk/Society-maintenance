using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationSocietyAndExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Expenses");

            migrationBuilder.AlterColumn<string>(
                name: "ExpenseBy",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocietyId",
                table: "Expenses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SocietyId",
                table: "Expenses",
                column: "SocietyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Society_SocietyId",
                table: "Expenses",
                column: "SocietyId",
                principalTable: "Society",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Society_SocietyId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_SocietyId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SocietyId",
                table: "Expenses");

            migrationBuilder.AlterColumn<string>(
                name: "ExpenseBy",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Expenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
