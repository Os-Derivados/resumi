using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resumi.Infra.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveResumeNormalizedName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Resumes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Resumes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
