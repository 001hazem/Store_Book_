using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.Web.Data.Migrations
{
    public partial class update_the_column_in_imageProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductImages",
                table: "Product",
                newName: "ImagesUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagesUrl",
                table: "Product",
                newName: "ProductImages");
        }
    }
}
