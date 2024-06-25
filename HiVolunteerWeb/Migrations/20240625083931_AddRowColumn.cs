using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiVolunteerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddRowColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPeopleCount",
                table: "Volunteerings");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "AspNetUsers",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "MaxPeopleCount",
                table: "Volunteerings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
