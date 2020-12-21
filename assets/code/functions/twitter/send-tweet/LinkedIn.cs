using System;
using System.Threading.Tasks;
using Sparkle.LinkedInNET;
using Sparkle.LinkedInNET.Common;
using Sparkle.LinkedInNET.UGCPost;

namespace AzureLowlands.Functions.Twitter
{
    public static class LinkedIn
    {
        private static string accessToken = SendTweet.Configuration["linkedInAccessToken"];

        public static async Task PublishMessage(string text, string imageUrl = null)
        {
            var config = new LinkedInApiConfiguration(SendTweet.Configuration["linkedInApiKey"], SendTweet.Configuration["linkedInApiKeySecret"]);
            var api = new LinkedInApi(config);

            var user = new UserAuthorization(accessToken);

            try
            {
                await api.Shares.PostAsync(user, new PostShare()
                {
                    Owner = $"urn:li:person:{SendTweet.Configuration["linkedInUserId"]}",
                    Subject = "Azure Lowlands",
                    Text = new PostShareText()
                    {
                        Text = "Remember, the full line-up for #AzureLowlands is available now, so check https://azurelowlands.com for all details and get your FREE tickets today!"
                    }
                });
            }
            catch (Exception exception)
            {
                var temp = exception;
            }

            /*await api.UGCPost.PostAsync(user, new UGCPostData()
            {
                Author = $"urn:li:person:{SendTweet.Configuration["linkedInUserId"]}",
                LifecycleState = "PUBLISHED",
                Visibility = new UGCPostvisibility()
                {
                    comLinkedinUgcMemberNetworkVisibility = "PUBLIC"
                },
                SpecificContent = new SpecificContent()
                {
                    ComLinkedinUgcShareContent = new ComLinkedinUgcShareContent()
                    {
                        ShareCommentary = new UGCText()
                        {
                            Text = "Remember, the full line-up for #AzureLowlands is available now, so check https://azurelowlands.com for all details and get your FREE tickets today!"
                        },
                        ShareMediaCategory = "NONE"
                    }
                }
            });*/
        }
    }
}