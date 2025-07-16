using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sheenam.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalInfo",
                table: "Home",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Home",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Area",
                table: "Home",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "HostId",
                table: "Home",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsPetAllowed",
                table: "Home",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShared",
                table: "Home",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVacant",
                table: "Home",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBathrooms",
                table: "Home",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBedrooms",
                table: "Home",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Home",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Home",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Home_HostId",
                table: "Home",
                column: "HostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Home_Hosts_HostId",
                table: "Home",
                column: "HostId",
                principalTable: "Hosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Home_Hosts_HostId",
                table: "Home");

            migrationBuilder.DropIndex(
                name: "IX_Home_HostId",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "AdditionalInfo",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "HostId",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "IsPetAllowed",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "IsShared",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "IsVacant",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "NumberOfBathrooms",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "NumberOfBedrooms",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Home");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Home");
        }
    }
}
