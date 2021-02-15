﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CSE_Hankers.Migrations
{
    public partial class userIdmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "AspNetUsers");
        }
    }
}
