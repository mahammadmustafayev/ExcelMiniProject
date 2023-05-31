using SendGrid.Helpers.Mail;
using SendGrid;
using Newtonsoft.Json;

namespace ExcelMiniProject.Utilities.Mail;

public  class EmailConfig
{

    private static readonly IConfiguration _configuration;
    
    //public EmailConfig(IConfiguration configuration)
    //{
    //    //_configuration = configuration;
    //    token = _configuration.GetValue<string>("apiKey");
    //}

    public static string token;
    public static string email = "mustafamehmed1251@gmail.com";
    public static string name = "Mahammad";
    //public static string token = new ConfigurationBuilder().AddUserSecrets<Program>().Build()["apiKey"];

    
}


