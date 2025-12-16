using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace mass.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStoryInvites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "story_invites",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    accepted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    story_id = table.Column<int>(type: "integer", nullable: false),
                    invited_user_id = table.Column<string>(type: "text", nullable: false),
                    invited_by_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_story_invites", x => x.id);
                    table.ForeignKey(
                        name: "fk_story_invites_mass_identity_user_invited_by_id",
                        column: x => x.invited_by_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_story_invites_mass_identity_user_invited_user_id",
                        column: x => x.invited_user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_story_invites_stories_story_id",
                        column: x => x.story_id,
                        principalTable: "stories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_story_invites_invited_by_id",
                table: "story_invites",
                column: "invited_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_story_invites_invited_user_id",
                table: "story_invites",
                column: "invited_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_story_invites_story_id",
                table: "story_invites",
                column: "story_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "story_invites");
        }
    }
}
