using System.Text.RegularExpressions;
using file_validator_app.Data.Models;

namespace file_validator_app.Data.Services;

public class ZipFileValidatorService : IValidatorService
{
    private readonly ILogger<ZipFileValidatorService> _logger;

    public ZipFileValidatorService(ILogger<ZipFileValidatorService> logger){
         _logger = logger;
    }

    public Structure validateStructure(string name, List<String> fileTree)
    {
        var zipStructure = new ZipFileStructure();

        var validationErrors = new List<String>();
        
        if(!name.Contains(".zip"))
        {
            zipStructure.errors.Add("Uploaded File should be a zip file");
            zipStructure.isValidationFailed = true;
            return zipStructure;
        } else if(fileTree.Count == 0)
        {
            zipStructure.errors.Add("Zip file cannot be empty");
            zipStructure.isValidationFailed = true;
            return zipStructure;
        }
        
        String rootFolderName = fileTree[0].Split("/")[0];
        var dllsContent = new List<String>();
        var imagesContent = new List<String>();
        var languagesContent = new List<String>();

        bool dllFound = false;
        bool imageFound = false;
        bool langFound = false;
        
        foreach(string node in fileTree)
        {
            string[] x = node.Split("/");
            _logger.LogInformation(node);

            // contruct the zip strucuture by populating the directoty contents
            if(x.Length>2)
            {
                    if(string.Equals(x[1],"dlls"))
                    {
                        dllFound = true;
                        
                        if(!string.IsNullOrEmpty(x[2]))
                            zipStructure.dllsContent.Add(x[2]);

                    } else if(string.Equals(x[1],"images"))
                    {
                        imageFound = true;

                        if(!string.IsNullOrEmpty(x[2]))
                            zipStructure.imagesContent.Add(x[2]);

                    } else if(string.Equals(x[1],"languages"))
                    {
                        langFound = true;

                        if(!string.IsNullOrEmpty(x[2]))
                            zipStructure.languagesContent.Add(x[2]);

                    } else 
                    {
                        // if there were directories, other than dlls, images and languages, will add a structure error. 
                        zipStructure.structureErrors.Add(node+ " folder/file does not adhere to the correct strucure.");
                    }
            } 
            
        }

        // if one of the must have directories are missing, will throw an exception immediately. 
        if(!(dllFound && imageFound && langFound)){
            zipStructure.errors.Add("Zip file does not adhere to the correct structure");
            zipStructure.errors.Add("Zip file should contain dlls, images, langauages directories");
            zipStructure.isValidationFailed = true;
            return zipStructure;    
        }

        // validate directory content and populate errors
        var dllErrors = validateDlls(zipStructure.dllsContent, rootFolderName);
        var imagesErrors = validateImages(zipStructure.imagesContent);
        var langErrors = validateLanguages(zipStructure.languagesContent, rootFolderName);

        //populate errors in the zip structure
        zipStructure.dllsErrors.AddRange(dllErrors);
        zipStructure.imagesErrors.AddRange(imagesErrors);
        zipStructure.languagesErrors .AddRange(langErrors);

        // add the validation result to zip strucuture.
        zipStructure.isValidationFailed = zipStructure.errors.Any() || dllErrors.Any() || langErrors.Any() || imagesErrors.Any();
        
        return zipStructure;
    }

    private IEnumerable<string> validateDlls(List<string> dllsContent, String rootFolderName)
    {
        var errors = new List<String>();
        
        if(!dllsContent.Any())
            errors.Add("dlls directory should not be empty");
    
        if(!dllsContent.Contains(rootFolderName+".dll"))
            errors.Add("dlls directory must contain '" +rootFolderName+ ".dll'");

        return errors;
    }

    private IEnumerable<string> validateImages(List<string> imagesContent)
    {
        var errors = new List<String>();
        
        if(!imagesContent.Any())
            errors.Add("No images in ‘images’ directory. Please add images.");

        foreach(string file in imagesContent)
        {
            if(!(file.Contains(".png") || file.Contains(".jpg")))
            {
                errors.Add("File types except .png or .jpg are not allowed in Images directory");
                break;
            }
                
        }

        return errors;
    }

    private IEnumerable<string> validateLanguages(List<string> languagesContent, String rootFolderName)
    {
        var errors = new List<String>();

        if(!languagesContent.Any())
            errors.Add("languages directory should not be empty");

        if(!languagesContent.Contains(rootFolderName +"_en.xml"))
            errors.Add("languages directory must contain " + rootFolderName + "_en.xml");

        var filePatternRegex = new Regex(rootFolderName + "_[a-z]{2}.xml");
        foreach(string file in languagesContent)
        {
            if(!filePatternRegex.IsMatch(file))
            {
                errors.Add("languages directory files should follow  '" + rootFolderName + "_xx.xml format");
                break;
            }
        }

        return errors;
    }
}