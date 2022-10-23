using file_validator_app.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace file_validator_app.Controllers;

[ApiController]
[Route("[controller]")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;

    private IValidatorService iValidatorService;

    public FileController(ILogger<FileController> logger, IValidatorService iValidatorService)
    {
        this._logger = logger;
        this.iValidatorService = iValidatorService;
    }

    [HttpPost]
    public async Task<IActionResult> ImportFile([FromForm] IFormFile file) 
    { 
        var fileTree = new List<String>();

        _logger.LogInformation("file import started...");

        string name = file.FileName;

        // Read the file and populate file tree
        await using(var memoryStream = new MemoryStream()) 
        {
            file.CopyTo(memoryStream);

            var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read, true);
            
            foreach(ZipArchiveEntry entry in archive.Entries)
            {
                fileTree.Add(entry.FullName);
            }
        }

        _logger.LogInformation(name + " file import completed...");
        var zipStructure = iValidatorService.validateStructure(name, fileTree);

        return Ok(zipStructure);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveFile([FromForm] IFormFile file) 
    {
        // Save validated zip file in the 'zips' folder
        var folderName = Path.Combine("zips");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        var fullPath = Path.Combine(pathToSave, file.FileName);
        await using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return Ok();
    }


    [HttpDelete]
    public ActionResult DeleteZips() 
    {
        // Delete files saved in 'zips' folder
        var folderName = Path.Combine("zip");
        var pathToDelte = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        var di = new DirectoryInfo(pathToDelte);

        foreach (FileInfo file in di.GetFiles())
        {       
            file.Delete(); 
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true); 
        }

        return NoContent();
    }
}