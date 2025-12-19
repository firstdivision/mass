using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace mass.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chapter_mass_identity_user_created_by_id",
                table: "chapter");

            migrationBuilder.DropForeignKey(
                name: "fk_chapter_stories_story_id",
                table: "chapter");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chapter",
                table: "chapter");

            migrationBuilder.RenameTable(
                name: "chapter",
                newName: "chapters");

            migrationBuilder.RenameIndex(
                name: "ix_chapter_story_id",
                table: "chapters",
                newName: "ix_chapters_story_id");

            migrationBuilder.RenameIndex(
                name: "ix_chapter_created_by_id",
                table: "chapters",
                newName: "ix_chapters_created_by_id");

            migrationBuilder.AlterColumn<string>(
                name: "created_by_id",
                table: "chapters",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "pk_chapters",
                table: "chapters",
                column: "id");

            migrationBuilder.CreateTable(
                name: "entries",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    last_modified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    chapter_id = table.Column<int>(type: "integer", nullable: false),
                    created_by_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_entries", x => x.id);
                    table.ForeignKey(
                        name: "fk_entries_chapters_chapter_id",
                        column: x => x.chapter_id,
                        principalTable: "chapters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_entries_mass_identity_user_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_entries_chapter_id",
                table: "entries",
                column: "chapter_id");

            migrationBuilder.CreateIndex(
                name: "ix_entries_created_by_id",
                table: "entries",
                column: "created_by_id");

            migrationBuilder.AddForeignKey(
                name: "fk_chapters_mass_identity_user_created_by_id",
                table: "chapters",
                column: "created_by_id",
                principalTable: "AspNetUsers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_chapters_stories_story_id",
                table: "chapters",
                column: "story_id",
                principalTable: "stories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chapters_mass_identity_user_created_by_id",
                table: "chapters");

            migrationBuilder.DropForeignKey(
                name: "fk_chapters_stories_story_id",
                table: "chapters");

            migrationBuilder.DropTable(
                name: "entries");

            migrationBuilder.DropPrimaryKey(
                name: "pk_chapters",
                table: "chapters");

            migrationBuilder.RenameTable(
                name: "chapters",
                newName: "chapter");

            migrationBuilder.RenameIndex(
                name: "ix_chapters_story_id",
                table: "chapter",
                newName: "ix_chapter_story_id");

            migrationBuilder.RenameIndex(
                name: "ix_chapters_created_by_id",
                table: "chapter",
                newName: "ix_chapter_created_by_id");

            migrationBuilder.AlterColumn<string>(
                name: "created_by_id",
                table: "chapter",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_chapter",
                table: "chapter",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_chapter_mass_identity_user_created_by_id",
                table: "chapter",
                column: "created_by_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_chapter_stories_story_id",
                table: "chapter",
                column: "story_id",
                principalTable: "stories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
