using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mass.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDateLastLoggedIn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "date_last_logged_in",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_last_logged_in",
                table: "AspNetUsers");
        }
    }
}
