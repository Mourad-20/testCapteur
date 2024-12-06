using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Model.Migrations
{
    public partial class AddAutoIncrementToCapteur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Capteur",
                keyColumn: "Id",
                keyValue: 1,
                column: "Dt_Modif",
                value: new DateTime(2024, 12, 5, 19, 36, 1, 537, DateTimeKind.Utc).AddTicks(9454));

            migrationBuilder.UpdateData(
                table: "Capteur",
                keyColumn: "Id",
                keyValue: 2,
                column: "Dt_Modif",
                value: new DateTime(2024, 12, 5, 19, 36, 1, 538, DateTimeKind.Utc).AddTicks(56));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Capteur",
                keyColumn: "Id",
                keyValue: 1,
                column: "Dt_Modif",
                value: new DateTime(2024, 12, 4, 21, 43, 25, 620, DateTimeKind.Utc).AddTicks(1394));

            migrationBuilder.UpdateData(
                table: "Capteur",
                keyColumn: "Id",
                keyValue: 2,
                column: "Dt_Modif",
                value: new DateTime(2024, 12, 4, 21, 43, 25, 620, DateTimeKind.Utc).AddTicks(1942));
        }
    }
}
