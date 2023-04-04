using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]

    public class SaveContactController : Controller
    {
        private readonly ILogger<SaveContactController> _logger;

        public SaveContactController(ILogger<SaveContactController> logger)
        {
            _logger = logger;
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

                    // Define the Los Angeles time zone
                    TimeZoneInfo azTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");

                    // Get the current date and time in Los Angeles time zone
                    DateTimeOffset azTime = TimeZoneInfo.ConvertTime(DateTimeOffset.Now, azTimeZone);


                    writer.WriteLine("--------------------------------");
                    writer.WriteLine($"Date: {DateTime.Now}/az: " + azTime.ToString());
                    writer.WriteLine($"Name: {name}");
                    writer.WriteLine($"Email: {email}");
                    writer.WriteLine($"Message: {message}");
                    writer.WriteLine();
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

