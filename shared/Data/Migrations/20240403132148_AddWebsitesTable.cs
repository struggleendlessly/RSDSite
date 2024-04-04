using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWebsitesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteName",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "WebsiteId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Websites",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Websites", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_WebsiteId",
                table: "AspNetUsers",
                column: "WebsiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Websites_WebsiteId",
                table: "AspNetUsers",
                column: "WebsiteId",
                principalTable: "Websites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Websites_WebsiteId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Websites");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WebsiteId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WebsiteId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "SiteName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
