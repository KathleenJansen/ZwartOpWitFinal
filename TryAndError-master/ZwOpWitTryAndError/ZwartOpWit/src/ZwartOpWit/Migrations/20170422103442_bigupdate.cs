using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ZwartOpWit.Migrations
{
    public partial class bigupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fold");

            migrationBuilder.DropTable(
                name: "Stitch");

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Cover = table.Column<int>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    Heigth = table.Column<int>(nullable: false),
                    JobNumber = table.Column<string>(nullable: true),
                    PageQuantity = table.Column<int>(nullable: false),
                    PaperBw = table.Column<string>(nullable: true),
                    PaperCover = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobLine",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    JobId = table.Column<int>(nullable: false),
                    JobLineType = table.Column<int>(nullable: false),
                    MachineId = table.Column<int>(nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobLine_Job_JobId",
                        column: x => x.JobId,
                        principalTable: "Job",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobLine_Machine_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobLine_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeRegister",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    JobLineId = table.Column<int>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    Stop = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeRegister", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeRegister_JobLine_JobLineId",
                        column: x => x.JobLineId,
                        principalTable: "JobLine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeRegister_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobLine_JobId",
                table: "JobLine",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLine_MachineId",
                table: "JobLine",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_JobLine_UserId",
                table: "JobLine",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRegister_JobLineId",
                table: "TimeRegister",
                column: "JobLineId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeRegister_UserId",
                table: "TimeRegister",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeRegister");

            migrationBuilder.DropTable(
                name: "JobLine");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.CreateTable(
                name: "Fold",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Heigth = table.Column<int>(nullable: false),
                    JobNumber = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fold", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stitch",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    Cover = table.Column<int>(nullable: false),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    Heigth = table.Column<int>(nullable: false),
                    JobNumber = table.Column<string>(nullable: true),
                    MachineId = table.Column<int>(nullable: false),
                    PageQuantity = table.Column<int>(nullable: false),
                    PaperBw = table.Column<string>(nullable: true),
                    PaperCover = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    StopDateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stitch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stitch_Machine_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stitch_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stitch_MachineId",
                table: "Stitch",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Stitch_UserId",
                table: "Stitch",
                column: "UserId");
        }
    }
}
