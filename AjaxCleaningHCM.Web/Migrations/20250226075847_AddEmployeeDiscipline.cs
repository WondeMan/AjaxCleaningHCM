using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AjaxCleaningHCM.Web.Migrations
{
    public partial class AddEmployeeDiscipline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeDisciplines",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    TimeZoneInfo = table.Column<string>(nullable: true),
                    RegisteredDate = table.Column<DateTime>(nullable: false),
                    RegisteredBy = table.Column<string>(nullable: true),
                    LastUpdateDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    RecordStatus = table.Column<int>(nullable: false),
                    IsReadOnly = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<long>(nullable: false),
                    DisciplineCategoryId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ActionTaken = table.Column<string>(nullable: true),
                    ActionTakenDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDisciplines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDisciplines_DisciplineCategorys_DisciplineCategoryId",
                        column: x => x.DisciplineCategoryId,
                        principalTable: "DisciplineCategorys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeDisciplines_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDisciplines_DisciplineCategoryId",
                table: "EmployeeDisciplines",
                column: "DisciplineCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDisciplines_EmployeeId",
                table: "EmployeeDisciplines",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeDisciplines");
        }
    }
}
