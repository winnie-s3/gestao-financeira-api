using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryToIncome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Incomes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_CategoryId",
                table: "Incomes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_Categories_CategoryId",
                table: "Incomes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_Categories_CategoryId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_CategoryId",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Incomes");
        }
    }
}
