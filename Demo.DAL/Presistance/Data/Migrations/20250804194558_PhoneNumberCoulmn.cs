﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demo.DAL.Presistance.Data.Migrations
{
    /// <inheritdoc />
    public partial class PhoneNumberCoulmn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Employees");
        }
    }
}
