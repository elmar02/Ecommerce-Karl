using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "CategoryLanguages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CategoryLanguages_CategoryId1",
                table: "CategoryLanguages",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryLanguages_Categories_CategoryId1",
                table: "CategoryLanguages",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryLanguages_Categories_CategoryId1",
                table: "CategoryLanguages");

            migrationBuilder.DropIndex(
                name: "IX_CategoryLanguages_CategoryId1",
                table: "CategoryLanguages");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "CategoryLanguages");
        }
    }
}
