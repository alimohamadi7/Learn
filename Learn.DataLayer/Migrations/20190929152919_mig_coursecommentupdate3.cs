using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class mig_coursecommentupdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_CourseComments_ParentId",
                table: "CourseComments");

            migrationBuilder.DropIndex(
                name: "IX_CourseComments_ParentId",
                table: "CourseComments");

            migrationBuilder.AddColumn<int>(
                name: "CourseComment_1CommentId",
                table: "CourseComments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseComments_CourseComment_1CommentId",
                table: "CourseComments",
                column: "CourseComment_1CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseComments_CourseComments_CourseComment_1CommentId",
                table: "CourseComments",
                column: "CourseComment_1CommentId",
                principalTable: "CourseComments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseComments_CourseComments_CourseComment_1CommentId",
                table: "CourseComments");

            migrationBuilder.DropIndex(
                name: "IX_CourseComments_CourseComment_1CommentId",
                table: "CourseComments");

            migrationBuilder.DropColumn(
                name: "CourseComment_1CommentId",
                table: "CourseComments");

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
    }
}
