using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumScraper.CommandsDomain.Abstract;
using SeleniumScraper.Services;

namespace SeleniumScraper.CommandsDomain.Commands;

public class SelectNewsPost(IUserInterfaceService userInterfaceService, 
    EdgeLauncher edgeLauncher, ILogger<SelectNewsPost> logger) : ICommand
{
    public IUserInterfaceService UserInterfaceService { get => userInterfaceService; }

    public string DisplayCommandName => "Select first 10 comments";

    public int Id => KnownCommands.GetPostComments;

    private EdgeDriver _edgeDriver { get => edgeLauncher.GetEdgeDriver(); }

    public void ExecuteCommand()
    {
        var commentsCountInString = UserInterfaceService.ReadInput("Enter number of comments to read: ");
        int.TryParse(commentsCountInString, out int commentsCount);

        logger.LogInformation("Start reading comments...");
        var parsedComments = GetComments(commentsCount);

        if (parsedComments.Count > 0)
        {
            UserInterfaceService.DisplayMessage(string.Join("\n", parsedComments));
        }
        else
        {
            var noCommentsToDisplayMessage = $"No comments to display. The requested number of comments is {commentsCountInString}";
            UserInterfaceService.DisplayMessage(noCommentsToDisplayMessage);
        }
        
        logger.LogInformation("Operation is finished.");
    }

    public List<string> GetComments(int commentsCount)
    {
        if (commentsCount is not int or not > 0)
        {
            return [];
        }

        var commentsList = new List<string>();

        List<string> UserTags = [];
        List<string> TimeStamps = [];
        List<string> Tweets = [];
        List<string> Replies = [];
        List<string> Retweets = [];
        List<string> Likes = [];

        if (_edgeDriver.Url.Contains("x.com") || _edgeDriver.Url.Contains("twitter.com"))
        {
            try
            {
                while (Tweets.Distinct().Count() <= 10)
                {
                    // Wait for page to load using WebDriverWait for better handling
                    WebDriverWait wait = new WebDriverWait(_edgeDriver, TimeSpan.FromSeconds(10));
                    wait.Until(driver => driver.FindElements(By.CssSelector("article[data-testid='tweet']")).Count > 0);
                    var articles = _edgeDriver.FindElements(By.CssSelector("article[data-testid='tweet']"));

                    foreach (var article in articles)
                    {
                        try
                        {
                            string tweetText = article.FindElementSafe(By.CssSelector("div[lang]"))?.Text ?? string.Empty;

                            if (!string.IsNullOrEmpty(tweetText) && !Tweets.Contains(tweetText))
                            {
                                Tweets.Add(tweetText);

                                string userTag = article.FindElementSafe(By.CssSelector("div[data-testid='User-Name'] > div"))?.Text ?? string.Empty;
                                UserTags.Add(userTag);

                                string timeStamp = article.FindElementSafe(By.CssSelector("time")).GetAttribute("datetime") ?? string.Empty;
                                TimeStamps.Add(timeStamp);

                                string replyCount = article.FindElementSafe(By.CssSelector("button[data-testid='reply']"))?.Text ?? string.Empty;
                                Replies.Add(replyCount);

                                string retweetCount = article.FindElementSafe(By.CssSelector("button[data-testid='retweet']"))?.Text ?? string.Empty;
                                Retweets.Add(replyCount);

                                string likeCount = article.FindElementSafe(By.CssSelector("button[data-testid='like']"))?.Text ?? string.Empty;
                                Likes.Add(likeCount);
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
            }
            catch (Exception ex) 
            {
                logger.LogError($"An exception occurred while reading the comments: {ex.Message}");
            }
        }

        // Remove \r\n from all elements in the lists
        UserTags = RemoveSymbols(UserTags);
        TimeStamps = FormatTimestamps(RemoveSymbols(TimeStamps));
        Tweets = RemoveSymbols(Tweets);
        Replies = RemoveSymbols(Replies);
        Retweets = RemoveSymbols(Retweets);
        Likes = RemoveSymbols(Likes);

        // Print header
        UserInterfaceService.DisplayMessage("{0,-30} {1,-30} {2,-50} {3,-10} {4,-10} {5,-10}",
            nameof(UserTags),
            nameof(TimeStamps),
            nameof(Tweets),
            nameof(Replies),
            nameof(Retweets),
            nameof(Likes));

        // Print separator line
        UserInterfaceService.DisplayMessage(new string('-', 110));

        // Print data rows
        for (int i = 0; i < UserTags.Count; i++)
        {
            UserInterfaceService.DisplayMessage("{0,-30} {1,-30} {2,-50} {3,-10} {4,-10} {5,-10}",
                UserTags[i],
                TimeStamps[i],
                Tweets[i].Length > 47 ? Tweets[i].Substring(0, 47) + "..." : Tweets[i],
                Replies[i],
                Retweets[i],
                Likes[i]);
        }

        foreach (var comment in Tweets.Distinct())
        {
            commentsList.Add(comment);
        }

        return commentsList;
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
