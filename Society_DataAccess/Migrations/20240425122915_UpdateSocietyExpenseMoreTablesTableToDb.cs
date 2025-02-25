﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Society_DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSocietyExpenseMoreTablesTableToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExpenseAmount",
                table: "SocietyExpenses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpenseDate",
                table: "SocietyExpenses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpenseAmount",
                table: "SocietyExpenses");

            migrationBuilder.DropColumn(
                name: "ExpenseDate",
                table: "SocietyExpenses");
        }
    }
}
