﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace CSE_Hankers.Migrations
{
    public partial class addedNameField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "AspNetUsers");
        }
    }
}
