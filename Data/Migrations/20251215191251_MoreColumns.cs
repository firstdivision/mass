using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace mass.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoreColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "stories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "stories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "last_modified_at",
                table: "stories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "story_id",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "chapter",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    last_modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    story_id = table.Column<int>(type: "integer", nullable: false),
                    created_by_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chapter", x => x.id);
                    table.ForeignKey(
                        name: "fk_chapter_mass_identity_user_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chapter_stories_story_id",
                        column: x => x.story_id,
                        principalTable: "stories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_story_id",
                table: "AspNetUsers",
                column: "story_id");

            migrationBuilder.CreateIndex(
                name: "ix_chapter_created_by_id",
                table: "chapter",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_chapter_story_id",
                table: "chapter",
                column: "story_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_stories_story_id",
                table: "AspNetUsers",
                column: "story_id",
                principalTable: "stories",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_stories_story_id",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "chapter");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_story_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "stories");

            migrationBuilder.DropColumn(
                name: "description",
                table: "stories");

            migrationBuilder.DropColumn(
                name: "last_modified_at",
                table: "stories");

            migrationBuilder.DropColumn(
                name: "story_id",
                table: "AspNetUsers");
        }
    }
}
