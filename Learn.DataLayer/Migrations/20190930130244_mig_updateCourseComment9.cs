using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class mig_updateCourseComment9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "CourseComments",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 700,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderComment",
                table: "CourseComments",
                maxLength: 700,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderComment",
                table: "CourseComments");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "CourseComments",
                maxLength: 700,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
