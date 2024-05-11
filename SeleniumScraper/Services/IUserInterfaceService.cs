namespace SeleniumScraper.Services;

public interface IUserInterfaceService
{
    public void DisplayMessage(string message);
    public string? ReadInput(string? optionalMessage = null);
}
