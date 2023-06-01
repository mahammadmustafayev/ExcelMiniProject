using SendGrid.Helpers.Mail;
using SendGrid;
using Newtonsoft.Json;

namespace ExcelMiniProject.Utilities.Mail;

public  class EmailConfig
{

    //private  readonly IConfiguration _configuration;
    

    //public  EmailConfig(IConfiguration configuration)
    //{
    //    _configuration = configuration;
    //    token = _configuration["apiKey"];
    //}
    //public  string OnGet()
    //{
    //    var token = _configuration["apiKey"];
    //    return token;
    //}
    //public static string token;
    public static string email = "mustafamehmed1251@gmail.com";
    public static string name = "Mahammad";
    //public static string token = new ConfigurationBuilder().AddUserSecrets<Program>().Build()["apiKey"];

    
}


