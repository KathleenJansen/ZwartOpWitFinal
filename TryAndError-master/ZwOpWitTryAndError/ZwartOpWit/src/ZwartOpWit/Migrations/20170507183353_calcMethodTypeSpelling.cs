using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZwartOpWit.Migrations
{
    public partial class calcMethodTypeSpelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "calculactionMethod",
                table: "Machine");

            migrationBuilder.AddColumn<int>(
                name: "calculationMethod",
                table: "Machine",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "calculationMethod",
                table: "Machine");

            migrationBuilder.AddColumn<int>(
                name: "calculactionMethod",
                table: "Machine",
                nullable: false,
                defaultValue: 0);
        }
    }
}
