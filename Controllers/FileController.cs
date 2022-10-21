using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace file_validator_app.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;

    [HttpPost("ImportFile")]
    public async Task <IActionResult> ImportFile([FromForm] IFormFile file) {
        
        string name = file.FileName;
        string extension = Path.GetExtension(file.FileName);

        _logger.LogInformation(name + extension + " file import started...");

        //read the file
        await using(var memoryStream = new MemoryStream()) {
            file.CopyTo(memoryStream);
        }

        _logger.LogInformation(name + extension + " file import completed...");

        return NoContent();
    }

}