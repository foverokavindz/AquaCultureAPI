using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaCulture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueGpsLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "worker",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_fish_farm_GpsLocation_Latitude_GpsLocation_Longitude",
                table: "fish_farm",
                columns: new[] { "GpsLocation_Latitude", "GpsLocation_Longitude" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_fish_farm_GpsLocation_Latitude_GpsLocation_Longitude",
                table: "fish_farm");

            migrationBuilder.AlterColumn<string>(
                name: "Position",
                table: "worker",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
