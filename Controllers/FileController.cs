using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;

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

        //read the file
        await using(var memoryStream = new MemoryStream()) {
            file.CopyTo(memoryStream);

            var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read, true);
            
            foreach(ZipArchiveEntry entry in archive.Entries){
                fileTree.Add(entry.FullName);
            }
        }

        _logger.LogInformation(name + " file import completed...");

        return NoContent();
    }

    private List<String> ValidateFileStructure(List<String> fileTree){

        var validationErrors = new List<String>();

        var dllsContent = new List<String>();
        var imagesContent = new List<String>();
        var languagesContent = new List<String>();
        
        if(fileTree.Count == 0){
            validationErrors.Add("Zip file content cannot be empty");
            return validationErrors;
        }
        
        String rootFolderName = fileTree[0].Split("/")[0];

        foreach(String node in fileTree){
            String[] x = node.Split("/");
            
            if(x.Length>2 && !String.IsNullOrEmpty(x[2])){
                if(x[1].Equals("dlls")){
                    dllsContent.Add(x[2]);
                } else if(x[1].Equals("images")){
                    imagesContent.Add(x[2]);
                } else if(x[1].Equals("languages")){
                    languagesContent.Add(x[2]);
                } else {
                    validationErrors.Add("zip file content is not in correct directory format");
                }
            }
        }

        validationErrors.AddRange(validateDlls(dllsContent, rootFolderName));
        validationErrors.AddRange(validateImages(imagesContent));
        validationErrors.AddRange(validateLanguages(languagesContent));

        return validationErrors;

    }

    private IEnumerable<string> validateDlls(List<string> dllsContent, String rootFolderName)
    {
        foreach(string file in dllsContent)
        {
            if(file.)
            {

            }
        }
        throw new NotImplementedException();
    }

private IEnumerable<string> validateImages(List<string> imagesContent)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<string> validateLanguages(List<string> languagesContent)
    {
        throw new NotImplementedException();
    }


}