using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]

    public class SaveContactController : Controller
    {
        private readonly ILogger<SaveContactController> _logger;
        private readonly IConfiguration _configuration;

        public SaveContactController(ILogger<SaveContactController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult SubmitForm(string name, string email, string message)
        {
            try
            {
                // Validate the form data (optional)

                // Append the form data to a text file
                var filePath = "textfile.txt"; 
                using (var writer = new StreamWriter(filePath, append: true))
                {

                    SmtpClient client = new SmtpClient();
                    client.Host = _configuration["MyAppInfo:useHost"];
                    client.Port = Convert.ToInt32(_configuration["MyAppInfo:usePort"]);
                    client.UseDefaultCredentials = false;
                    //client.Credentials = new NetworkCredential("chris@southmountainwebsites.com", "gtYDPwRVQ8x$");
                    client.Credentials = new NetworkCredential(_configuration["MyAppInfo:useEmail"], _configuration["MyAppInfo:usePass"]);
                    client.EnableSsl = true;

                    MailMessage myMessage = new MailMessage();
                    myMessage.From = new MailAddress(_configuration["MyAppInfo:useEmail"]);
                    myMessage.To.Add(_configuration["MyAppInfo:useEmail"]);
                    myMessage.Subject = "Incoming Contact From SouthMountainWebsites.com";
                    myMessage.Body = "This is a test email sent using SMTP in ASP.NET C#.";



                    // Define the Los Angeles time zone
                    TimeZoneInfo azTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Phoenix");

                    // Get the current date and time in Los Angeles time zone
                    DateTimeOffset azTime = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, azTimeZone);


                    writer.WriteLine("--------------------------------");
                    writer.WriteLine($"Date: {DateTime.Now}/az: " + azTime.ToString());
                    writer.WriteLine($"Name: {name}");
                    writer.WriteLine($"Email: {email}");
                    writer.WriteLine($"Message: {message}");
                    writer.WriteLine();

                    myMessage.Body = "Someone submitted their contact information on SouthMountainWebsites.com.\n";
                    myMessage.Body += "Date: " + azTime.ToString() + "\n";
                    myMessage.Body += "Name: " + name + "\n";
                    myMessage.Body += "Email: " + email + "\n";
                    myMessage.Body += "Message: " + message;

                    client.Send(myMessage);

                }

                // Return a success response
                // return Ok();

                // Return a success response
                var response = new
                {
                    Success = true,
                    Message = "Thank you. I'll be reaching out to you very soon."
                };
                return Json(response);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing contact form data to file");
                return StatusCode(500);
            }
        }
    }

