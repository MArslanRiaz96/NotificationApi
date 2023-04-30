using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.Data.Migrations
{
    /// <inheritdoc />
    public partial class removesoftdeleteconnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "HubConnections");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "HubConnections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
