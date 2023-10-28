using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateisSpecific : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "NotificationProducts",
                keyColumn: "Id",
                keyValue: "11db7c7d-2ddb-49b6-9c40-ff4dc23a7730",
                columns: new[] { "CreatedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2023, 10, 9, 14, 43, 28, 957, DateTimeKind.Utc).AddTicks(2226), new DateTime(2023, 10, 9, 14, 43, 28, 957, DateTimeKind.Utc).AddTicks(2228) });

            migrationBuilder.UpdateData(
                table: "NotificationProducts",
                keyColumn: "Id",
                keyValue: "56730618-A053-4605-BFA0-42DC6CBE0CF7",
                columns: new[] { "CreatedOn", "ModifiedOn" },
                values: new object[] { new DateTime(2023, 10, 9, 14, 43, 28, 957, DateTimeKind.Utc).AddTicks(2328), new DateTime(2023, 10, 9, 14, 43, 28, 957, DateTimeKind.Utc).AddTicks(2329) });
        }
    }
}
