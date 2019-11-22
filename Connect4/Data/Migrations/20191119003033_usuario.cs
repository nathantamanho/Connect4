using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Connect4.Data.Migrations
{
    public partial class usuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Jogador",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jogador_UsuarioId",
                table: "Jogador",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jogador_AspNetUsers_UsuarioId",
                table: "Jogador",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jogador_AspNetUsers_UsuarioId",
                table: "Jogador");

            migrationBuilder.DropIndex(
                name: "IX_Jogador_UsuarioId",
                table: "Jogador");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Jogador");
        }
    }
}
