using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SvcService.Migrations
{
    /// <inheritdoc />
    public partial class ReInit : Migration
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
                    Id = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Repo = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageUrl = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HasVersion = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VersionMajor = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VersionMinor = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VersionPatch = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HasIdleResource = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdleCpu = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    IdleRam = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    IdleDisk = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    IdleGpuCore = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    IdleGpuMem = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
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
                    ServiceId = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdSuffix = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Path = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InputSize = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    OutputSize = table.Column<decimal>(type: "decimal(16,4)", nullable: false),
                    Info = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Method = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Dependencies",
                columns: table => new
                {
                    CallerServiceId = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CallerIdSuffix = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CalleeServiceId = table.Column<string>(type: "varchar(64)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CalleeIdSuffix = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SerilizedData = table.Column<string>(type: "varchar(4096)", maxLength: 4096, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependencies", x => new { x.CallerServiceId, x.CallerIdSuffix, x.CalleeServiceId, x.CalleeIdSuffix });
                    table.ForeignKey(
                        name: "FK_Dependencies_Interfaces_CalleeServiceId_CalleeIdSuffix",
                        columns: x => new { x.CalleeServiceId, x.CalleeIdSuffix },
                        principalTable: "Interfaces",
                        principalColumns: new[] { "ServiceId", "IdSuffix" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dependencies_Interfaces_CallerServiceId_CallerIdSuffix",
                        columns: x => new { x.CallerServiceId, x.CallerIdSuffix },
                        principalTable: "Interfaces",
                        principalColumns: new[] { "ServiceId", "IdSuffix" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dependencies_Services_CalleeServiceId",
                        column: x => x.CalleeServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dependencies_Services_CallerServiceId",
                        column: x => x.CallerServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Dependencies_CalleeServiceId_CalleeIdSuffix",
                table: "Dependencies",
                columns: new[] { "CalleeServiceId", "CalleeIdSuffix" });

            migrationBuilder.CreateIndex(
                name: "IX_Services_Name",
                table: "Services",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dependencies");

            migrationBuilder.DropTable(
                name: "Interfaces");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
