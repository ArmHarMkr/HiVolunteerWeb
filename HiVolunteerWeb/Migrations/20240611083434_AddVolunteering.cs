using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiVolunteerWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddVolunteering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VolunteeringEntityId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Volunteerings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VolunteeringName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VolunteeringDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeadLineDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsNeededAccept = table.Column<bool>(type: "bit", nullable: false),
                    IsFoodAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MaxPeopleCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteerings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_VolunteeringEntityId",
                table: "AspNetUsers",
                column: "VolunteeringEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Volunteerings_VolunteeringEntityId",
                table: "AspNetUsers",
                column: "VolunteeringEntityId",
                principalTable: "Volunteerings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Volunteerings_VolunteeringEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Volunteerings");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_VolunteeringEntityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "VolunteeringEntityId",
                table: "AspNetUsers");
        }
    }
}
