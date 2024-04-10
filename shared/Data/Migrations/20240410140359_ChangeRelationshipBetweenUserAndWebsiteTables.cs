using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRelationshipBetweenUserAndWebsiteTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Websites_WebsiteId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_WebsiteId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "WebsiteId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Websites",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Websites_UserId",
                table: "Websites",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Websites_AspNetUsers_UserId",
                table: "Websites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Websites_AspNetUsers_UserId",
                table: "Websites");

            migrationBuilder.DropIndex(
                name: "IX_Websites_UserId",
                table: "Websites");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Websites");

            migrationBuilder.AddColumn<Guid>(
                name: "WebsiteId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
