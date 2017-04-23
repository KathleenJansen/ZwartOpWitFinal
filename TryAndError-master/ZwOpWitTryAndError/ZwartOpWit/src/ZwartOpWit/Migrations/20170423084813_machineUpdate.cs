using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZwartOpWit.Migrations
{
    public partial class machineUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobLineType",
                table: "JobLine");

            migrationBuilder.AddColumn<int>(
                name: "MachineType",
                table: "Machine",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MachineType",
                table: "JobLine",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachineType",
                table: "Machine");

            migrationBuilder.DropColumn(
                name: "MachineType",
                table: "JobLine");

            migrationBuilder.AddColumn<int>(
                name: "JobLineType",
                table: "JobLine",
                nullable: false,
                defaultValue: 0);
        }
    }
}
