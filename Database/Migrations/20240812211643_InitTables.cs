using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class InitTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "journals",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<long>(type: "bigint", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_journals", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "trees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    first_parent_id = table.Column<int>(type: "integer", nullable: true),
                    parent_id = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trees", x => x.id);
                    table.ForeignKey(
                        name: "FK_trees_trees_first_parent_id",
                        column: x => x.first_parent_id,
                        principalTable: "trees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_trees_trees_parent_id",
                        column: x => x.parent_id,
                        principalTable: "trees",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_journals_event_id",
                table: "journals",
                column: "event_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trees_first_parent_id",
                table: "trees",
                column: "first_parent_id");

            migrationBuilder.CreateIndex(
                name: "IX_trees_name_first_parent_id_parent_id",
                table: "trees",
                columns: new[] { "name", "first_parent_id", "parent_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_trees_parent_id",
                table: "trees",
                column: "parent_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "journals");

            migrationBuilder.DropTable(
                name: "trees");
        }
    }
}
