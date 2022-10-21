using Microsoft.AspNetCore.Mvc;

namespace file_validator_app.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;

    public FileController(ILogger<FileController> logger)
    {
        _logger = logger;
    }

/*
    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55)
        })
        .ToArray();
    }
*/
    [HttpPost]
    public async Task<IActionResult> ImportFile([FromForm] IFormFile file) {
        
          _logger.LogInformation("file import started...");

        string name = file.FileName;

        _logger.LogInformation(name + " file import started...");

        //read the file
        await using(var memoryStream = new MemoryStream()) {
            file.CopyTo(memoryStream);
        }

        _logger.LogInformation(name + " file import completed...");

        return NoContent();
    }

}