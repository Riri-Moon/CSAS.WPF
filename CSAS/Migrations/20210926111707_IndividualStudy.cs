using Microsoft.EntityFrameworkCore.Migrations;

namespace CSAS.Migrations
{
    public partial class IndividualStudy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IndividualStudy",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndividualStudy",
                table: "Students");
        }
    }
}
