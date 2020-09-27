using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Crawl.WebAPI.DAL.MsSQL.Migrations
{
    public partial class AddSiteAndImagesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    DbKey = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppKey = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.DbKey);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    DbKey = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppKey = table.Column<Guid>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: false),
                    Image = table.Column<byte[]>(nullable: false),
                    SiteDbKey = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.DbKey);
                    table.ForeignKey(
                        name: "FK_Images_Sites_SiteDbKey",
                        column: x => x.SiteDbKey,
                        principalTable: "Sites",
                        principalColumn: "DbKey",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageUrl",
                table: "Images",
                column: "ImageUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_SiteDbKey",
                table: "Images",
                column: "SiteDbKey");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_Url",
                table: "Sites",
                column: "Url",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Sites");
        }
    }
}
