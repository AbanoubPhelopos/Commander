using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace commander.infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddCreatedAt : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        ArgumentNullException.ThrowIfNull(migrationBuilder);

        migrationBuilder.AddColumn<DateTime>(
            name: "CreatedAt",
            table: "Platforms",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        ArgumentNullException.ThrowIfNull(migrationBuilder);

        migrationBuilder.DropColumn(
            name: "CreatedAt",
            table: "Platforms");
    }
}
