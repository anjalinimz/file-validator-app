namespace file_validator_app.Data.Services;

using System.Text.RegularExpressions;
using file_validator_app.Data.Models;

public class ZipFileValidatorService : IValidatorService
{
    public Structure validateStructure(string name, List<String> fileTree)
    {
        var zipStructure = new ZipFileStructure();

        var validationErrors = new List<String>();
        
        if(!name.Contains(".zip")){
            zipStructure.errors.Add("Uploaded File should be a zip file");
            zipStructure.isValidationFailed = true;
            return zipStructure;
        } else if(fileTree.Count == 0){
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
                    zipStructure.structureErrors.Add(node+ " folder/file does not adhere to the correct strucure");
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