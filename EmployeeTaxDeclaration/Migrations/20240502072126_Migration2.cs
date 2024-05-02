using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmployeeTaxDeclaration.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxForm_AspNetUsers_UserId",
                table: "TaxForm");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaxForm",
                table: "TaxForm");

            migrationBuilder.DropIndex(
                name: "IX_TaxForm_FinancialYear",
                table: "TaxForm");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58fda1e2-03ae-4b93-9281-ffe437a800c4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c26d3fa-825e-405b-98dd-d573385a0560");

            migrationBuilder.RenameTable(
                name: "TaxForm",
                newName: "TaxForms");

            migrationBuilder.RenameIndex(
                name: "IX_TaxForm_UserId",
                table: "TaxForms",
                newName: "IX_TaxForms_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaxForms",
                table: "TaxForms",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1f0eac60-4736-438d-a282-7b6a2b5c5338", null, "client", "client" },
                    { "a44fb154-c877-46e0-9d86-fe57c498d742", null, "admin", "admin" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TaxForms_AspNetUsers_UserId",
                table: "TaxForms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaxForms_AspNetUsers_UserId",
                table: "TaxForms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaxForms",
                table: "TaxForms");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f0eac60-4736-438d-a282-7b6a2b5c5338");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a44fb154-c877-46e0-9d86-fe57c498d742");

            migrationBuilder.RenameTable(
                name: "TaxForms",
                newName: "TaxForm");

            migrationBuilder.RenameIndex(
                name: "IX_TaxForms_UserId",
                table: "TaxForm",
                newName: "IX_TaxForm_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaxForm",
                table: "TaxForm",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "58fda1e2-03ae-4b93-9281-ffe437a800c4", null, "client", "client" },
                    { "7c26d3fa-825e-405b-98dd-d573385a0560", null, "admin", "admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxForm_FinancialYear",
                table: "TaxForm",
                column: "FinancialYear",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TaxForm_AspNetUsers_UserId",
                table: "TaxForm",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
