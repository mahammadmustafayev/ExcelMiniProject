using ExcelMiniProject.Data.DAL;
using ExcelMiniProject.Data.Models;
using ExcelMiniProject.Utilities.Extension;
using ExcelMiniProject.Utilities.Mail;
using Ganss.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using SendGrid.Helpers.Mail;
using SendGrid;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using NPOI.SS.Formula.Functions;
using System.Runtime.CompilerServices;
using System.Text;

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
            path.DeleteFiles();
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
    //2014-12-01 00:00:00.0000000
    //2013-11-01 00:00:00.0000000
    public async Task<IActionResult> SendReport(Report report,[FromQuery] string[] AcceptorEmail, [FromQuery] DateTime EndDate,[FromQuery] DateTime StartDate)
    {
        
        if ( StartDate.CheckDate(EndDate) && AcceptorEmail.CheckEmail("code.edu.az"))
        {
            switch (report)
            {
                case Report.Segment:
                {
                    foreach (var segment in _context.ExcelProps.GroupBy(e=>e.Segment))
                    {
                        var segmentQuery = segment.Where(d => d.Date > StartDate && d.Date < EndDate);
                        var productCount=segmentQuery.Select(x=>x.Product).Count();
                        var salesSum=segmentQuery.Sum(x=>x.Sales);
                        var discountSum=segmentQuery.Sum(x=>x.Discounts);
                        var profitSum=segmentQuery.Sum(x=>x.Profit);
                        StringBuilder message=new StringBuilder();
                        message.Append(
                            $"""
                                <h1>
                                <b>{report}</b>
                                </h1>
                                <br>
                                <b>  Product Count:</b>
                                <span>{productCount}</span>
                                <hr>
                                <b>  Sales Sum:</b>
                                <span> $ {salesSum}</span>
                                <hr>
                                <b>  Discount Sum:</b>
                                <span>$ {discountSum}</span>
                                <hr>
                                <b>  Profit Sum:</b>
                                <span>$ {profitSum}</span>
                                <hr>
                                <span>{EndDate} to {StartDate}</span>
                            """);
                        await AcceptorEmail.SendEmail(message.ToString());

                    }
                    break;
                }
                case Report.Country: 
                {
                     foreach (var segment in _context.ExcelProps.GroupBy(e => e.Country))
                        {
                            var segmentQuery = segment.Where(d => d.Date > StartDate && d.Date < EndDate);
                            var productCount = segmentQuery.Select(x => x.Product).Count();
                            var salesSum = segmentQuery.Sum(x => x.Sales);
                            var discountSum = segmentQuery.Sum(x => x.Discounts);
                            var profitSum = segmentQuery.Sum(x => x.Profit);
                            StringBuilder message = new StringBuilder();
                            message.Append(
                                $"""
                                <h1>
                                <b>{report}</b>
                                </h1>
                                <br>
                                <b>  Product Count:</b>
                                <span>{productCount}</span>
                                <hr>
                                <b>  Sales Sum:</b>
                                <span> $ {salesSum}</span>
                                <hr>
                                <b>  Discount Sum:</b>
                                <span>$ {discountSum}</span>
                                <hr>
                                <b>  Profit Sum:</b>
                                <span>$ {profitSum}</span>
                                <hr>
                                <span>{EndDate} to {StartDate}</span>
                            """);
                            await AcceptorEmail.SendEmail(message.ToString());

                        }
                     break;
                }
                case Report.Products:
                {
                     foreach (var segment in _context.ExcelProps.GroupBy(e => e.Product))
                        {
                            var segmentQuery = segment.Where(d => d.Date > StartDate && d.Date < EndDate);
                            var productCount = segmentQuery.Select(x => x.Product).Count();
                            var salesSum = segmentQuery.Sum(x => x.Sales);
                            var discountSum = segmentQuery.Sum(x => x.Discounts);
                            var profitSum = segmentQuery.Sum(x => x.Profit);
                            StringBuilder message = new StringBuilder();
                            message.Append(
                                $"""
                                <h1>
                                <b>{report}</b>
                                </h1>
                                <br>
                                <b>  Product Count:</b>
                                <span>{productCount}</span>
                                <hr>
                                <b>  Sales Sum:</b>
                                <span> $ {salesSum}</span>
                                <hr>
                                <b>  Discount Sum:</b>
                                <span>$ {discountSum}</span>
                                <hr>
                                <b>  Profit Sum:</b>
                                <span>$ {profitSum}</span>
                                <hr>
                                <span>{EndDate} to {StartDate}</span>
                            """);
                            await AcceptorEmail.SendEmail(message.ToString());

                        }
                     break;
                }
                case Report.ProdutsDiscont:
                    {
                        
                        foreach (var segment in _context.ExcelProps.GroupBy(e => e.Product))
                        {
                            var segmentQuery = segment.Where(d => d.Date > StartDate && d.Date < EndDate);
                            var checkDiscount = segmentQuery.Sum(x => (x.Discounts/x.Sales)*100);
                            
                            StringBuilder message = new StringBuilder();
                            message.Append(
                                $"""
                                    <h1>
                                    <b>{report}</b>
                                    </h1>
                                    <br>
                                    <hr>
                                    <b>  Products Discount:</b>
                                    <span>{Math.Round(checkDiscount)} %</span>
                                    <br>
                                    <span>{EndDate} to {StartDate}</span>
                                """);
                            await AcceptorEmail.SendEmail(message.ToString());
                        }
                            break;
                    }
            }
            
        }
        else
        {
            return StatusCode(StatusCodes.Status404NotFound, new { statuscode = 404, message = "This mails is not contains code.edu.az domain or date" });
        }
        return Ok("Correct");
    }
}
