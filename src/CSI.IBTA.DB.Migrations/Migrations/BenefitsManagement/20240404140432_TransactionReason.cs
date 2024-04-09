using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSI.IBTA.DB.Migrations.Migrations.BenefitsManagement
{
    /// <inheritdoc />
    public partial class TransactionReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Reason",
                table: "Transaction",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Transaction");
        }
    }
}
