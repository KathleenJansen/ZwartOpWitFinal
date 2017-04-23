using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZwartOpWit.Migrations
{
    public partial class machineUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachineType",
                table: "Machine");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Machine",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Machine");

            migrationBuilder.AddColumn<int>(
                name: "MachineType",
                table: "Machine",
                nullable: false,
                defaultValue: 0);
        }
    }
}
