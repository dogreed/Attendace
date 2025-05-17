using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Attendace.Migrations
{
    /// <inheritdoc />
    public partial class addbool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFlexible",
                table: "QRSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFlexible",
                table: "QRSessions");
        }
    }
}
