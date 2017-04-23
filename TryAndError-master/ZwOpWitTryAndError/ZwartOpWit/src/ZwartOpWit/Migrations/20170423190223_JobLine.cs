using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZwartOpWit.Migrations
{
    public partial class JobLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "JobLine",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "MachineType",
                table: "JobLine",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_JobLine_DepartmentId",
                table: "JobLine",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobLine_Department_DepartmentId",
                table: "JobLine",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobLine_Department_DepartmentId",
                table: "JobLine");

            migrationBuilder.DropIndex(
                name: "IX_JobLine_DepartmentId",
                table: "JobLine");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "JobLine");

            migrationBuilder.DropColumn(
                name: "MachineType",
                table: "JobLine");
        }
    }
}
