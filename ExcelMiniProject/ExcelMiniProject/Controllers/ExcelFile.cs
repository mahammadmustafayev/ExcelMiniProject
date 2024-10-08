﻿using ExcelMiniProject.Data.DAL;
using ExcelMiniProject.Data.Models;
using ExcelMiniProject.Utilities.Extension;
using Ganss.Excel;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System.Text;

namespace ExcelMiniProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExcelFile : ControllerBase
{
	private readonly ExcelDbContext _context;
	private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _configuration;

    public ExcelFile(ExcelDbContext context,IWebHostEnvironment env,IConfiguration configuration)
	{
		_context= context;
        _env= env;
        _configuration= configuration;
	}

    private IActionResult SendEmail( string[] mails, string message)
    {
        foreach (var mail in mails)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["mailName"]));
            email.To.Add(MailboxAddress.Parse(mail));
            email.Subject = "Excel Project";
            email.Body = new TextPart(TextFormat.Html) { Text = message };
            using (SmtpClient smtp = new())
            {
                smtp.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration["mailName"], _configuration["mailPassword"]);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
        return NoContent();
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
            string path = Path.Combine(_env.ContentRootPath,"Files");
            
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
    //2013-11-01 00:00:00.0000000
    //2014-12-01 00:00:00.0000000
    public async Task<IActionResult> SendReport(Report report,[FromQuery] string[] AcceptorEmail, [FromQuery] DateTime StartDate,[FromQuery] DateTime EndDate)
    {
        
        if ( StartDate.CheckDate(EndDate) && AcceptorEmail.CheckEmail("gmail.com"))
        {
            switch (report)
            {
                case Report.Segment:
                {
                        decimal ProductCount = 0;
                        decimal SalesSum = 0;
                        decimal DiscountSum = 0;
                        decimal ProfitSum = 0;
                        foreach (var segment in _context.ExcelProps.GroupBy(e=>e.Segment))
                        {
                        //var segmentQuery = segment.Where(d => d.Date > StartDate && d.Date < EndDate);
                        decimal product= segment.Where(d => d.Date > StartDate && d.Date < EndDate).Select(x=>x.Product).Count();
                        ProductCount+=product;
                        decimal sales=     segment.Where(d => d.Date > StartDate && d.Date < EndDate).Sum(x=>x.Sales);
                        SalesSum+=sales;
                        decimal discount=  segment.Where(d => d.Date > StartDate && d.Date < EndDate).Sum(x=>x.Discounts);
                        DiscountSum+=discount;
                        decimal profit= (decimal)segment.Where(d => d.Date > StartDate && d.Date < EndDate).Sum(x=>x.Profit);
                        ProfitSum+=profit;

                    }
                        StringBuilder message = new StringBuilder();
                        message.Append(
                            $"""
                               <div style="border: 2px solid rgb(171, 171, 171);text-align: center;border-radius: 25px;">
                                <h1>
                                   <b style="font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;" >{report}</b>
                                </h1>
                                <br>
                                <b style="font-size: 1.3rem;">  Product Count:</b>
                                <span>{ProductCount}</span>
                                <hr>
                                <b style="font-size: 1.3rem;">  Sales Sum:</b>
                                <span>{SalesSum} $</span>
                                <hr>
                                <b style="font-size: 1.3rem;">  Discount Sum:</b>
                                <span>{DiscountSum} $</span>
                                <hr>
                                <b style="font-size: 1.3rem;">  Profit Sum:</b>
                                <span>{ProfitSum} $</span>
                                <hr>
                                <span><b style="font-size: 1.1rem;">({EndDate})</b> To <b style="font-size: 1.1rem;">({StartDate})</b></span>

                            </div>
                            """);
                        SendEmail(AcceptorEmail,message.ToString());
                        //await AcceptorEmail.SendEmail(message.ToString(), senderName,mailKey);
                        break;
                }
                case Report.Country: 
                {
                        decimal ProductCount = 0;
                        decimal SalesSum = 0;
                        decimal DiscountSum = 0;
                        decimal ProfitSum = 0;
                        foreach (var segment in _context.ExcelProps.GroupBy(e => e.Country))
                        {
                            //var segmentQuery = segment.Where(d => d.Date > StartDate && d.Date < EndDate);
                            decimal product = segment.Where(d => d.Date > StartDate && d.Date < EndDate).Select(x => x.Product).Count();
                            ProductCount+= product;
                            decimal sales =    segment.Where(d => d.Date > StartDate && d.Date < EndDate).Sum(x => x.Sales);
                            SalesSum+= sales;
                            decimal discount = segment.Where(d => d.Date > StartDate && d.Date < EndDate).Sum(x => x.Discounts);
                            DiscountSum+= discount;
                            decimal profit =   (decimal)segment.Where(d => d.Date > StartDate && d.Date < EndDate).Sum(x => x.Profit);
                            ProfitSum+= profit;
                        }
                            StringBuilder message = new StringBuilder();
                            message.Append(
                                $"""
                                <div style="border: 2px solid rgb(171, 171, 171);text-align: center;border-radius: 25px;">
                                <h1>
                                   <b style="font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;" >{report}</b>
                                </h1>
                                <br>
                                <b style="font-size: 1.3rem;">  Product Count:</b>
                                <span>{ProductCount}</span>
                                <hr>
                                <b style="font-size: 1.3rem;">  Sales Sum:</b>
                                <span>{SalesSum} $</span>
                                <hr>
                                <b style="font-size: 1.3rem;">  Discount Sum:</b>
                                <span>{DiscountSum} $</span>
                                <hr>
                                <b style="font-size: 1.3rem;">  Profit Sum:</b>
                                <span>{ProfitSum} $</span>
                                <hr>
                                <span><b style="font-size: 1.1rem;">({EndDate})</b> To <b style="font-size: 1.1rem;">({StartDate})</b></span>
                            
                            </div>
                            """);
                        SendEmail(AcceptorEmail, message.ToString());
                        //await AcceptorEmail.SendEmail(message.ToString(), senderName, mailKey);
                        break;
                }
                case Report.Products:
                {
                        decimal ProductCount = 0 ;
                        decimal SalesSum = 0;
                        decimal DiscountSum = 0;
                        decimal ProfitSum = 0;
                     foreach (var segment in _context.ExcelProps.GroupBy(e => e.Product))
                        {
                            //var segmentQuery = segment.Where(d => d.Date > StartDate && d.Date < EndDate);
                            decimal product = segment.Where(d => d.Date > StartDate && d.Date < EndDate).Select(x => x.Product).Count();
                            ProductCount+= product;
                            decimal sales = segment.Where(d => d.Date > StartDate && d.Date < EndDate).Sum(x => x.Sales);
                            SalesSum+= sales;
                            decimal discount = segment.Where(d => d.Date > StartDate && d.Date < EndDate).Sum(x => x.Discounts);
                            DiscountSum+= discount;
                            decimal profit = (decimal)segment.Where(d => d.Date > StartDate && d.Date < EndDate).Sum(x => x.Profit);
                            ProfitSum+= profit;
                        }
                            StringBuilder message = new StringBuilder();
                            message.Append(
                                $"""
                                <div style="border: 2px solid rgb(171, 171, 171);text-align: center;border-radius: 25px;">
                                <h1>
                                   <b style="font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;" >{report}</b>
                                </h1>
                                <br>
                                <b style="font-size: 1.3rem;">  Product Count:</b>
                                <span>{ProductCount}</span>
                                <hr>
                                <b style="font-size: 1.3rem;">  Sales Sum:</b>
                                <span>{SalesSum} $</span>
                                <hr>
                                <b style="font-size: 1.3rem;">  Discount Sum:</b>
                                <span>{DiscountSum} $</span>
                                <hr>
                                <b style="font-size: 1.3rem;">  Profit Sum:</b>
                                <span>{ProfitSum} $</span>
                                <hr>
                                <span><b style="font-size: 1.1rem;">({EndDate})</b> To <b style="font-size: 1.1rem;">({StartDate})</b></span>
                            
                            </div>
                            """);
                        SendEmail(AcceptorEmail,message.ToString());
                        //await AcceptorEmail.SendEmail(message.ToString(), senderName, mailKey);
                        break;
                }
                case Report.ProdutsDiscont:
                { 
                        decimal Discountsum=0;
                        
                        foreach (var segment in _context.ExcelProps.GroupBy(e => e.Product))
                        {
                            //var segmentQuery = segment.Where(d => d.Date > StartDate && d.Date < EndDate);
                            decimal checkDiscount = (decimal)segment.Where(d => d.Date > StartDate && d.Date < EndDate).Sum(x => (x.Discounts/x.Sales)*100);
                            Discountsum += checkDiscount;
                            
                        }
                        StringBuilder message = new StringBuilder();
                        message.Append(
                            $"""
                                    <div style="border: 2px solid rgb(171, 171, 171);text-align: center;border-radius: 25px;">
                                    <h1>
                                       <b style="font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;" >{report}</b>
                                    </h1>
                                    <br>
                                    <hr>
                                    <b style="font-size: 1.3rem;">  Products Discount:</b>
                                    <span>{Discountsum} $</span>
                                    <br>
                                    <hr>
                                    <span><b style="font-size: 1.1rem;">({EndDate})</b> To <b style="font-size: 1.1rem;">({StartDate})</b></span>
                                
                                </div>
                                """);
                        SendEmail(AcceptorEmail, message.ToString());
                        //await AcceptorEmail.SendEmail(message.ToString(), senderName, mailKey);
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
