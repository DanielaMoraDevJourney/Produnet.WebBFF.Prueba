using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Produnet.WebBFF.Prueba.Migrations
{
    /// <inheritdoc />
    public partial class Inicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SesionesUsuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreEquipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraFin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionesUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PeriodosActividad",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SesionUsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraFin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodosActividad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeriodosActividad_SesionesUsuario_SesionUsuarioId",
                        column: x => x.SesionUsuarioId,
                        principalTable: "SesionesUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PeriodosInactividad",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SesionUsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HoraInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HoraFin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PeriodosInactividad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PeriodosInactividad_SesionesUsuario_SesionUsuarioId",
                        column: x => x.SesionUsuarioId,
                        principalTable: "SesionesUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosActividad_SesionUsuarioId",
                table: "PeriodosActividad",
                column: "SesionUsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PeriodosInactividad_SesionUsuarioId",
                table: "PeriodosInactividad",
                column: "SesionUsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PeriodosActividad");

            migrationBuilder.DropTable(
                name: "PeriodosInactividad");

            migrationBuilder.DropTable(
                name: "SesionesUsuario");
        }
    }
}
