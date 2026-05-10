using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaCulture.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteAndNullableFarmId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_worker_fish_farm_FishFarmId",
                table: "worker");

            migrationBuilder.AlterColumn<Guid>(
                name: "FishFarmId",
                table: "worker",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "worker",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "fish_farm",
                type: "boolean",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "worker");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "fish_farm");

            migrationBuilder.AlterColumn<Guid>(
                name: "FishFarmId",
                table: "worker",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_worker_fish_farm_FishFarmId",
                table: "worker",
                column: "FishFarmId",
                principalTable: "fish_farm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
