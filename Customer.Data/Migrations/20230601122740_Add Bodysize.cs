using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBodysize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bodysize",
                table: "Notifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "NotificationProducts",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "IsActive", "ModifiedBy", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { "5d9edece-39ee-43de-a764-be370c012630", "arslan", new DateTime(2023, 5, 19, 11, 45, 31, 167, DateTimeKind.Utc).AddTicks(703), true, "arslan", new DateTime(2023, 5, 19, 11, 45, 31, 167, DateTimeKind.Utc).AddTicks(706), "PartnerLinq EUR" },
                    { "5d9edece-39ee-43de-a764-be370c01ffff", "arslan", new DateTime(2023, 5, 19, 11, 45, 31, 167, DateTimeKind.Utc).AddTicks(736), true, "arslan", new DateTime(2023, 5, 19, 11, 45, 31, 167, DateTimeKind.Utc).AddTicks(736), "Customer Portal" },
                    { "74D06F2D-ED92-4EE7-B383-EB634DEB3DAA", "arslan", new DateTime(2023, 5, 19, 11, 45, 31, 167, DateTimeKind.Utc).AddTicks(736), true, "arslan", new DateTime(2023, 5, 19, 11, 45, 31, 167, DateTimeKind.Utc).AddTicks(736), "AtClose Exchange" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
