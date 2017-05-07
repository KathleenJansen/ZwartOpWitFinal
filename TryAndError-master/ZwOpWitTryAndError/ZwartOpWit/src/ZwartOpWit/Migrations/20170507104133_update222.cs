using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZwartOpWit.Migrations
{
    public partial class update222 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobReady",
                table: "JobLine",
                newName: "Completed");

            migrationBuilder.AddColumn<double>(
                name: "RunTimeFrom1000Speed",
                table: "Machine",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RunTimeFrom1000SpeedStationFactor",
                table: "Machine",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RunTimeTo1000Speed",
                table: "Machine",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RunTimeTo1000SpeedStationFactor",
                table: "Machine",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SetupTime",
                table: "Machine",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SetupTimeStationFactor",
                table: "Machine",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "CalculatedTime",
                table: "JobLine",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RunTimeFrom1000Speed",
                table: "Machine");

            migrationBuilder.DropColumn(
                name: "RunTimeFrom1000SpeedStationFactor",
                table: "Machine");

            migrationBuilder.DropColumn(
                name: "RunTimeTo1000Speed",
                table: "Machine");

            migrationBuilder.DropColumn(
                name: "RunTimeTo1000SpeedStationFactor",
                table: "Machine");

            migrationBuilder.DropColumn(
                name: "SetupTime",
                table: "Machine");

            migrationBuilder.DropColumn(
                name: "SetupTimeStationFactor",
                table: "Machine");

            migrationBuilder.DropColumn(
                name: "CalculatedTime",
                table: "JobLine");

            migrationBuilder.RenameColumn(
                name: "Completed",
                table: "JobLine",
                newName: "JobReady");
        }
    }
}
