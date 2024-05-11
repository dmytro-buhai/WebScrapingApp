using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
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
        logger.LogInformation("Start reading messages...");
        UserInterfaceService.DisplayMessage(string.Join("\n", GetComments()));
        logger.LogInformation("Operation is finished.");
    }

    public List<string> GetComments()
    {
        var commentsList = new List<string>();

        List<string> UserTags = new List<string>();
        List<string> TimeStamps = new List<string>();
        List<string> Tweets = new List<string>();
        List<string> Replys = new List<string>();
        List<string> reTweets = new List<string>();
        List<string> Likes = new List<string>();

        if (_edgeDriver.Url.Contains("twitter.com"))
        {
            try
            {
                //var tweet = _edgeDriver.FindElement(By.CssSelector("div[data-testid='tweetText']"));

                while (Tweets.Distinct().Count() <= 10)
                {
                    var articles = _edgeDriver.FindElements(By.XPath("//article[@data-testid='tweet']"));

                    foreach (var article in articles)
                    {
                        try
                        {
                            string Tweet = article.FindElement(By.XPath(".//div[@data-testid='tweetText']")).Text;
                            if (!string.IsNullOrEmpty(Tweet))
                            {
                                Tweets.Add(Tweet);

                                string TimeStamp = article.FindElement(By.XPath(".//time")).GetAttribute("datetime");
                                TimeStamps.Add(TimeStamp);

                                string Reply = article.FindElement(By.XPath(".//div[@data-testid='reply']")).Text;
                                Replys.Add(Reply);

                                string reTweet = article.FindElement(By.XPath(".//div[@data-testid='retweet']")).Text;
                                reTweets.Add(reTweet);

                                string Like = article.FindElement(By.XPath(".//div[@data-testid='like']")).Text;
                                Likes.Add(Like);
                            }
                        }
                        catch
                        {
                            // Probably some image...
                        }
                    }

                    Actions actions = new Actions(_edgeDriver);
                    actions.MoveToElement(articles.Last());
                    actions.Perform();

                    Thread.Sleep(1000);
                };
            }
            catch (Exception ex) 
            {
                var t = ex.Message;
            }
        }

        foreach (var comment in Tweets.Distinct())
        {
            commentsList.Add(comment);
        }

        return commentsList;
    }
}
