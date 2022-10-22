using file_validator_app.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

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

    [HttpPost]
    public async Task<IActionResult> ImportFile([FromForm] IFormFile file) {
        
        var fileTree = new List<String>();

        _logger.LogInformation("file import started...");

        string name = file.FileName;
        _logger.LogInformation(name + " file import started...");

        //read the file and populate file tree
        await using(var memoryStream = new MemoryStream()) {
            file.CopyTo(memoryStream);

            var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read, true);
            
            foreach(ZipArchiveEntry entry in archive.Entries){
                fileTree.Add(entry.FullName);
            }
        }

        _logger.LogInformation(name + " file import completed...");
        var zipStructure = ValidateFileStructure(fileTree);

        return Ok(zipStructure);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SaveFile([FromForm] IFormFile file) {
        
        var folderName = Path.Combine("zip");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        var fullPath = Path.Combine(pathToSave, file.FileName);
        await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Ok();
    }

    private ZipFileStructure ValidateFileStructure(List<String> fileTree){

        var zipStructure = new ZipFileStructure();

        var validationErrors = new List<String>();
        
        if(fileTree.Count == 0){
            zipStructure.errors.Add("Zip file cannot be empty");
            zipStructure.isValidationFailed = true;
            return zipStructure;
        }
        
        String rootFolderName = fileTree[0].Split("/")[0];
        var dllsContent = new List<String>();
        var imagesContent = new List<String>();
        var languagesContent = new List<String>();
        
        foreach(String node in fileTree){
            String[] x = node.Split("/");
            
            if(x.Length>2 && !string.IsNullOrEmpty(x[2])){
                if(string.Equals(x[1],"dlls")){
                    zipStructure.dllsContent.Add(x[2]);
                } else if(string.Equals(x[1],"images")){
                    zipStructure.imagesContent.Add(x[2]);
                } else if(string.Equals(x[1],"languages")){
                    zipStructure.languagesContent.Add(x[2]);
                } else {
                    zipStructure.errors.Add("Zip file structure is invalid. Un-recognized file path : "+ node);
                }
            }
        }

        // validate errors
        var dllErrors = validateDlls(zipStructure.dllsContent, rootFolderName);
        var imagesErrors = validateImages(zipStructure.imagesContent);
        var langErrors = validateLanguages(zipStructure.languagesContent, rootFolderName);

        //populate errors
        zipStructure.dllsErrors.AddRange(dllErrors);
        zipStructure.imagesErrors.AddRange(imagesErrors);
        zipStructure.languagesErrors .AddRange(langErrors);

        zipStructure.isValidationFailed = zipStructure.errors.Any() || dllErrors.Any() || langErrors.Any() || imagesErrors.Any();
        return zipStructure;
    }

    private IEnumerable<string> validateDlls(List<string> dllsContent, String rootFolderName)
    {
        var errors = new List<String>();
        
        if(!dllsContent.Any())
            errors.Add("Dlls should not be empty.");
    
        if(!dllsContent.Contains(rootFolderName+".dll"))
            errors.Add("Dlls directory should have a dll file with the name '" +rootFolderName+ "'");

        return errors;
    }

    private IEnumerable<string> validateImages(List<string> imagesContent)
    {
        var errors = new List<String>();
        
        if(!imagesContent.Any())
            errors.Add("Images should not be empty.");

        foreach(string file in imagesContent)
        {
            if(!(file.Contains(".png") || file.Contains(".jpg")))
                errors.Add("File types except .png or .jpg are not allowed in Images directory");
                break;
        }

        return errors;
    }

    private IEnumerable<string> validateLanguages(List<string> languagesContent, String rootFolderName)
    {
        var errors = new List<String>();

        if(!languagesContent.Any())
            errors.Add("Languages directory should not be empty.");

        if(!languagesContent.Contains(rootFolderName +"_en.xml"))
            errors.Add("Languages directory should have " + rootFolderName + "_en.xml");

        var filePatternRegex = new Regex(rootFolderName + "_[a-z]{2}.xml");
        foreach(string file in languagesContent)
        {
            if(!filePatternRegex.IsMatch(file)){
                errors.Add("Languages directory files should follow  '" + rootFolderName + "_xx.xml format");
                break;
            }
        }

        return errors;
    }

}