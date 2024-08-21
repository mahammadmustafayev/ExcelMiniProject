using Ganss.Excel;
using System.Text.Json.Serialization;

namespace ExcelMiniProject.Data.Models;
public enum Report
{
    
    Segment = 1,
    Country = 2,
    Products = 3,
    ProdutsDiscont = 4
}
public class ExcelProps
{
    public int Id { get; set; }
    [Column("Segment", MappingDirections.ExcelToObject)]
    public string Segment { get; set; }
    [Column("Country", MappingDirections.ExcelToObject)]
    public string Country { get; set; }
    [Column("Product", MappingDirections.ExcelToObject)]
    public string Product { get; set; }
    [Column("Discount Band", MappingDirections.ExcelToObject)]
    public string? DiscountBand { get; set; }
    [Column("Units Sold", MappingDirections.ExcelToObject)]
    public decimal? UnitsSold { get; set; }
    [Column("Manufacturing Price", MappingDirections.ExcelToObject)]

    public decimal? ManufacturingPrice { get; set; }
    [Column("Sale Price", MappingDirections.ExcelToObject)]
    public decimal? SalePrice { get; set; }
    [Column("Gross Sales", MappingDirections.ExcelToObject)]
    public decimal GrossSales { get; set; }
    [Column("Discounts", MappingDirections.ExcelToObject)]
    public decimal Discounts { get; set; }
    [Column("Sales", MappingDirections.ExcelToObject)]
    public decimal Sales { get; set; }
    [Column("COGS", MappingDirections.ExcelToObject)]
    public decimal? COGS { get; set; }
    [Column("Profit", MappingDirections.ExcelToObject)]
    public decimal? Profit { get; set; }
    [Column("Date", MappingDirections.ExcelToObject)]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime Date { get; set; }

}
