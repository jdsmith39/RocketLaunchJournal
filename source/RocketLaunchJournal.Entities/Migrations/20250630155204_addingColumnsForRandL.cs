using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketLaunchJournal.Entities.Migrations
{
    /// <inheritdoc />
    public partial class addingColumnsForRandL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CenterOfGravity",
                table: "Rockets",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CenterOfPreassure",
                table: "Rockets",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TubeLengthForApogeeCharge",
                table: "Rockets",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TubeLengthForMainCharge",
                table: "Rockets",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecoveryNotes",
                table: "Launches",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CenterOfGravity",
                table: "Rockets");

            migrationBuilder.DropColumn(
                name: "CenterOfPreassure",
                table: "Rockets");

            migrationBuilder.DropColumn(
                name: "TubeLengthForApogeeCharge",
                table: "Rockets");

            migrationBuilder.DropColumn(
                name: "TubeLengthForMainCharge",
                table: "Rockets");

            migrationBuilder.DropColumn(
                name: "RecoveryNotes",
                table: "Launches");
        }
    }
}
