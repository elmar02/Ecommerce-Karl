using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModels1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_CategoryLanguages_CategoryLanguageId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryLanguages_Categories_CategoryId1",
                table: "CategoryLanguages");

            migrationBuilder.DropIndex(
                name: "IX_ProductLanguages_ProductId",
                table: "ProductLanguages");

            migrationBuilder.DropIndex(
                name: "IX_CategoryLanguages_CategoryId1",
                table: "CategoryLanguages");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryLanguageId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "CategoryLanguages");

            migrationBuilder.DropColumn(
                name: "CategoryLanguageId",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLanguages_ProductId",
                table: "ProductLanguages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLanguages_CategoryId",
                table: "CategoryLanguages",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryLanguages_Categories_CategoryId",
                table: "CategoryLanguages",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryLanguages_Categories_CategoryId",
                table: "CategoryLanguages");

            migrationBuilder.DropIndex(
                name: "IX_ProductLanguages_ProductId",
                table: "ProductLanguages");

            migrationBuilder.DropIndex(
                name: "IX_CategoryLanguages_CategoryId",
                table: "CategoryLanguages");

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "CategoryLanguages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryLanguageId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductLanguages_ProductId",
                table: "ProductLanguages",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLanguages_CategoryId1",
                table: "CategoryLanguages",
                column: "CategoryId1");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryLanguageId",
                table: "Categories",
                column: "CategoryLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_CategoryLanguages_CategoryLanguageId",
                table: "Categories",
                column: "CategoryLanguageId",
                principalTable: "CategoryLanguages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryLanguages_Categories_CategoryId1",
                table: "CategoryLanguages",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
