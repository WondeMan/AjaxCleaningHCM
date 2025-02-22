using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AjaxCleaningHCM.Web.Migrations
{
    public partial class AddEmployeeTermination : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeTerminations",
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
                    EmployeeId = table.Column<long>(nullable: false),
                    TerminationDate = table.Column<DateTime>(nullable: false),
                    TerminationReason = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Letter = table.Column<string>(nullable: true),
                    LetterType = table.Column<int>(nullable: false),
                    EmployeeStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeTerminations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeTerminations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeTerminations_EmployeeId",
                table: "EmployeeTerminations",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeTerminations");
        }
    }
}
