using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportCenter.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FIO = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Trainer",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FIO = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainer", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "GroupTrain",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Trainer = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Capacity = table.Column<int>(nullable: false),
                    DayOfWeek = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTrain", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GroupTrain_Trainer_ID",
                        column: x => x.ID_Trainer,
                        principalTable: "Trainer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderGroup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_GroupTrain = table.Column<int>(nullable: false),
                    ID_Client = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderGroup", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderGroup_Client_ID",
                        column: x => x.ID_Client,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderGroup_GroupTrain_ID",
                        column: x => x.ID_GroupTrain,
                        principalTable: "GroupTrain",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupTrain_ID_Trainer",
                table: "GroupTrain",
                column: "ID_Trainer");

            migrationBuilder.CreateIndex(
                name: "IX_OrderGroup_ID_Client",
                table: "OrderGroup",
                column: "ID_Client");

            migrationBuilder.CreateIndex(
                name: "IX_OrderGroup_ID_GroupTrain",
                table: "OrderGroup",
                column: "ID_GroupTrain");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderGroup");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "GroupTrain");

            migrationBuilder.DropTable(
                name: "Trainer");
        }
    }
}
