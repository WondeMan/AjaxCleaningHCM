using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AjaxCleaningHCM.Web.Migrations
{
    public partial class AddPayroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payrolls",
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
                    BasicSalary = table.Column<decimal>(nullable: false),
                    Allowances = table.Column<decimal>(nullable: false),
                    Deductions = table.Column<decimal>(nullable: false),
                    NetSalary = table.Column<decimal>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaymentStatus = table.Column<int>(nullable: false),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    BankId = table.Column<long>(nullable: true),
                    PaymentReference = table.Column<string>(nullable: true),
                    PaymentMode = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payrolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payrolls_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payrolls_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_BankId",
                table: "Payrolls",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Payrolls_EmployeeId",
                table: "Payrolls",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payrolls");
        }
    }
}
