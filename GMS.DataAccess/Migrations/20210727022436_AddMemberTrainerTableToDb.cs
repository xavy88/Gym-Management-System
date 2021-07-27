using Microsoft.EntityFrameworkCore.Migrations;

namespace GMS.DataAccess.Migrations
{
    public partial class AddMemberTrainerTableToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberTrainers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Member_Id = table.Column<int>(type: "int", nullable: false),
                    Trainer_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberTrainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberTrainers_Members_Member_Id",
                        column: x => x.Member_Id,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberTrainers_Trainer_Trainer_Id",
                        column: x => x.Trainer_Id,
                        principalTable: "Trainer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberTrainers_Member_Id",
                table: "MemberTrainers",
                column: "Member_Id");

            migrationBuilder.CreateIndex(
                name: "IX_MemberTrainers_Trainer_Id",
                table: "MemberTrainers",
                column: "Trainer_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberTrainers");
        }
    }
}
