using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class mig_updatacoursecomment10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderComment",
                table: "CourseComments",
                maxLength: 700,
                nullable: false,
                oldClrType: typeof(int),
                oldMaxLength: 700,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OrderComment",
                table: "CourseComments",
                maxLength: 700,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 700);
        }
    }
}
