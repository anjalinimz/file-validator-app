namespace file_validator_app.Models;

public class ZipFileStructure
{

    public ZipFileStructure()
    {
        this.dllsContent = new List<string>();
        this.imagesContent = new List<string>();
        this.languagesContent = new List<string>();
        this.dllsErrors = new List<string>();
        this.imagesErrors = new List<string>();
        this.languagesErrors = new List<string>();
        this.errors = new List<string>();
    }

    public List<string> dllsContent {get; set;}

    public List<string> imagesContent {get; set;} 

    public List<string> languagesContent {get; set;} 

     public List<string> dllsErrors {get; set;}

    public List<string> imagesErrors {get; set;} 

    public List<string> languagesErrors {get; set;} 

    public List<string> errors {get; set;} 

}
