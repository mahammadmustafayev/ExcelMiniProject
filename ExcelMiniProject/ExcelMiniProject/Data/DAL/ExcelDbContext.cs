using ExcelMiniProject.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcelMiniProject.Data.DAL;

public class ExcelDbContext:DbContext
{
    public ExcelDbContext(DbContextOptions options) : base(options) { }
    public DbSet<ExcelProps> ExcelProps { get; set; }
}
