using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatechatmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnvironmentId",
                table: "NotificationChats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSpecific",
                table: "NotificationChats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "NotificationProducts",
                keyColumn: "Id",
                keyValue: "11db7c7d-2ddb-49b6-9c40-ff4dc23a7730",
                columns: new[] { "CreatedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2023, 10, 27, 12, 13, 20, 875, DateTimeKind.Utc).AddTicks(4902), new DateTime(2023, 10, 27, 12, 13, 20, 875, DateTimeKind.Utc).AddTicks(4903) });

            migrationBuilder.UpdateData(
                table: "NotificationProducts",
                keyColumn: "Id",
                keyValue: "56730618-A053-4605-BFA0-42DC6CBE0CF7",
                columns: new[] { "CreatedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2023, 10, 27, 12, 13, 20, 875, DateTimeKind.Utc).AddTicks(4959), new DateTime(2023, 10, 27, 12, 13, 20, 875, DateTimeKind.Utc).AddTicks(4960) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "NotificationChats");

            migrationBuilder.DropColumn(
                name: "IsSpecific",
                table: "NotificationChats");

            migrationBuilder.UpdateData(
                table: "NotificationProducts",
                keyColumn: "Id",
                keyValue: "11db7c7d-2ddb-49b6-9c40-ff4dc23a7730",
                columns: new[] { "CreatedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2023, 10, 27, 12, 0, 52, 592, DateTimeKind.Utc).AddTicks(2607), new DateTime(2023, 10, 27, 12, 0, 52, 592, DateTimeKind.Utc).AddTicks(2608) });

            migrationBuilder.UpdateData(
                table: "NotificationProducts",
                keyColumn: "Id",
                keyValue: "56730618-A053-4605-BFA0-42DC6CBE0CF7",
                columns: new[] { "CreatedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2023, 10, 27, 12, 0, 52, 592, DateTimeKind.Utc).AddTicks(2666), new DateTime(2023, 10, 27, 12, 0, 52, 592, DateTimeKind.Utc).AddTicks(2667) });
        }
    }
}
