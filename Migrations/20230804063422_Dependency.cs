using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SvcService.Migrations
{
    /// <inheritdoc />
    public partial class Dependency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dependencies",
                columns: table => new
                {
                    CallerServiceId = table.Column<string>(type: "varchar(32)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CallerIdSuffix = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CalleeServiceId = table.Column<string>(type: "varchar(32)", nullable: false)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dependencies");
        }
    }
}
