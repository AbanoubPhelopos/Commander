using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace commander.infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddKeyRegistrations : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "KeyRegistrations",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                KeyIndex = table.Column<Guid>(type: "uuid", nullable: false),
                Salt = table.Column<Guid>(type: "uuid", nullable: false),
                KeyHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                UserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_KeyRegistrations", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "Index_KeyRegistration_KeyIndex",
            table: "KeyRegistrations",
            column: "KeyIndex",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "KeyRegistrations");
    }
}
