using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mass.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_stories_story_id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "fk_stories_mass_identity_user_created_by_id",
                table: "stories");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_story_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "story_id",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "story_contributor",
                columns: table => new
                {
                    contributor_id = table.Column<string>(type: "text", nullable: false),
                    story_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_story_contributor", x => new { x.contributor_id, x.story_id });
                    table.ForeignKey(
                        name: "fk_story_contributor_asp_net_users_contributor_id",
                        column: x => x.contributor_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_story_contributor_stories_story_id",
                        column: x => x.story_id,
                        principalTable: "stories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_story_contributor_story_id",
                table: "story_contributor",
                column: "story_id");

            migrationBuilder.AddForeignKey(
                name: "fk_stories_asp_net_users_created_by_id",
                table: "stories",
                column: "created_by_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_stories_asp_net_users_created_by_id",
                table: "stories");

            migrationBuilder.DropTable(
                name: "story_contributor");

            migrationBuilder.AddColumn<int>(
                name: "story_id",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_story_id",
                table: "AspNetUsers",
                column: "story_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_stories_story_id",
                table: "AspNetUsers",
                column: "story_id",
                principalTable: "stories",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_stories_mass_identity_user_created_by_id",
                table: "stories",
                column: "created_by_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
