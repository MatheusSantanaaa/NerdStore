using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NerdStore.Vendas.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "Codigo",
            //    table: "Pedidos",
            //    type: "int",
            //    nullable: false,
            //    defaultValueSql: "NEXT VALUE FOR MinhaSequencia",
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true,
            //    oldDefaultValueSql: "NEXT VALUE FOR MinhaSequencia");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClienteId",
                table: "Pedidos",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<int>(
            //    name: "Codigo",
            //    table: "Pedidos",
            //    type: "int",
            //    nullable: true,
            //    defaultValueSql: "NEXT VALUE FOR MinhaSequencia",
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldDefaultValueSql: "NEXT VALUE FOR MinhaSequencia");

            migrationBuilder.AlterColumn<Guid>(
                name: "ClienteId",
                table: "Pedidos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}
