using System.Diagnostics.CodeAnalysis;

namespace SeleniumScraper.Services;

public interface IUserInterfaceService
{
    public void DisplayMessage(string message);

    public void DisplayMessage([StringSyntax("CompositeFormat")] string format, params object?[]? arg);

    public string? ReadInput(string? optionalMessage = null);
}
