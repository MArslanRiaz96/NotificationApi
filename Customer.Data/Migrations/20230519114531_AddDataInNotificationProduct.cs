using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Customer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDataInNotificationProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "NotificationProducts",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "IsActive", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { "11db7c7d-2ddb-49b6-9c40-ff4dc23a7730", "arslan", new DateTime(2023, 5, 19, 11, 45, 31, 167, DateTimeKind.Utc).AddTicks(703), true, "arslan", new DateTime(2023, 5, 19, 11, 45, 31, 167, DateTimeKind.Utc).AddTicks(706), "PartnerLinq US" },
                    { "56730618-A053-4605-BFA0-42DC6CBE0CF7", "arslan", new DateTime(2023, 5, 19, 11, 45, 31, 167, DateTimeKind.Utc).AddTicks(736), true, "arslan", new DateTime(2023, 5, 19, 11, 45, 31, 167, DateTimeKind.Utc).AddTicks(736), "Data Fabric" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
