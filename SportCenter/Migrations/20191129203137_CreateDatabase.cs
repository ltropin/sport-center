using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SportCenter.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ID_Role",
                table: "Client",
                nullable: false,
                defaultValueSql: "((1))");

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

            migrationBuilder.CreateIndex(
                name: "IX_Client_ID_Role",
                table: "Client",
                column: "ID_Role");

            migrationBuilder.CreateIndex(
                name: "IX_Abonement_ID_Client",
                table: "Abonement",
                column: "ID_Client");

            migrationBuilder.CreateIndex(
                name: "IX_RequestAbonement_ID_Client",
                table: "RequestAbonement",
                column: "ID_Client");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_Role_ID",
                table: "Client",
                column: "ID_Role",
                principalTable: "Role",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_Role_ID",
                table: "Client");

            migrationBuilder.DropTable(
                name: "Abonement");

            migrationBuilder.DropTable(
                name: "RequestAbonement");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropIndex(
                name: "IX_Client_ID_Role",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ID_Role",
                table: "Client");
        }
    }
}
