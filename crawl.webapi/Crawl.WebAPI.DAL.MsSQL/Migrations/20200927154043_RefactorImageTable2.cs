using Microsoft.EntityFrameworkCore.Migrations;

namespace Crawl.WebAPI.DAL.MsSQL.Migrations
{
    public partial class RefactorImageTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_ImageUrl",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Images",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Images",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageUrl",
                table: "Images",
                column: "ImageUrl",
                unique: true);
        }
    }
}
