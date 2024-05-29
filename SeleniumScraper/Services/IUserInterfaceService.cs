using System.Diagnostics.CodeAnalysis;

namespace SeleniumScraper.Services;

public interface IUserInterfaceService
{
    public void DisplayMessage(string message);

    public void DisplayMessageInline(string message);

    public void DisplayMessageInline([StringSyntax("CompositeFormat")] string format, params object?[]? arg);

    public void DisplayMessage([StringSyntax("CompositeFormat")] string format, params object?[]? arg);

    public string? ReadInput(string? optionalMessage = null);
}
