using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class stripe3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscriptionStripeInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionStripeInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionModule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StripeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionModule_SubscriptionStripeInfo_StripeId",
                        column: x => x.StripeId,
                        principalTable: "SubscriptionStripeInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WebsiteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StripeCustomerId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StripeSubscriptionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SubscriptionModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscription_SubscriptionModule_SubscriptionModuleId",
                        column: x => x.SubscriptionModuleId,
                        principalTable: "SubscriptionModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_Websites_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Websites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_SubscriptionModuleId",
                table: "Subscription",
                column: "SubscriptionModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_WebsiteId",
                table: "Subscription",
                column: "WebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionModule_StripeId",
                table: "SubscriptionModule",
                column: "StripeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscription");

            migrationBuilder.DropTable(
                name: "SubscriptionModule");

            migrationBuilder.DropTable(
                name: "SubscriptionStripeInfo");
        }
    }
}
