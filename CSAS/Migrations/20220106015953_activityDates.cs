using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSAS.Migrations
{
    public partial class activityDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Activities",
                newName: "Modified");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Activities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Activities");

            migrationBuilder.RenameColumn(
                name: "Modified",
                table: "Activities",
                newName: "Created");
        }
    }
}
