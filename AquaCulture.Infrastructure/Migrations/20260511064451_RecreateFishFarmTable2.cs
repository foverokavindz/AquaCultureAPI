using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaCulture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecreateFishFarmTable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fish_farm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GpsLocation_Latitude = table.Column<decimal>(type: "numeric(9,4)", nullable: false),
                    GpsLocation_Longitude = table.Column<decimal>(type: "numeric(9,4)", nullable: false),
                    NoOfCages = table.Column<int>(type: "integer", nullable: false),
                    HasBarge = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    PictureUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fish_farm", x => x.Id);
                });

            // GPS unique index
            migrationBuilder.CreateIndex(
                name: "IX_fish_farm_GpsLocation_Latitude_GpsLocation_Longitude",
                table: "fish_farm",
                columns: new[] { "GpsLocation_Latitude", "GpsLocation_Longitude" },
                unique: true);

            // Restore FK from worker → fish_farm
            migrationBuilder.AddForeignKey(
                name: "FK_worker_fish_farm_FishFarmId",
                table: "worker",
                column: "FishFarmId",
                principalTable: "fish_farm",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_worker_fish_farm_FishFarmId",
                table: "worker");

            migrationBuilder.DropTable(name: "fish_farm");
        }
    }
}
