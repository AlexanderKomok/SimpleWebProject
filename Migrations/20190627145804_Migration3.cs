using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAppTry3.Migrations
{
    public partial class Migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectModel_Albums_AlbumID",
                table: "ConnectModel");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectModel_Albums_AlbumID",
                table: "ConnectModel",
                column: "AlbumID",
                principalTable: "Albums",
                principalColumn: "AlbumID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConnectModel_Albums_AlbumID",
                table: "ConnectModel");

            migrationBuilder.AddForeignKey(
                name: "FK_ConnectModel_Albums_AlbumID",
                table: "ConnectModel",
                column: "AlbumID",
                principalTable: "Albums",
                principalColumn: "AlbumID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
