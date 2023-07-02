using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lets_leave.Migrations
{
    public partial class leaverequeststitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "LeaveRequests",
                type: "longtext",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "LeaveRequests");
        }
    }
}
