using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSI.IBTA.DB.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class Employers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employer_EmployerId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employer",
                table: "Employer");

            migrationBuilder.RenameTable(
                name: "Employer",
                newName: "Employers");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employers",
                table: "Employers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employers_EmployerId",
                table: "Users",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employers_EmployerId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employers",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Employers");

            migrationBuilder.RenameTable(
                name: "Employers",
                newName: "Employer");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "Employer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employer",
                table: "Employer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employer_EmployerId",
                table: "Users",
                column: "EmployerId",
                principalTable: "Employer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
