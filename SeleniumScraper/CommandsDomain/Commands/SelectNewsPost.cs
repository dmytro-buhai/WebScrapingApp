using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumScraper.CommandsDomain.Abstract;
using SeleniumScraper.Services;
using System.Data;
using System.Text;

namespace SeleniumScraper.CommandsDomain.Commands;

public class SelectNewsPost(ProcessedData processedData, IUserInterfaceService userInterfaceService, 
    EdgeLauncher edgeLauncher, ILogger<SelectNewsPost> logger) : ICommand
{
    public IUserInterfaceService UserInterfaceService { get => userInterfaceService; }

    public string DisplayCommandName => "Read comments";

    public int Id => KnownCommands.GetPostComments;

    private EdgeDriver _edgeDriver { get => edgeLauncher.GetEdgeDriver(); }

    private const string DataFilePath = @"C:\dev\temp";

    private List<string> UserNames = [];
    private List<string> Dates = [];
    private List<string> Comments = [];
    private List<string> Replies = [];
    private List<string> Retweets = [];
    private List<string> Likes = [];

    public void ExecuteCommand()
    {
        var commentsCountInString = UserInterfaceService.ReadInput("Enter min number of comments to read: ");
        int.TryParse(commentsCountInString, out int commentsCount);

        logger.LogInformation("Start reading comments...");
        GetComments(commentsCount);

        if (Comments.Count > 0)
        {
            logger.LogInformation("Operation was completed successfully. Data stored in memory. Execute 'Save' command to save the data to a file.");
        }
        else
        {
            var noCommentsToDisplayMessage = $"No comments to display. The requested number of comments is {commentsCountInString}";
            UserInterfaceService.DisplayMessage(noCommentsToDisplayMessage);
        }
    }

    public void GetComments(int commentsCount)
    {
        UserNames.Clear();
        Dates.Clear();
        Comments.Clear();
        Replies.Clear();
        Retweets.Clear();
        Likes.Clear();

        if (commentsCount is not int or not > 0)
        {
            return;
        }

        if (_edgeDriver.Url.Contains("x.com") || _edgeDriver.Url.Contains("twitter.com"))
        {
            try
            {
                while (Comments.Distinct().Count() <= commentsCount)
                {
                    // Wait for page to load using WebDriverWait for better handling
                    WebDriverWait wait = new WebDriverWait(_edgeDriver, TimeSpan.FromSeconds(10));
                    wait.Until(driver => driver.FindElements(By.CssSelector("article[data-testid='tweet']")).Count > 0);
                    var articles = _edgeDriver.FindElements(By.CssSelector("article[data-testid='tweet']"));

                    foreach (var article in articles)
                    {
                        try
                        {
                            var tweetText = article.FindElementSafe(By.CssSelector("div[lang]"))?.Text ?? string.Empty;
 
                            if (!string.IsNullOrEmpty(tweetText))
                            {
                                tweetText = tweetText.Replace("Translate with DeepL", string.Empty);

                                if (!Comments.Contains(tweetText))
                                {
                                    Comments.Add(tweetText);

                                    string userTag = article.FindElementSafe(By.CssSelector("div[data-testid='User-Name'] > div"))?.Text ?? string.Empty;
                                    UserNames.Add(userTag);

                                    string timeStamp = article.FindElementSafe(By.CssSelector("time")).GetAttribute("datetime") ?? string.Empty;
                                    Dates.Add(timeStamp);

                                    string replyCount = article.FindElementSafe(By.CssSelector("button[data-testid='reply']"))?.Text ?? string.Empty;
                                    Replies.Add(replyCount);

                                    string retweetCount = article.FindElementSafe(By.CssSelector("button[data-testid='retweet']"))?.Text ?? string.Empty;
                                    Retweets.Add(replyCount);

                                    string likeCount = article.FindElementSafe(By.CssSelector("button[data-testid='like']"))?.Text ?? string.Empty;
                                    Likes.Add(likeCount);
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            logger.LogError($"An exception occurred while using FindElement By: {ex.Message}");
                        }
                    }

                    Actions actions = new(_edgeDriver);
                    actions.MoveToElement(articles.Last());
                    actions.Perform();

                    Thread.Sleep(1000);
                };

                // Remove \r\n from all elements in the lists
                UserNames = RemoveSymbols(UserNames);
                Dates = FormatTimestamps(RemoveSymbols(Dates));
                Comments = RemoveSymbols(Comments);
                Replies = RemoveSymbols(Replies);
                Retweets = RemoveSymbols(Retweets);
                Likes = RemoveSymbols(Likes);

                // Initialize the DataTable
                processedData.Data = new DataTable();
                processedData.Data.Columns.Add("UserName");
                processedData.Data.Columns.Add("Date");
                processedData.Data.Columns.Add("Comment");
                processedData.Data.Columns.Add("Replies");
                processedData.Data.Columns.Add("Retweets");
                processedData.Data.Columns.Add("Likes");

                // Add data to the DataTable
                for (var i = 0; i < Comments.Count; i++)
                {
                    processedData.Data.Rows.Add(UserNames[i], Dates[i], Comments[i], Replies[i], Retweets[i], Likes[i]);
                }
            }
            catch (Exception ex) 
            {
                logger.LogError($"An exception occurred while reading the comments: {ex.Message}");
            }
        }
    }

    static List<string> RemoveSymbols(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = list[i].Replace("\r\n", "");
        }
        return list;
    }

    static List<string> FormatTimestamps(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            DateTime parsedTimestamp = DateTime.Parse(list[i]);
            list[i] = parsedTimestamp.ToString("yyyy-MM-dd HH:mm tt");
        }
        return list;
    }
}
