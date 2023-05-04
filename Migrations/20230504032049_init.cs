using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SvcService.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Repo = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VersionMajor = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VersionMinor = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VersionPatch = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdleCpu = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    IdleRam = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    IdleDisk = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    IdleGpuCore = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    IdleGpuMem = table.Column<decimal>(type: "decimal(16,4)", nullable: true),
                    DesiredCpu = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    DesiredRam = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    DesiredDisk = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    DesiredGpuCore = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    DesiredGpuMem = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    DesiredCapability = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Interfaces",
                columns: table => new
                {
                    ServiceId = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdSuffix = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Path = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InputSize = table.Column<int>(type: "int", nullable: false),
                    OutputSize = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interfaces", x => new { x.ServiceId, x.IdSuffix });
                    table.ForeignKey(
                        name: "FK_Interfaces_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Interfaces");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
