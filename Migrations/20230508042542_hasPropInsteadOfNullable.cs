using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SvcService.Migrations
{
    /// <inheritdoc />
    public partial class hasPropInsteadOfNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "VersionPatch",
                keyValue: null,
                column: "VersionPatch",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "VersionPatch",
                table: "Services",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "VersionMinor",
                keyValue: null,
                column: "VersionMinor",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "VersionMinor",
                table: "Services",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "VersionMajor",
                keyValue: null,
                column: "VersionMajor",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "VersionMajor",
                table: "Services",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "IdleRam",
                table: "Services",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IdleGpuMem",
                table: "Services",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IdleGpuCore",
                table: "Services",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IdleDisk",
                table: "Services",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "IdleCpu",
                table: "Services",
                type: "decimal(16,4)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DesiredCpu",
                table: "Services",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AddColumn<bool>(
                name: "HasIdleResource",
                table: "Services",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasVersion",
                table: "Services",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasIdleResource",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "HasVersion",
                table: "Services");

            migrationBuilder.AlterColumn<string>(
                name: "VersionPatch",
                table: "Services",
                type: "varchar(32)",
                maxLength: 32,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "VersionMinor",
                table: "Services",
                type: "varchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "VersionMajor",
                table: "Services",
                type: "varchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<decimal>(
                name: "IdleRam",
                table: "Services",
                type: "decimal(16,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "IdleGpuMem",
                table: "Services",
                type: "decimal(16,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "IdleGpuCore",
                table: "Services",
                type: "decimal(16,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "IdleDisk",
                table: "Services",
                type: "decimal(16,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "IdleCpu",
                table: "Services",
                type: "decimal(16,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(16,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "DesiredCpu",
                table: "Services",
                type: "decimal(16,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }
    }
}
