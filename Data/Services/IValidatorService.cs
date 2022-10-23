namespace file_validator_app.Data.Services;
using file_validator_app.Data.Models;

public interface IValidatorService
{

    Structure validateStructure(string fileName, List<String> fileTree);

}
