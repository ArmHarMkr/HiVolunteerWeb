using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiVolunteerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Volunteerings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Volunteerings");
        }
    }
}
