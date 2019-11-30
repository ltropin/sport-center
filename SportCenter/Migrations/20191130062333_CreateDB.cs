using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportCenter.Migrations
{
    public partial class CreateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
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
                name: "Client",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FIO = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    ID_Role = table.Column<int>(nullable: false, defaultValueSql: "((1))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Client_Role_ID",
                        column: x => x.ID_Role,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupTrain",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Capacity = table.Column<int>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    DayOfWeek = table.Column<int>(nullable: false),
                    ID_Trainer = table.Column<int>(nullable: false)
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
                name: "Abonement",
                columns: table => new
                {
                    Number = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(nullable: false),
                    DateBlock = table.Column<DateTime>(type: "date", nullable: true),
                    IntervalBlock = table.Column<int>(nullable: true),
                    Capacity = table.Column<int>(nullable: false),
                    ID_Client = table.Column<int>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    Term = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonement_ID", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Abonement_Client_ID",
                        column: x => x.ID_Client,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalTrain",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    ID_Trainer = table.Column<int>(nullable: false),
                    ID_Client = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalTrain", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PersonalTrain_Client_ID",
                        column: x => x.ID_Client,
                        principalTable: "Client",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalTrain_Trainer_ID",
                        column: x => x.ID_Trainer,
                        principalTable: "Trainer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestAbonement",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Client = table.Column<int>(nullable: false),
                    Term = table.Column<int>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAbonement", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RequestAbonement_Client_ID",
                        column: x => x.ID_Client,
                        principalTable: "Client",
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
                name: "IX_Abonement_ID_Client",
                table: "Abonement",
                column: "ID_Client");

            migrationBuilder.CreateIndex(
                name: "IX_Client_ID_Role",
                table: "Client",
                column: "ID_Role");

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

            migrationBuilder.CreateIndex(
                name: "IX_PersonalTrain_ID_Client",
                table: "PersonalTrain",
                column: "ID_Client");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalTrain_ID_Trainer",
                table: "PersonalTrain",
                column: "ID_Trainer");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAbonement_ID_Client",
                table: "RequestAbonement",
                column: "ID_Client");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abonement");

            migrationBuilder.DropTable(
                name: "OrderGroup");

            migrationBuilder.DropTable(
                name: "PersonalTrain");

            migrationBuilder.DropTable(
                name: "RequestAbonement");

            migrationBuilder.DropTable(
                name: "GroupTrain");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Trainer");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
