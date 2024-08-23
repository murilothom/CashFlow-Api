using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CashFlow.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixAmountColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Expenses",
                type: "decimal(38,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Expenses",
                type: "decimal(38,30)",
                nullable: true);
        }
    }
}
