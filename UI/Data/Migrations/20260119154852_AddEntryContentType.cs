using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mass.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEntryContentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Convert existing plain text entries to HTML format
            // Wrap each line in <p> tags and replace line breaks with </p><p>
            migrationBuilder.Sql(@"
                UPDATE entries
                SET content = '<p>' || regexp_replace(content, '\n', '</p><p>', 'g') || '</p>'
                WHERE content IS NOT NULL AND content NOT LIKE '%<%';
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse migration: convert HTML back to plain text
            migrationBuilder.Sql(@"
                UPDATE entries
                SET content = regexp_replace(regexp_replace(content, '</?p>', '', 'g'), '<br\s*/?>', E'\n', 'gi')
                WHERE content IS NOT NULL AND content LIKE '%<%';
            ");
        }
    }
}


