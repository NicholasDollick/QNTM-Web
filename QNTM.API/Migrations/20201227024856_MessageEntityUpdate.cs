using Microsoft.EntityFrameworkCore.Migrations;

namespace QNTM.API.Migrations
{
    public partial class MessageEntityUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipientUsername",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderUsername",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientUsername",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderUsername",
                table: "Messages");
        }
    }
}
