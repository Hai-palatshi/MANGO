using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.ShoppingCartAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddShoppingCartTablenewproperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CardHeaderId",
                table: "CartHeader",
                newName: "CartHeaderId");

            migrationBuilder.AddColumn<int>(
                name: "ProductId1",
                table: "CardDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductDto",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDto", x => x.ProductId);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardDetail_ProductDto_ProductId1",
                table: "CardDetail");

            migrationBuilder.DropTable(
                name: "ProductDto");

            migrationBuilder.DropIndex(
                name: "IX_CardDetail_ProductId1",
                table: "CardDetail");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "CardDetail");

            migrationBuilder.RenameColumn(
                name: "CartHeaderId",
                table: "CartHeader",
                newName: "CardHeaderId");
        }
    }
}
