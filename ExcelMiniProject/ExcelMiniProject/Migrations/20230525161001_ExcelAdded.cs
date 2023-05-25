using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExcelMiniProject.Migrations
{
    /// <inheritdoc />
    public partial class ExcelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExcelProps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Segment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountBand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitsSold = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ManufacturingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrossSales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discounts = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Sales = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    COGS = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Profit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelProps", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcelProps");
        }
    }
}
