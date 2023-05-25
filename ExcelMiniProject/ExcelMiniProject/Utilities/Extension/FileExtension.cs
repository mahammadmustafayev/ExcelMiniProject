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
    public static bool CheckEmail(this string[] emails,string domain)
    {
        foreach (var email in emails)
        {
            email.Contains(domain);
            return true;
        }
        return false;
    }
}
