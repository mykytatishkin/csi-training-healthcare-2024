using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSI.IBTA.DB.Migrations.Migrations.BenefitsManagement
{
    /// <inheritdoc />
    public partial class ClaimEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claim_Plan_PlanId",
                table: "Claim");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Claim");

            migrationBuilder.RenameColumn(
                name: "PlanId",
                table: "Claim",
                newName: "EnrollmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Claim_PlanId",
                table: "Claim",
                newName: "IX_Claim_EnrollmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Claim_Enrollment_EnrollmentId",
                table: "Claim",
                column: "EnrollmentId",
                principalTable: "Enrollment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claim_Enrollment_EnrollmentId",
                table: "Claim");

            migrationBuilder.RenameColumn(
                name: "EnrollmentId",
                table: "Claim",
                newName: "PlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Claim_EnrollmentId",
                table: "Claim",
                newName: "IX_Claim_PlanId");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Claim",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Claim_Plan_PlanId",
                table: "Claim",
                column: "PlanId",
                principalTable: "Plan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
