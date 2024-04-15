using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shared.Data.Migrations
{
    /// <inheritdoc />
    public partial class stripe4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_SubscriptionModule_SubscriptionModuleId",
                table: "Subscription");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubscriptionModuleId",
                table: "Subscription",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_SubscriptionModule_SubscriptionModuleId",
                table: "Subscription",
                column: "SubscriptionModuleId",
                principalTable: "SubscriptionModule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_SubscriptionModule_SubscriptionModuleId",
                table: "Subscription");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubscriptionModuleId",
                table: "Subscription",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_SubscriptionModule_SubscriptionModuleId",
                table: "Subscription",
                column: "SubscriptionModuleId",
                principalTable: "SubscriptionModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
