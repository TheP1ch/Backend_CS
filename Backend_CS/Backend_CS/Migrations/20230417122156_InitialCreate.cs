using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_CS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workers",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    imgUrl = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WorkGroups",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkGroups", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "requestDatas",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    price = table.Column<int>(type: "INTEGER", nullable: false),
                    lastUpdateDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    createDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    userId = table.Column<int>(type: "INTEGER", nullable: true),
                    priorityId = table.Column<int>(type: "INTEGER", nullable: false),
                    statusNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    statusPosition = table.Column<int>(type: "INTEGER", nullable: false),
                    comment = table.Column<string>(type: "TEXT", nullable: true),
                    WorkGroupid = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requestDatas", x => x.id);
                    table.ForeignKey(
                        name: "FK_requestDatas_WorkGroups_WorkGroupid",
                        column: x => x.WorkGroupid,
                        principalTable: "WorkGroups",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    requestDataId = table.Column<int>(type: "INTEGER", nullable: false),
                    workGroupId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_Requests_WorkGroups_workGroupId",
                        column: x => x.workGroupId,
                        principalTable: "WorkGroups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Requests_requestDatas_requestDataId",
                        column: x => x.requestDataId,
                        principalTable: "requestDatas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_requestDatas_WorkGroupid",
                table: "requestDatas",
                column: "WorkGroupid");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_requestDataId",
                table: "Requests",
                column: "requestDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_workGroupId",
                table: "Requests",
                column: "workGroupId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Workers");

            migrationBuilder.DropTable(
                name: "requestDatas");

            migrationBuilder.DropTable(
                name: "WorkGroups");
        }
    }
}
