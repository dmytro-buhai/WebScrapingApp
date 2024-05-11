namespace SeleniumScraper.Services
{
    public class ConsoleUIService : IUserInterfaceService
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
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
