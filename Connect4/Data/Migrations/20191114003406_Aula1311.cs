using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Connect4.Data.Migrations
{
    public partial class Aula1311 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Jogador_jogadorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Jogador_Jogador1Id",
                table: "Jogo");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Jogador_Jogador2Id",
                table: "Jogo");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Jogador_JogadorPessoaId",
                table: "Jogo");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Tabuleiros_TabuleiroId",
                table: "Jogo");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Torneio_TorneioId",
                table: "Jogo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Jogo",
                table: "Jogo");

            migrationBuilder.RenameTable(
                name: "Jogo",
                newName: "Jogos");

            migrationBuilder.RenameIndex(
                name: "IX_Jogo_TorneioId",
                table: "Jogos",
                newName: "IX_Jogos_TorneioId");

            migrationBuilder.RenameIndex(
                name: "IX_Jogo_TabuleiroId",
                table: "Jogos",
                newName: "IX_Jogos_TabuleiroId");

            migrationBuilder.RenameIndex(
                name: "IX_Jogo_JogadorPessoaId",
                table: "Jogos",
                newName: "IX_Jogos_JogadorPessoaId");

            migrationBuilder.RenameIndex(
                name: "IX_Jogo_Jogador2Id",
                table: "Jogos",
                newName: "IX_Jogos_Jogador2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Jogo_Jogador1Id",
                table: "Jogos",
                newName: "IX_Jogos_Jogador1Id");

            migrationBuilder.RenameColumn(
                name: "jogadorId",
                table: "AspNetUsers",
                newName: "JogadorId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_jogadorId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_JogadorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jogos",
                table: "Jogos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Jogador_JogadorId",
                table: "AspNetUsers",
                column: "JogadorId",
                principalTable: "Jogador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jogos_Jogador_Jogador1Id",
                table: "Jogos",
                column: "Jogador1Id",
                principalTable: "Jogador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jogos_Jogador_Jogador2Id",
                table: "Jogos",
                column: "Jogador2Id",
                principalTable: "Jogador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jogos_Jogador_JogadorPessoaId",
                table: "Jogos",
                column: "JogadorPessoaId",
                principalTable: "Jogador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jogos_Tabuleiros_TabuleiroId",
                table: "Jogos",
                column: "TabuleiroId",
                principalTable: "Tabuleiros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jogos_Torneio_TorneioId",
                table: "Jogos",
                column: "TorneioId",
                principalTable: "Torneio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Jogador_JogadorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogos_Jogador_Jogador1Id",
                table: "Jogos");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogos_Jogador_Jogador2Id",
                table: "Jogos");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogos_Jogador_JogadorPessoaId",
                table: "Jogos");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogos_Tabuleiros_TabuleiroId",
                table: "Jogos");

            migrationBuilder.DropForeignKey(
                name: "FK_Jogos_Torneio_TorneioId",
                table: "Jogos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Jogos",
                table: "Jogos");

            migrationBuilder.RenameTable(
                name: "Jogos",
                newName: "Jogo");

            migrationBuilder.RenameIndex(
                name: "IX_Jogos_TorneioId",
                table: "Jogo",
                newName: "IX_Jogo_TorneioId");

            migrationBuilder.RenameIndex(
                name: "IX_Jogos_TabuleiroId",
                table: "Jogo",
                newName: "IX_Jogo_TabuleiroId");

            migrationBuilder.RenameIndex(
                name: "IX_Jogos_JogadorPessoaId",
                table: "Jogo",
                newName: "IX_Jogo_JogadorPessoaId");

            migrationBuilder.RenameIndex(
                name: "IX_Jogos_Jogador2Id",
                table: "Jogo",
                newName: "IX_Jogo_Jogador2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Jogos_Jogador1Id",
                table: "Jogo",
                newName: "IX_Jogo_Jogador1Id");

            migrationBuilder.RenameColumn(
                name: "JogadorId",
                table: "AspNetUsers",
                newName: "jogadorId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_JogadorId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_jogadorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jogo",
                table: "Jogo",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Jogador_jogadorId",
                table: "AspNetUsers",
                column: "jogadorId",
                principalTable: "Jogador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Jogo_Jogador_JogadorPessoaId",
                table: "Jogo",
                column: "JogadorPessoaId",
                principalTable: "Jogador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jogo_Tabuleiros_TabuleiroId",
                table: "Jogo",
                column: "TabuleiroId",
                principalTable: "Tabuleiros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jogo_Torneio_TorneioId",
                table: "Jogo",
                column: "TorneioId",
                principalTable: "Torneio",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
