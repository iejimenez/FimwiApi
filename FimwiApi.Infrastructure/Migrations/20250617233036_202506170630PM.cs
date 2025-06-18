using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FimwiApi.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _202506170630PM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_TaxId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Customers",
                newName: "DocumentType");

            migrationBuilder.RenameColumn(
                name: "ContactName",
                table: "Customers",
                newName: "DocumentNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DocumentType",
                table: "Customers",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "DocumentNumber",
                table: "Customers",
                newName: "ContactName");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TaxId",
                table: "Customers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TaxId",
                table: "Customers",
                column: "TaxId",
                unique: true);
        }
    }
}
