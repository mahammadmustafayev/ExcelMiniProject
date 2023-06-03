using System.Net.Mail;
using System.Net;
using SendGrid.Helpers.Mail;
using SendGrid;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;

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

    
    

}
