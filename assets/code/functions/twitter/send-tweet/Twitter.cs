using System.Collections.Generic;
using System.Linq;
using System.Net;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace AzureLowlands.Functions.Twitter
{
    public class Twitter
    {
        private static TwitterClient _userClient;

        public Twitter()
        {
            var consumerKey = SendTweet.Configuration["TwitterApiKey"];
            var consumerSecret = SendTweet.Configuration["TwitterApiSecret"];
            var accessToken = SendTweet.Configuration["TwitterAccestoken"];
            var accessTokenSecret = SendTweet.Configuration["TwitterAccestokenSecret"];
            _userClient = new TwitterClient(consumerKey, consumerSecret, accessToken, accessTokenSecret);
        }

        public async void PublishTweet(string text, string imageUrl = null)
        {
            var publishTweetParameters = new PublishTweetParameters(text);

            if (string.IsNullOrWhiteSpace(imageUrl) == false)
            {
                var response = await WebRequest.Create(imageUrl).GetResponseAsync();
                var allBytes = new List<byte>();
                using (var imageStream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[4000];
                    int bytesRead;
                    while ((bytesRead = await imageStream.ReadAsync(buffer, 0, 4000)) > 0)
                    {
                        allBytes.AddRange(buffer.Take(bytesRead));
                    }
                }

                var media = await _userClient.Upload.UploadBinaryAsync(allBytes.ToArray());
                publishTweetParameters = new PublishTweetParameters(text)
                {
                    Medias = new List<IMedia> { media }
                };
            }

            await _userClient.Tweets.PublishTweetAsync(publishTweetParameters);
        }
    }
}