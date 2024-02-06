using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSI.IBTA.DB.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberToTestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Test",
                columns: new[] { "test" },
                values: new object[] { 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Test",
                keyColumn: "test",
                keyValue: 1);
        }
    }
}
