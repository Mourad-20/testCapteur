using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Model.Migrations
{
    public partial class SeedCapteurs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Capteur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<double>(type: "double precision", nullable: true),
                    Dt_Modif = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capteur", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Capteur",
                columns: new[] { "Id", "Dt_Modif", "Name", "Type", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 12, 4, 21, 43, 25, 620, DateTimeKind.Utc).AddTicks(1394), "Capteur 01", "Type01", 22.5 },
                    { 2, new DateTime(2024, 12, 4, 21, 43, 25, 620, DateTimeKind.Utc).AddTicks(1942), "Capteur 02", "Type02", 55.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Capteur");
        }
    }
}
