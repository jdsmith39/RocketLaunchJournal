using Microsoft.EntityFrameworkCore.Migrations;

namespace RocketLaunchJournal.Entities.Migrations
{
    public partial class RemovedDelayNote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Launches_DelayNotes_DelayNoteId",
                table: "Launches");

            migrationBuilder.DropTable(
                name: "DelayNotes");

            migrationBuilder.DropIndex(
                name: "IX_Launches_DelayNoteId",
                table: "Launches");

            migrationBuilder.RenameColumn(
                name: "ParachuteSize",
                table: "Rockets", "Recovery");

            migrationBuilder.DropColumn(
                name: "DelayNoteId",
                table: "Launches");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UserTokens",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Users",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "UserRefreshTokens",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "UserLogins",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Recovery",
                table: "Rockets");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "UserTokens",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<string>(
                name: "ConcurrencyStamp",
                table: "Users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "RefreshToken",
                table: "UserRefreshTokens",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderDisplayName",
                table: "UserLogins",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<string>(
                name: "ParachuteSize",
                table: "Rockets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "DelayNoteId",
                table: "Launches",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DelayNotes",
                columns: table => new
                {
                    DelayNoteId = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelayNotes", x => x.DelayNoteId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Launches_DelayNoteId",
                table: "Launches",
                column: "DelayNoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Launches_DelayNotes_DelayNoteId",
                table: "Launches",
                column: "DelayNoteId",
                principalTable: "DelayNotes",
                principalColumn: "DelayNoteId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
