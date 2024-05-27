using System.Diagnostics.CodeAnalysis;

namespace SeleniumScraper.Services
{
    public class ConsoleUIService : IUserInterfaceService
    {
        public void DisplayMessage(string message)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine(message);
        }        
        
        public void DisplayMessage([StringSyntax("CompositeFormat")] string format, params object?[]? arg)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine(format, arg);
        }

        public string? ReadInput(string? optionalMessage)
        {
            if (!string.IsNullOrEmpty(optionalMessage))
            {
                Console.Write(optionalMessage);
            }
            
            var userInput = Console.ReadLine();

            return userInput;
        }
    }
}
