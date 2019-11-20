using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Connect4.Data.Migrations
{
    public partial class TabuleiroJogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Jogador1Id",
                table: "Jogo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Jogador2Id",
                table: "Jogo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TabuleiroId",
                table: "Jogo",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tabuleiro",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    InternalData = table.Column<string>(nullable: true),
                    Turno = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tabuleiro", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_Jogador1Id",
                table: "Jogo",
                column: "Jogador1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_Jogador2Id",
                table: "Jogo",
                column: "Jogador2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Jogo_TabuleiroId",
                table: "Jogo",
                column: "TabuleiroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jogo_Jogador_Jogador1Id",
                table: "Jogo",
                column: "Jogador1Id",
                principalTable: "Jogador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jogo_Jogador_Jogador2Id",
                table: "Jogo",
                column: "Jogador2Id",
                principalTable: "Jogador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jogo_Tabuleiro_TabuleiroId",
                table: "Jogo",
                column: "TabuleiroId",
                principalTable: "Tabuleiro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Jogador_Jogador1Id",
                table: "Jogo");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Jogador_Jogador2Id",
                table: "Jogo");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Tabuleiro_TabuleiroId",
                table: "Jogo");

            migrationBuilder.DropTable(
                name: "Tabuleiro");

            migrationBuilder.DropIndex(
                name: "IX_Jogo_Jogador1Id",
                table: "Jogo");

            migrationBuilder.DropIndex(
                name: "IX_Jogo_Jogador2Id",
                table: "Jogo");

            migrationBuilder.DropIndex(
                name: "IX_Jogo_TabuleiroId",
                table: "Jogo");

            migrationBuilder.DropColumn(
                name: "Jogador1Id",
                table: "Jogo");

            migrationBuilder.DropColumn(
                name: "Jogador2Id",
                table: "Jogo");

            migrationBuilder.DropColumn(
                name: "TabuleiroId",
                table: "Jogo");
        }
    }
}
