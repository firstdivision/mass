using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mass.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLockFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_locked",
                table: "stories",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "locked_by_id",
                table: "stories",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_stories_locked_by_id",
                table: "stories",
                column: "locked_by_id");

            migrationBuilder.AddForeignKey(
                name: "fk_stories_asp_net_users_locked_by_id",
                table: "stories",
                column: "locked_by_id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_stories_asp_net_users_locked_by_id",
                table: "stories");

            migrationBuilder.DropIndex(
                name: "ix_stories_locked_by_id",
                table: "stories");

            migrationBuilder.DropColumn(
                name: "is_locked",
                table: "stories");

            migrationBuilder.DropColumn(
                name: "locked_by_id",
                table: "stories");
        }
    }
}
