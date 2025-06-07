using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addusers2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_on_case_CaseId",
                table: "users_on_case",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_on_case_case_info_CaseId",
                table: "users_on_case",
                column: "CaseId",
                principalTable: "case_info",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_on_case_case_info_CaseId",
                table: "users_on_case");

            migrationBuilder.DropIndex(
                name: "IX_users_on_case_CaseId",
                table: "users_on_case");
        }
    }
}
