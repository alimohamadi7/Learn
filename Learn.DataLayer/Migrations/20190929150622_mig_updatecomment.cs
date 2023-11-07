using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class mig_updatecomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_ParentId",
                table: "CourseComments",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseComments_CourseComments_ParentId",
                table: "CourseComments",
                column: "ParentId",
                principalTable: "CourseComments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_CourseComments_ParentId",
                table: "CourseComments");

            migrationBuilder.DropIndex(
                name: "IX_CourseComments_ParentId",
                table: "CourseComments");
        }
    }
}
