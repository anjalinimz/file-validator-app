using file_validator_app.Data.Models;

namespace file_validator_app.Data.Services;

public interface IValidatorService
{
    Structure validateStructure(string fileName, List<String> fileTree);

}
