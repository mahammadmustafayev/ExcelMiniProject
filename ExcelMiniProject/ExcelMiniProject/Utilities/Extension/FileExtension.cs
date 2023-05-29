using ExcelMiniProject.Utilities.Mail;
using System.Net.Mail;
using System.Net;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace ExcelMiniProject.Utilities.Extension;

public static class FileExtension
{
    public static bool Checksize(this IFormFile file, int mb)
    {
        if (file.Length < mb) return true;
        return false;
    }
    public static bool CheckType(this IFormFile file, string type)
    {
        if (Path.GetExtension(file.FileName) == type) return true;
        return false;
    }
    public static void DeleteFiles(this string path)
    {
        string[] removeFiles = Directory.GetFiles(path);
        foreach (var file in removeFiles)
        {
            File.Delete(file);
        }
    }
    public static bool CheckEmail(this string[] emails,string domain)
    {
        foreach (var email in emails)
        {
            if ( email.Contains("@") && email.Contains(domain))
            {
                return true;
            }
        }
        return false;
    }
    public static bool CheckDate(this DateTime start,DateTime end)
    {
        if ((end.Year>start.Year) || (end.Month>start.Month) || (end.Day>start.Day))
        {
            return true;
        }
        return false;
    }
    public static  async Task  SendEmail(this string[] emails,string message)
    {
        foreach (var email in emails)
        {
            var apiKey = EmailConfig.token;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(EmailConfig.email, EmailConfig.name);
            var subject = "Excel Mail Project";
            var to = new EmailAddress(email);
            var plainTextContent = " ";
            var htmlContent = message;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
           
        }
    }
}
