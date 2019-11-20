using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Connect4.Data.Migrations
{
    public partial class Jogador : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jogador",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Discriminator = table.Column<string>(nullable: false),
                    TorneioId = table.Column<int>(nullable: true),
                    NomeComputador = table.Column<string>(nullable: true),
                    URLServico = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jogador_Torneio_TorneioId",
                        column: x => x.TorneioId,
                        principalTable: "Torneio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Jogo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    JogadorPessoaId = table.Column<int>(nullable: true),
                    TorneioId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jogo_Jogador_JogadorPessoaId",
                        column: x => x.JogadorPessoaId,
                        principalTable: "Jogador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Jogo_Torneio_TorneioId",
                        column: x => x.TorneioId,
                        principalTable: "Torneio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jogador_TorneioId",
                table: "Jogador",
                column: "TorneioId");

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_JogadorPessoaId",
                table: "Jogo",
                column: "JogadorPessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_TorneioId",
                table: "Jogo",
                column: "TorneioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jogo");

            migrationBuilder.DropTable(
                name: "Jogador");
        }
    }
}
