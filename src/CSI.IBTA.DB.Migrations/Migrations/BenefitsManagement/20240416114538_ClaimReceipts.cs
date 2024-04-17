using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSI.IBTA.DB.Migrations.Migrations.BenefitsManagement
{
    /// <inheritdoc />
    public partial class ClaimReceipts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Receipt",
                table: "Claim",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Receipt",
                table: "Claim");
        }
    }
}
