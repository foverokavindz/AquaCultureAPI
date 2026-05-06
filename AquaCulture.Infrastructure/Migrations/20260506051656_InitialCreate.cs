using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaCulture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
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
                    PictureUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fish_farm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "worker",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    CertifiedUntil = table.Column<DateOnly>(type: "date", nullable: false),
                    FishFarmId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_worker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_worker_fish_farm_FishFarmId",
                        column: x => x.FishFarmId,
                        principalTable: "fish_farm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_worker_Email",
                table: "worker",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_worker_FishFarmId",
                table: "worker",
                column: "FishFarmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "worker");

            migrationBuilder.DropTable(
                name: "fish_farm");
        }
    }
}
