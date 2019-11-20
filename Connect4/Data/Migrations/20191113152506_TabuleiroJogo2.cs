using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Connect4.Data.Migrations
{
    public partial class TabuleiroJogo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Tabuleiro_TabuleiroId",
                table: "Jogo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tabuleiro",
                table: "Tabuleiro");

            migrationBuilder.RenameTable(
                name: "Tabuleiro",
                newName: "Tabuleiros");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tabuleiros",
                table: "Tabuleiros",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jogo_Tabuleiros_TabuleiroId",
                table: "Jogo",
                column: "TabuleiroId",
                principalTable: "Tabuleiros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jogo_Tabuleiros_TabuleiroId",
                table: "Jogo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tabuleiros",
                table: "Tabuleiros");

            migrationBuilder.RenameTable(
                name: "Tabuleiros",
                newName: "Tabuleiro");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tabuleiro",
                table: "Tabuleiro",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jogo_Tabuleiro_TabuleiroId",
                table: "Jogo",
                column: "TabuleiroId",
                principalTable: "Tabuleiro",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
