using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Learn.DataLayer.Migrations
{
    public partial class mig_removereplaycomment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReplayCourseComments");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "CourseComments",
                nullable: true);

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

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "CourseComments");

            migrationBuilder.CreateTable(
                name: "ReplayCourseComments",
                columns: table => new
                {
                    ReplayCommentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Comment = table.Column<string>(maxLength: 700, nullable: true),
                    CommentId = table.Column<int>(nullable: false),
                    CourseId = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    IsAdminRead = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplayCourseComments", x => x.ReplayCommentId);
                    table.ForeignKey(
                        name: "FK_ReplayCourseComments_CourseComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "CourseComments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplayCourseComments_CommentId",
                table: "ReplayCourseComments",
                column: "CommentId");
        }
    }
}
