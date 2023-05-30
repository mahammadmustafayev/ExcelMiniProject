using SendGrid.Helpers.Mail;
using SendGrid;
using Newtonsoft.Json;

namespace ExcelMiniProject.Utilities.Mail;

public class EmailConfig
{
    private readonly IConfiguration _configuration;
    public static string token;
    public EmailConfig(IConfiguration configuration)
    {
        _configuration = configuration;
        token = _configuration["apiKey"];
    }
    public static string email = "mustafamehmed1251@gmail.com";
    public static string name = "Mahammad";
    //public static string token = new ConfigurationBuilder().AddUserSecrets<Program>().Build()["apiKey"];

    
}


