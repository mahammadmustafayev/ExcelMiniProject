using ExcelMiniProject.Data.DAL;
using ExcelMiniProject.Data.Models;
using ExcelMiniProject.Utilities.Extension;
using Ganss.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExcelMiniProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExcelFile : ControllerBase
{
	private readonly ExcelDbContext _context;
	public ExcelFile(ExcelDbContext context)
	{
		_context= context;
	}
	[HttpPost]
	public IActionResult UploadData(IFormFile formFile)
	{
		if (formFile != null)
		{
            if (formFile.Checksize(5))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { statuscode = 404, message = "This file is must be than max 5 mb" });
            }
            if (!formFile.CheckType(".xlsx") && !formFile.CheckType(".xls"))
            {
                return StatusCode(StatusCodes.Status404NotFound, new { statuscode = 404, message = "This file type must be (xlsx or xls)" });
            }
           
            string path = @"C:\Users\acer\Desktop\ExcelMiniProject\ExcelMiniProject\ExcelMiniProject\Files\";
            
            string filePath = Path.Combine(path, formFile.FileName);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }
            var fileDatas = new ExcelMapper(filePath).Fetch<ExcelProps>();
            foreach (ExcelProps item in fileDatas)
            {
                _context.ExcelProps.Add(item);
            }
            _context.SaveChanges();
        }
		return Ok("Correct");
	}

    [HttpGet]
    public IActionResult SendReport(DateTime StartDate,DateTime EndDate, string[] AcceptorEmail)
    {
        if (EndDate>StartDate && AcceptorEmail.CheckEmail("code.edu.az"))
        {
            return StatusCode(StatusCodes.Status200OK, new { statuscode = 200, message = "yaxsi isleyir" });

        }
        return Ok("Correct");
    }
}
