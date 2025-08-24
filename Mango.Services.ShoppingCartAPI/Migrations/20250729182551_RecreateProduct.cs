using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class RecreateProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardDetail_ProductDto_ProductId1",
                table: "CardDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDto",
                table: "ProductDto");

            migrationBuilder.DropIndex(
                name: "IX_CardDetail_ProductId1",
                table: "CardDetail");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "CardDetail");

            // 🟥 Drop the old column with wrong identity
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductDto");

            // 🟩 Add new ProductId with IDENTITY again
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ProductDto",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // 🟦 Add back the primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDto",
                table: "ProductDto",
                column: "ProductId");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductDto",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "CardDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDto",
                table: "ProductDto",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CardDetail_ProductId1",
                table: "CardDetail",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CardDetail_ProductDto_ProductId1",
                table: "CardDetail",
                column: "ProductId1",
                principalTable: "ProductDto",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
