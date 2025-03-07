using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AjaxCleaningHCM.Web.Migrations
{
    public partial class addIssueRegistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IssueRegistrations",
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
                    Subject = table.Column<string>(nullable: false),
                    IssueDetail = table.Column<string>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    AttachmentPath = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    BranchId = table.Column<long>(nullable: false),
                    ActionTakenBy = table.Column<string>(nullable: true),
                    ActionTakenDate = table.Column<DateTime>(nullable: false),
                    ActionTakenRemark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueRegistrations_Branchs_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IssueRegistrations_BranchId",
                table: "IssueRegistrations",
                column: "BranchId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssueRegistrations");
        }
    }
}
