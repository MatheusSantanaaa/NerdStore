using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NerdStore.Vendas.Data.Migrations
{
    public partial class TrocaTipoDeDados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "Codigo",
              table: "Vouchers",
              nullable: false
              );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
