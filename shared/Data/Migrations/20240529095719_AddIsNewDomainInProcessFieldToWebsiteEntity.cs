using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsNewDomainInProcessFieldToWebsiteEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNewDomainInProcess",
                table: "Websites",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNewDomainInProcess",
                table: "Websites");
        }
    }
}
